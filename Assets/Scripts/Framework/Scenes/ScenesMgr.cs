using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEngine;

/// <summary>
/// 场景加载模块:给外部提供同步,异步加载场景的方法
/// </summary>
public class ScenesMgr : BaseSingleton<ScenesMgr>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadScene(string sceneName, UnityAction action)
    {
        //同步加载场景
        SceneManager.LoadScene(sceneName);
        //场景加载完成的委托函数
        action.Invoke();
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    public void LoadSceneAsync(string sceneName, UnityAction action)
    {
        MonoMgr.Instance.StartCoroutine(WaitLoadSceneAsync(sceneName, action));
    }

    /// <summary>
    /// 协程--异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator WaitLoadSceneAsync(string sceneName, UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        while (!ao.isDone)
        {
            //分发事件
            //EventCenter.Instance.EventTrigger("Loading", ao.progress);
            //挂起一帧
            yield return ao.progress;
        }
        //异步加载完成后的委托函数
        action.Invoke();
    }
}