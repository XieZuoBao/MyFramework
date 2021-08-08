/*
 * 
 *      Title:  基础框架
 *           
 *      Description: 
 *              背景音乐,音效管理模块
 *              统一管理背景音乐,音效相关的逻辑
 *              ***音效的对象池管理还没有实现***     
 ***/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseSingleton<MusicMgr>
{
    private string bgMusicGoName = GlobalsDefine.BG_MUSIC_GO_NAME;      //背景音效组件依附对象(new出来的)的名称
    private AudioSource bgMusic;                                        //背景音乐组件
    private float bgValue = GlobalsDefine.DEFAULT_BG_MUSIC_VOLUME;      //背景音乐音量

    private string soundGoName = GlobalsDefine.SOUND_GO_NAME;           //音效组件依附的游戏对象(new出来的)的名称
    private GameObject soundGo;                                         //音效组件依附的游戏对象
    private List<AudioSource> soundList = new List<AudioSource>();      //音效集合
    private float soundValue = GlobalsDefine.DEFAULT_SOUND_VOLUME;      //音效音量

    public MusicMgr()
    {
        MonoMgr.Instance.AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 处理音效播放完毕的逻辑
    /// </summary>
    private void MyUpdate()
    {
        for (int i = soundList.Count - 1; i >= 0; i--)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    #region 背景音乐
    public void PlayBgMusic(string path)
    {
        if (bgMusic == null)
        {
            GameObject go = new GameObject(bgMusicGoName);
            bgMusic = go.AddComponent<AudioSource>();
        }
        //异步加载背景音乐,加载完成播放
        ResMgr.Instance.LoadResourceAsync<AudioClip>(path, (clip) =>
        {
            bgMusic.clip = clip;
            bgMusic.loop = true;
            bgMusic.playOnAwake = false;
            bgMusic.volume = bgValue;
            bgMusic.Play();
        });
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBgMusic()
    {
        if (bgMusic == null)
            return;
        bgMusic.Stop();
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBgMusic()
    {
        if (bgMusic == null)
            return;
        bgMusic.Pause();
    }

    /// <summary>
    /// 设置背景音乐音量大小
    /// </summary>
    public void SetBgMusicValue(float volume)
    {
        bgValue = volume;
        if (bgMusic == null)
            return;
        bgMusic.volume = volume;
    }
    #endregion

    #region 音效
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">音效路径</param>
    /// <param name="isLoop">音效是否循环</param>
    /// <param name="callBack">加载完音效后的委托</param>
    public void PlaySound(string path, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        //实例化音效依附的游戏对象
        if (soundGo == null)
            soundGo = new GameObject(soundGoName);
        //异步加载音效,加载完毕后给游戏对象添加音效组件
        ResMgr.Instance.LoadResourceAsync<AudioClip>(path, (clip) =>
        {
            AudioSource source = soundGo.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = isLoop;
            source.playOnAwake = false;
            source.volume = soundValue;
            source.Play();
            soundList.Add(source);
            if (callBack != null)
                callBack.Invoke(source);
        });
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            source.Stop();
            soundList.Remove(source);
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetSoundValue(float volume)
    {
        soundValue = volume;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = volume;
        }
    }
    #endregion
}