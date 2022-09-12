using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI管理器
/// <para>1.管理所有的UI面板,包括普通窗口和弹出窗口</para>
/// <para>2.提供切换面板和显示弹出框的功能函数,切换窗口时会自动关闭当前窗口</para>
/// <para>3.弹出窗口由于可以和普通窗口重叠显示，所以使用额外的列表进行存储</para>
/// </summary>
public class UIMgr : BaseSingleton<UIMgr>
{
    /// <summary>
    /// UI窗体预设路径列表
    /// <para>key:窗体预设名称;value:窗体预设路径</para>
    /// </summary>
    private Dictionary<string, string> panelPathDic;
    /// <summary>
    /// 所有UI窗体列表
    /// <para>key:窗体预设名称;value:窗体脚本</para>
    /// </summary>
    private Dictionary<string, UIBasePanel> allPanelDic;
    /// <summary>
    /// 当前显示的UI窗体列表
    /// <para>key:窗体预设名称;value:窗体脚本</para>
    /// </summary>
    private Dictionary<string, UIBasePanel> curShowUIPanelDic;
    /// <summary>
    /// "反向切换"UI窗体列表
    /// </summary>
    private Stack<UIBasePanel> stackPopupPanels;
    //====================几个重要的节点Start===========================
    /// <summary>
    /// UI根节点Canvas
    /// </summary>
    private Transform transCanvas;
    /// <summary>
    /// 全屏窗体挂点
    /// </summary>
    private Transform transNormal;
    /// <summary>
    /// 固定窗体挂点
    /// </summary>
    private Transform transFixed;
    /// <summary>
    /// 弹出窗体挂点
    /// </summary>
    private Transform transPopup;
    /// <summary>
    /// 弹出窗体的遮罩
    /// </summary>
    private Transform transPopupMask;
    //====================几个重要的节点End=============================

    private Camera uiCamera;
    private float originalUICameraDepth;

    public void Init()
    {
        panelPathDic = new Dictionary<string, string>();
        allPanelDic = new Dictionary<string, UIBasePanel>();
        curShowUIPanelDic = new Dictionary<string, UIBasePanel>();
        stackPopupPanels = new Stack<UIBasePanel>();
        //动态加载UICanvas预设到场景中
        GameObject canvasGo = AssetLoader.Instance.Clone("Launch", GlobalsDefine.UI_CANVAS_PREFAB_PATH);
        //窗体挂点
        transCanvas = canvasGo.transform;
        transNormal = transCanvas.GetChildByName(GlobalsDefine.CANVAS_NORMAL_NODE_NAME);
        transFixed = transCanvas.GetChildByName(GlobalsDefine.CANVAS_FIXED_NODE_NAME);
        transPopup = transCanvas.GetChildByName(GlobalsDefine.CANVAS_POPUP_NODE_NAME);
        transPopupMask = transCanvas.GetChildByName(GlobalsDefine.CANVAS_POPUP_MASK_NODE_NAME);
        //默认隐藏遮罩
        transPopupMask.gameObject.SetActive(false);
        //UICamera
        uiCamera = transCanvas.GetChildByName(GlobalsDefine.CANVAS_UI_CAMERA_NODE_NAME).GetComponent<Camera>();
        originalUICameraDepth = uiCamera.depth;
        //过场景时不移除UICanvas预设
        GameObject.DontDestroyOnLoad(canvasGo);
        //加载UI窗体预设路径的配置文件
        InitUIPanelConfig();
    }

    /// <summary>
    /// 根据UI窗体名称显示窗体
    /// </summary>
    /// <param name="panelName">要显示的UI窗体名称</param>
    public void ShowPanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;
        //根据UI窗体名称,加载窗体到所有UI窗体列表allPanelDic中
        UIBasePanel panel = LoadPanelToAllPanelDic(panelName);
        if (panel == null)
            return;

        //是否清空"反向切换"UI窗体列表中的数据
        if (panel.CurUIInfo.IsClearStack)
        {
            if (stackPopupPanels != null && stackPopupPanels.Count >= 1)
            {
                //清空"反向切换"UI窗体列表中的数据
                stackPopupPanels.Clear();
            }
        }

