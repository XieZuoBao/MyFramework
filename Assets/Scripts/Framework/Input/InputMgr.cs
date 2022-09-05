/*
 * 
 *      Title:  基础框架
 * 
 *             
 *      Description: 
 *              输入控制模块
 *              管理input输入的相关逻辑           
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseSingleton<InputMgr>
{
    private bool isStart = false;
    /// <summary>
    /// 在构造函数中添加Upate监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 是否需要开启/关闭按键输入检测
    /// </summary>
    /// <returns></returns>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    private void MyUpdate()
    {
        if (!isStart)
            return;

        #region 测试代码
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        #endregion

    }

    /// <summary>
    /// 检测某键按下抬起状态
    /// </summary>
    /// <param name="key">按键</param>
    /// <param name="eventName">分发的事件名</param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            #region 测试代码
            //分发按键按下事件
            EventCenter.Instance.EventTrigger("按键按下", key);
            #endregion
        }
        if (Input.GetKeyUp(key))
        {
            #region 测试代码
            //分发按键抬起事件
            EventCenter.Instance.EventTrigger("按键抬起", key);
            #endregion
        }
    }
}