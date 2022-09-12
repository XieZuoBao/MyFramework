using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类:子类中需要重写CurUIInfo的属性
/// </summary>
public class UIBasePanel : MonoBehaviour
{
    /// <summary>
    /// 当前UI窗体信息
    /// </summary>
    private UIInfo curUIInfo = new UIInfo();
    /// <summary>
    /// 当前UI窗体信息
    /// </summary>
    public UIInfo CurUIInfo
    {
        get => curUIInfo;
        set => curUIInfo = value;
    }

    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void Show()
    {
        this.gameObject.SetActive(true);
        //显示弹窗遮罩
        if (curUIInfo.panelType == UIPanelType.Popup)
        {
            UIMgr.Instance.ShowPopupMask(gameObject);
        }
    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
        //隐藏弹窗遮罩
        if (curUIInfo.panelType == UIPanelType.Popup)
        {
            UIMgr.Instance.HidePopupMask();
        }
    }
}