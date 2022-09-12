using UnityEngine;

#region 系统枚举类型
/// <summary>
/// UI窗体位置类型
/// </summary>
public enum UIPanelType
{
    /// <summary>
    /// 全屏窗体
    /// </summary>
    Panel,
    ///// <summary>
    ///// 固定窗体
    ///// </summary>
    //Fixed,
    /// <summary>
    /// 弹出窗体
    /// </summary>
    Popup,
}

#endregion

/// <summary>
/// 系统常量,系统枚举,委托定义等
/// </summary>
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

    #region Canvas节点常量
    /// <summary>
    /// 全屏窗体挂点
    /// </summary>
    public const string CANVAS_NORMAL_NODE_NAME = "Normal";
    /// <summary>
    /// 固定窗体挂点(暂未用到)
    /// </summary>
    public const string CANVAS_FIXED_NODE_NAME = "Fixed";
    /// <summary>
    /// 弹出窗体挂点
    /// </summary>
    public const string CANVAS_POPUP_NODE_NAME = "Popup";
    /// <summary>
    /// 弹出窗体遮罩挂点
    /// </summary>
    public const string CANVAS_POPUP_MASK_NODE_NAME = "PopupMask";
    /// <summary>
    /// UICamera挂点
    /// </summary>
    public const string CANVAS_UI_CAMERA_NODE_NAME = "UICamera";
    #endregion

    #region 遮罩透明度
    /// <summary>
    /// 半透明遮罩配色
    /// </summary>
    public static readonly Color LUCENCY_COLOR = new Color(0, 0, 0, 128.0f / 255);
    #endregion

    #region 预设路径常量
    /// <summary>
    /// UICanvas预制路径
    /// </summary>
    public const string UI_CANVAS_PREFAB_PATH = "Assets/GAssets/Launch/Res/UI/Base/UICanvas.prefab";
    #endregion

    #region 配置文件常量
    /// <summary>
    /// 多语言配置文件
    /// </summary>
    public const string I18N_APP_STRINGS_PATH = "Assets/GAssets/Launch/Res/Config/i18nAppStrings.json";
    /// <summary>
    /// UI面板配置文件
    /// </summary>
    public const string UI_PANTL_CONFIG_PATH = "Assets/GAssets/Launch/Res/Config/UIPanelConfig.json";
    #endregion

    #region 标签tag

    #endregion

    /// <summary>
    /// 背景音乐默认音量大小
    /// </summary>
    public const float DEFAULT_BG_MUSIC_VOLUME = 1.0F;

    /// <summary>
    /// 声效默认音量大小
    /// </summary>
    public const float DEFAULT_SOUND_VOLUME = 1.0F;

    /// <summary>
    /// 游戏帧频
    /// </summary>
    public const int GAME_FRAME_RATE = 30;

    /// <summary>
    /// 语言类型
    /// </summary>
    public const string LANGUAGE_TYPE = "LANGUAGE_TYPE";
}