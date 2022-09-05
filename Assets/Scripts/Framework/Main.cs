/*
 * 
 *      Title:框架启动脚本
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        //热更新之前初始化一些模块
        InitBeforeHotUpdate();
    }

    /// <summary>
    /// 热更新之前初始化一些模块
    /// </summary>
    private void InitBeforeHotUpdate()
    {
        // 限制游戏帧数
        Application.targetFrameRate = GlobalsDefine.GAME_FRAME_RATE;
        // 手机常亮
        Screen.sleepTimeout = -1;
        // 后台运行
        Application.runInBackground = true;

        //日志
        GameLogger.Init();
        LogCat.Init();
    }
}