        //根据不同的UI窗体显示模式,做不同的处理
        switch (panel.CurUIInfo.showMode)
        {
            case UIPanelShowMode.Normal:
                //显示全屏窗体:当前显示的UI窗体列表curShowUIPanelDic中"加入"UI窗体
                ShowNormalPanel(panelName);
                break;
            case UIPanelShowMode.ReverseChange:
                //显示反向切换窗体(Popup):"反向切换"UI窗体列表stackPopupPanels入栈UI窗体 
                ShowReversePanel(panelName);
                break;
            case UIPanelShowMode.HideOther:
                //显示指定窗体并隐藏其他窗体:
                ShowHideOtherPanel(panelName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 根据UI窗体名称关闭窗体
    /// </summary>
    /// <param name="panelName">要关闭的UI窗体名称</param>
    public void HidePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        //所有UI窗体列表中有则处理后续,没有则返回
        UIBasePanel closePanel;
        allPanelDic.TryGetValue(panelName, out closePanel);
        if (closePanel == null)
            return;
        //根据不同的UI窗体显示模式,做不同的处理
        switch (closePanel.CurUIInfo.showMode)
        {
            case UIPanelShowMode.Normal:
                //关闭普通UI窗体
                CloseNormalPanel(panelName);
                break;
            case UIPanelShowMode.ReverseChange:
                //关闭反向切换窗体(Popup):"反向切换"UI窗体列表stackPopupPanels出栈UI窗体
                CloseReversePanel();
                break;
            case UIPanelShowMode.HideOther:
                //关闭指定窗体并显示其他窗体
                CloseHideOtherPanel(panelName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 显示遮罩
    /// </summary>
    /// <param name="needShowPanelGo">要显示的窗体对象</param>
    /// <param name="lucencyType"></param>
    public void ShowPopupMask(GameObject needShowPanelGo, UIPanelLucencyType lucencyType = UIPanelLucencyType.Lucency)
    {
        //显示遮罩,设置遮罩不同透明度
        switch (lucencyType)
        {
            case UIPanelLucencyType.Lucency:
                //完成透明,不能穿透
                transPopupMask.gameObject.SetActive(true);
                transPopupMask.GetComponent<Image>().color = Color.clear;
                break;
            case UIPanelLucencyType.Translucene:
                //半透明,不能穿透
                transPopupMask.gameObject.SetActive(true);
                transPopupMask.GetComponent<Image>().color = GlobalsDefine.TRANSLUCENE_COLOR;
                break;
            case UIPanelLucencyType.ImPenetrable:
                //低透明度,不能穿透
                transPopupMask.gameObject.SetActive(true);
                transPopupMask.GetComponent<Image>().color = GlobalsDefine.IMPENETRABLE_COLOR;
                break;
            case UIPanelLucencyType.Pentrate:
                //可以穿透
                if (transPopupMask.gameObject.activeInHierarchy)
                {
                    transPopupMask.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
        //遮罩最上层显示
        transPopupMask.SetAsLastSibling();
        //要显示的窗体最上层显示
        needShowPanelGo.transform.SetAsLastSibling();
        //设置UICamera层深确保UICamera为最前显示
        uiCamera.depth += 100;
    }

    /// <summary>
    /// 隐藏遮罩
    /// </summary>
    public void HidePopupMask()
    {
        //隐藏遮罩
        if (transPopupMask.gameObject.activeInHierarchy)
        {
            transPopupMask.gameObject.SetActive(false);
        }
        //重置UICamera层深
        uiCamera.depth = originalUICameraDepth;
    }

    /// <summary>
    /// 根据UI窗体预设名称,加载窗体到所有UI窗体列表allPanelDic中
    /// </summary>
    /// <param name="panelName">要显示的UI窗体名称</param>
    /// <returns>窗体预设对应的脚本</returns>
    UIBasePanel LoadPanelToAllPanelDic(string panelName)
    {
        UIBasePanel resultPanel;
        allPanelDic.TryGetValue(panelName, out resultPanel);
        //列表中有直接返回列表中的窗体预设对应的脚本,没有则加载窗体到所有UI窗体列表allPanelDic中
        if (resultPanel == null)
        {
            //根据UI窗体预设名称加载窗体预设
            string panelPath;
            panelPathDic.TryGetValue(panelName, out panelPath);
            if (!string.IsNullOrEmpty(panelPath))
            {
                GameObject panelGo = AssetLoader.Instance.Clone("Launch", panelPath);
                if (panelGo)
                {
                    resultPanel = panelGo.GetComponent<UIBasePanel>();
                    if (resultPanel == null)
                    {
                        GameLogger.LogError("窗体预设上没有挂载对应的脚本:" + panelName);
                        return null;
                    }
                    //根据窗体位置类型将窗体挂载在对应的挂点上
                    switch (resultPanel.CurUIInfo.panelType)
                    {
                        //全屏窗体
                        case UIPanelType.Panel:
                            resultPanel.transform.SetParent(transNormal, false);
                            break;
                        //固定窗体
                        case UIPanelType.Fixed:
                            resultPanel.transform.SetParent(transFixed, false);
                            break;
                        //弹出窗体
                        case UIPanelType.Popup:
                            resultPanel.transform.SetParent(transPopup, false);
                            break;
                        default:
                            break;
                    }
                    //默认隐藏
                    panelGo.SetActive(false);
                    //更新所有UI窗体列表allPanelDic
                    allPanelDic.Add(panelName, resultPanel);
                    return resultPanel;
                }
                else
                {
                    GameLogger.LogError(string.Format(@"在路径: {0} 中找不到对应预设,请确认!!!", panelPath));
                    return null;
                }
            }
            else
            {
                GameLogger.LogError(string.Format(@"{0}预设路径配置有误,请确认!!!", panelName));
                return null;
            }
        }
        return resultPanel;
    }

    /// <summary>
    /// 全屏窗体显示:当前显示的UI窗体列表curShowUIPanelDic中"加入"UI窗体
    /// </summary>
    /// <param name="panelName">要显示的UI窗体名称</param>
    void ShowNormalPanel(string panelName)
    {
        //当前显示的UI窗体列表中没有,则加入
        UIBasePanel tempCurPanel;
        curShowUIPanelDic.TryGetValue(panelName, out tempCurPanel);
        if (tempCurPanel != null)
            return;
        tempCurPanel = allPanelDic[panelName];
        curShowUIPanelDic.Add(panelName, tempCurPanel);
        //显示窗体
        tempCurPanel.Display();
    }

    /// <summary>
    /// "反向切换"PopupPanel的UI窗体入栈
    /// </summary>
    /// <param name="panelName">要显示的UI窗体名称</param>
    void ShowReversePanel(string panelName)
    {
        //冻结栈顶弹窗
        if (stackPopupPanels.Count > 0)
        {
            UIBasePanel topPanel = stackPopupPanels.Peek();
            topPanel.Freeze();
        }

        //"反向切换"UI窗体列表入栈
        UIBasePanel pushPanel = allPanelDic[panelName];
        stackPopupPanels.Push(pushPanel);
        pushPanel.Display();
    }

    /// <summary>
    /// 显示指定窗体并隐藏其他窗体
    /// </summary>
    /// <param name="panelName">要显示的UI窗体名称</param>
    void ShowHideOtherPanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        //当前显示的UI窗体列表中有记录,则直接返回
        UIBasePanel showPanel;
        curShowUIPanelDic.TryGetValue(panelName, out showPanel);
        if (showPanel != null)
            return;

        //隐藏当前显示的UI窗体列表中所有窗体
        foreach (UIBasePanel curShowPanel in curShowUIPanelDic.Values)
        {
            curShowPanel.Hiding();
        }
        //隐藏"反向切换"UI窗体列表中所有窗体
        foreach (UIBasePanel stackPanel in stackPopupPanels)
        {
            stackPanel.Hiding();
        }
        //把要显示的指定窗体加入到当前显示的UI窗体列表中
        UIBasePanel panel;
        allPanelDic.TryGetValue(panelName, out panel);
        if (panel != null)
        {
            curShowUIPanelDic.Add(panelName, panel);
            //显示窗体
            panel.Display();
        }
    }

    /// <summary>
    /// 关闭普通UI窗体
    /// </summary>
    /// <param name="panelName">要关闭的UI窗体名称</param>
    void CloseNormalPanel(string panelName)
    {
        UIBasePanel closePanel;
        //当前显示的UI窗体列表中没有,则直接返回
        curShowUIPanelDic.TryGetValue(panelName, out closePanel);
        if (closePanel == null)
            return;
        //隐藏窗体,从当前显示的UI窗体列表中移除
        closePanel.Hiding();
        curShowUIPanelDic.Remove(panelName);
    }

    /// <summary>
    /// 关闭反向切换窗体(Popup):出栈不需要UI窗体名称参数
    /// </summary>
    void CloseReversePanel()
    {
        if (stackPopupPanels.Count >= 2)
        {
            //出栈
            UIBasePanel popPanel = stackPopupPanels.Pop();
            //隐藏弹窗
            popPanel.Hiding();
            //重新显示新的栈顶弹窗
            UIBasePanel newTopPanel = stackPopupPanels.Peek();
            newTopPanel.ReDisplay();
        }
        //"反向切换"UI窗体列表只有当前要关闭的弹窗
        else if (stackPopupPanels.Count == 1)
        {
            //出栈
            UIBasePanel popPanel = stackPopupPanels.Pop();
            //隐藏弹窗
            popPanel.Hiding();
        }
    }

    /// <summary>
    /// 关闭指定窗体并显示其他窗体
    /// </summary>
    /// <param name="panelName">要关闭的UI窗体名称</param>
    void CloseHideOtherPanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        //当前显示的UI窗体列表中没有记录,则直接返回
        UIBasePanel closePanel;
        curShowUIPanelDic.TryGetValue(panelName, out closePanel);
        if (closePanel == null)
            return;

        //隐藏要关闭的UI窗体
        closePanel.Hiding();
        //把要关闭的指定窗体从当前显示的UI窗体列表中移除
        curShowUIPanelDic.Remove(panelName);

        //重新显示当前显示的UI窗体列表中所有窗体
        foreach (UIBasePanel curShowPanel in curShowUIPanelDic.Values)
        {
            curShowPanel.ReDisplay();
        }
        //重新显示"反向切换"UI窗体列表中所有窗体
        foreach (UIBasePanel stackPanel in stackPopupPanels)
        {
            stackPanel.ReDisplay();
        }
    }

    /// <summary>
    /// 初始化UI窗体配置文件
    /// </summary>
    void InitUIPanelConfig()
    {
        TextAsset textAsset = AssetLoader.Instance.CreateAsset<TextAsset>("Launch", GlobalsDefine.UI_PANTL_CONFIG_PATH, I18N.Instance.gameObject);
        JsonWrapper<UICfgItem> jsonObj = JsonMapper.ToObject<JsonWrapper<UICfgItem>>(textAsset.text);
        for (int i = 0; i < jsonObj.JsonConfig.Length; i++)
        {
            panelPathDic.Add(jsonObj.JsonConfig[i].panelName, jsonObj.JsonConfig[i].path);
        }
    }















    #region 测试方法
    /// <summary>
    /// 所有UI窗体列表的数量  
    /// </summary>
    /// <returns></returns>
    public int ShowAllPanelDicCount()
    {
        if (allPanelDic != null)
            return allPanelDic.Count;
        else
            return 0;
    }

    /// <summary>
    /// 当前显示的UI窗体列表的数量
    /// </summary>
    /// <returns></returns>
    public int ShowCurShowUIPanelDic()
    {
        if (curShowUIPanelDic != null)
            return curShowUIPanelDic.Count;
        else
            return 0;
    }

    /// <summary>
    /// "反向切换"UI窗体列表的数量
    /// </summary>
    /// <returns></returns>
    public int ShowStackPopupPanels()
    {
        if (stackPopupPanels != null)
            return stackPopupPanels.Count;
        else
            return 0;
    }
    #endregion










    /// <summary>
    /// 给控件添加自定义事件(服务于像摇杆,背包系统中的装备的拖拽操作等)
    /// </summary>
    /// <param name="component">控件</param>
    /// <param name="type">自定义事件类型</param>
    /// <param name="callBack">事件回调</param>
    public static void AddCustomEventListener(UIBehaviour component, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //从控件上获取/给控件添加自定义事件组件
        EventTrigger trigger = component.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = component.gameObject.AddComponent<EventTrigger>();
        //定义自定义事件的类型
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        //给自定义事件添加监听
        entry.callback.AddListener(callBack);
        //捆绑自定义事件给组件
        trigger.triggers.Add(entry);
    }
}
