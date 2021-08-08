/*
 * 
 *      Title:
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTest : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 0, 200, 100), "播放背景音乐"))
            MusicMgr.Instance.PlayBgMusic("Music/Bg/bg2");

        if (GUI.Button(new Rect(100, 120, 200, 100), "暂停背景音乐"))
            MusicMgr.Instance.PauseBgMusic();

        if (GUI.Button(new Rect(100, 240, 200, 100), "停止背景音乐"))
            MusicMgr.Instance.StopBgMusic();

        if (GUI.Button(new Rect(100, 360, 200, 100), "播放音效"))
            MusicMgr.Instance.PlaySound("Music/Sounds/1", false);

    }
}