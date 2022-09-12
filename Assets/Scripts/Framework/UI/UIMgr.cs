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
    /// 当前显示的所有UI窗体列表
    /// </summary>
    private Stack<UIBasePanel> stackCurPanels;
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
        stackCurPanels = new Stack<UIBasePanel>();
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
    /// 显示UI窗体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelType"></param>
    public void ShowPanel<T>(UIPanelType panelType) where T : UIBasePanel
    {
        string panelName = typeof(T).ToString();
        UIBasePanel panel = LoadPanelToAllPanelDic(panelName, panelType);
        if (panel == null)
            return;

        //打开全屏窗体时隐藏其他窗体
        if (panelType == UIPanelType.Panel)
        {
            foreach (UIBasePanel stackPanel in stackCurPanels)
            {
                stackPanel.Hide();
            }
        }
        //把要显示的指定窗体加入到当前显示的UI窗体列表中
        UIBasePanel pushPanel = allPanelDic[panelName];
        stackCurPanels.Push(pushPanel);
        pushPanel.Show();
    }

    /// <summary>
    /// 关闭/隐藏窗体(返回上一个状态)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>() where T : UIBasePanel
    {
        string panelName = typeof(T).ToString();
        //所有UI窗体列表中有则处理后续,没有则返回
        UIBasePanel closePanel;
        allPanelDic.TryGetValue(panelName, out closePanel);
        if (closePanel == null)
            return;

        if (stackCurPanels.Count >= 2)
        {
            //出栈
            UIBasePanel popPanel = stackCurPanels.Pop();
            //隐藏弹窗
            popPanel.Hide();
            //恢复到上一个全屏窗体显示的状态
            List<UIBasePanel> resumePanel = new List<UIBasePanel>();
            IEnumerator<UIBasePanel> panelEnumerator = stackCurPanels.GetEnumerator();
            while (panelEnumerator.MoveNext())
            {
                resumePanel.Add(panelEnumerator.Current);
                if (panelEnumerator.Current.CurUIInfo.panelType == UIPanelType.Panel)
                    break;
            }
            if (resumePanel.Count == 0)
                return;
            //倒序显示需要重新显示的界面
            for (int i = resumePanel.Count - 1; i >= 0; i--)
            {
                resumePanel[i].Show();
            }
        }
    }

    /// <summary>
    /// 显示遮罩
    /// </summary>
    /// <param name="needShowPanelGo">要显示的窗体对象</param>
    /// <param name="lucencyType"></param>
    public void ShowPopupMask(GameObject needShowPanelGo)
    {
        //显示遮罩,设置遮罩不同透明度
        transPopupMask.gameObject.SetActive(true);
        transPopupMask.GetComponent<Image>().color = GlobalsDefine.LUCENCY_COLOR;
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
    /// <param name="panelType">窗体类型</param>
    /// <returns>窗体预设对应的脚本</returns>
    UIBasePanel LoadPanelToAllPanelDic(string panelName, UIPanelType panelType)
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
                    resultPanel.CurUIInfo.panelType = panelType;
                    //根据窗体位置类型将窗体挂载在对应的挂点上
                    switch (resultPanel.CurUIInfo.panelType)
                    {
                        //全屏窗体
                        case UIPanelType.Panel:
                            resultPanel.transform.SetParent(transNormal, false);
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