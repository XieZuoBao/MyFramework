/*
 * 
 *      Title:  基础框架
 *             
 *      Description: 
 *              基础框架的全局常量
 *              
 *                          
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalsDefine
{
    /// <summary>
    /// 对象池脚本依附的对象节点名称
    /// </summary>
    public const string POOL_GO_NAME = "Pool";

    /// <summary>
    /// 背景音乐脚本依附的对象节点名称
    /// </summary>
    public const string BG_MUSIC_GO_NAME = "BgMusic";

    /// <summary>
    /// 音效脚本依附的对象节点名称
    /// </summary>
    public const string SOUND_GO_NAME = "Sound";

    /// <summary>
    /// UI层级根节点_底层(Bottom)
    /// </summary>
    public const string UI_BOTTOM_NODE_NAME = "Bottom";

    /// <summary>
    /// UI层级根节点_中间层(Middle)
    /// </summary>
    public const string UI_MIDDLE_NODE_NAME = "Middle";

    /// <summary>
    /// UI层级根节点_顶层(Top)
    /// </summary>
    public const string UI_TOP_NODE_NAME = "Top";

    /// <summary>
    /// UI层级根节点_系统层(System,最顶层)
    /// </summary>
    public const string UI_SYSTEM_NODE_NAME = "System";

    /// <summary>
    /// UICanvas预制路径
    /// </summary>
    public const string UI_CANVAS_PREFAB_PATH = "Prefabs/UI/Common/UICanvas";

    /// <summary>
    /// 背景音乐默认音量大小
    /// </summary>
    public const float DEFAULT_BG_MUSIC_VOLUME = 1.0F;

    /// <summary>
    /// 声效默认音量大小
    /// </summary>
    public const float DEFAULT_SOUND_VOLUME = 1.0F;
}