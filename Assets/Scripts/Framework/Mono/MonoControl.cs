/*
 * 
 *      Title:  基础框架
 * 
 *             
 *      Description: 
 *              公共Mono模块
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoControl : MonoBehaviour
{
    private event UnityAction updateEvent;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (updateEvent != null)
            updateEvent.Invoke();
    }

    /// <summary>
    /// 给MonoMgr脚本提供的添加帧更新事件的函数
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 给MonoMgr脚本提供的用于移除帧更新事件函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateLinstener(UnityAction action)
    {
        updateEvent -= action;
    }
}