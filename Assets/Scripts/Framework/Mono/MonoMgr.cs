using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono模块
/// <para>1.提供给外部(没有继承MonoBehaviour的类)添加帧更新事件的方法</para>
/// <para>2.提供给外部(没有继承MonoBehaviour的类)添加协程的方法</para>
/// </summary>
public class MonoMgr : BaseSingleton<MonoMgr>
{
    private MonoControl monoControl;

    public MonoMgr()
    {
        GameObject go = new GameObject(typeof(MonoControl).ToString());
        monoControl = go.AddComponent<MonoControl>();
    }

    /// <summary>
    /// 给外部提供的添加帧更新事件的函数
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        monoControl.AddUpdateListener(action);
    }

    /// <summary>
    /// 给外部提供的用于移除帧更新事件函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateLinstener(UnityAction action)
    {
        monoControl.RemoveUpdateLinstener(action);
    }

    #region 协程
    #region 开启协程
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoControl.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName)
    {
        return monoControl.StartCoroutine(methodName);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return monoControl.StartCoroutine(methodName, value);
    }
    #endregion

    #region 停止协程
    public void StopAllCoroutines()
    {
        monoControl.StopAllCoroutines();
    }

    public void StopCoroutine(string methodName)
    {
        monoControl.StopCoroutine(methodName);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        monoControl.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        monoControl.StopCoroutine(routine);
    }
    #endregion
    #endregion
}