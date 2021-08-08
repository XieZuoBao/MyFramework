/*
 * 
 *      Title:  基础框架
 *         
 *      Description: 
 *              UI管理模块
 *              统一管理所有的UI面板的显示,隐藏等
 *                        
 ***/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI分层
/// </summary>
public enum E_UI_LAYER
{
    BOTTOM,
    MIDDLE,
    TOP,
    SYSTEM,
}

public class UIMgr : BaseSingleton<UIMgr>
{
    private string uiCanvasPath = GlobalsDefine.UI_CANVAS_PREFAB_PATH;          //UICanvas预制路径
    private string bottomNodeName = GlobalsDefine.UI_BOTTOM_NODE_NAME;          //底层ui节点
    private string middleNodeName = GlobalsDefine.UI_MIDDLE_NODE_NAME;          //中间层ui节点
    private string topNodeName = GlobalsDefine.UI_TOP_NODE_NAME;                //顶层ui节点
    private string systemNodeName = GlobalsDefine.UI_SYSTEM_NODE_NAME;          //系统层(最上层)ui节点
    private Transform bottomTrans;
    private Transform middleTrans;
    private Transform topTrans;
    private Transform systemTrans;
    public RectTransform canvasTrans;                                       //UI根节点-----背包时,外界要用到此根节点
    public Dictionary<string, UIBase> uiPanelDict = new Dictionary<string, UIBase>();

    public UIMgr()
    {
        //动态加载UICanvas到场景中
        GameObject canvasGo = ResMgr.Instance.LoadResource<GameObject>(uiCanvasPath);
        canvasTrans = canvasGo.transform as RectTransform;
        bottomTrans = canvasTrans.Find(bottomNodeName);
        middleTrans = canvasTrans.Find(middleNodeName);
        topTrans = canvasTrans.Find(topNodeName);
        systemTrans = canvasTrans.Find(systemNodeName);
        //过场景时不移除 
        GameObject.DontDestroyOnLoad(canvasGo);
    }

    /// <summary>
    /// 获取UI面板的父节点
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetUIPanelParent(E_UI_LAYER layer)
    {
        switch (layer)
        {
            case E_UI_LAYER.BOTTOM:
                return this.bottomTrans;
            case E_UI_LAYER.MIDDLE:
                return this.middleTrans;
            case E_UI_LAYER.TOP:
                return this.topTrans;
            case E_UI_LAYER.SYSTEM:
                return this.systemTrans;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="panelPath">面板路径</param>
    /// <param name="layer">面板显示层级</param>
    /// <param name="callBack">面板创建成功后要处理的逻辑</param>
    public void ShowPanel<T>(string panelPath, E_UI_LAYER layer = E_UI_LAYER.MIDDLE, UnityAction<T> callBack = null) where T : UIBase
    {
        //避免面板重复加载
        if (uiPanelDict.ContainsKey(panelPath))
        {
            uiPanelDict[panelPath].ShowPanel();
            //处理面板创建完成后的逻辑
            if (callBack != null)
                callBack.Invoke(uiPanelDict[panelPath] as T);
            return;
        }
        ResMgr.Instance.LoadResourceAsync<GameObject>(panelPath, (obj) =>
        {
            //异步加载完面板后将面板显示在指定位置
            Transform parent = bottomTrans;
            switch (layer)
            {
                case E_UI_LAYER.MIDDLE:
                    parent = middleTrans;
                    break;
                case E_UI_LAYER.TOP:
                    parent = topTrans;
                    break;
                case E_UI_LAYER.SYSTEM:
                    parent = systemTrans;
                    break;
            }
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            (obj.transform as RectTransform).offsetMax = Vector3.zero;
            (obj.transform as RectTransform).offsetMin = Vector3.zero;

            //得到预设身上的面板脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建完成后的逻辑
            if (callBack != null)
                callBack.Invoke(panel);
            panel.ShowPanel();
            //把面板存起来
            uiPanelDict.Add(panelPath, panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (uiPanelDict.ContainsKey(panelName))
        {
            uiPanelDict[panelName].HidePanel();
            GameObject.Destroy(uiPanelDict[panelName].gameObject);
            uiPanelDict.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到一个面板(用于背包系统装备格子内容的更新)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panelName) where T : UIBase
    {
        if (uiPanelDict.ContainsKey(panelName))
            return uiPanelDict[panelName] as T;
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件(服务于像背包系统中的装备的拖拽操作等)
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