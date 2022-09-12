/*
 * 
 *      Title:框架启动脚本
 * 
 *             
 *      Description:  
 *           
 *              
 ***/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        //热更新之前初始化一些模块
        InitBeforeHotUpdate();

        //TODO热更新

        //热更新后初始化一些模块
        InitAfterHotUpdate();
    }

    /// <summary>
    /// 热更新之前初始化一些模块
    /// </summary>
    private void InitBeforeHotUpdate()
    {
        // TODO 网络消息初始化

        // 限制游戏帧数
        Application.targetFrameRate = GlobalsDefine.GAME_FRAME_RATE;
        // 手机常亮
        Screen.sleepTimeout = -1;
        // 后台运行
        Application.runInBackground = true;

        //日志
        GameLogger.Init();
        LogCat.Init();
        //TODO 网络消息注册

        //界面管理器
        UIMgr.Instance.Init();

        //TODO 版本号

        //TODO 预加载AssetBundle

        //TODO 加载必要的资源AssetBundle

        //TODO 定时器

        //TODO 客户端网络连接

        //TODO 截屏
    }

    /// <summary>
    /// 热更新后初始化一些模块
    /// </summary>
    private void InitAfterHotUpdate()
    {
        //TODO 资源管理器

        //TODO 音效管理器

        // 多语言
        LanguageMgr.Instance.Init();
        I18N.Instance.Init();
        //测试代码
        RedPointTestData.Instance.Init();
        UIMgr.Instance.ShowPanel<APanel>(UIPanelType.Panel);

        //图集管理器
    }
}

[Serializable]
public class Hero
{
    public string Name;
    public Level MyLevel;
}

[Serializable]
public class Level
{
    public int HeroLevel;
}
