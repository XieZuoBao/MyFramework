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
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        //显示弹窗遮罩
        if (curUIInfo.panelType == UIPanelType.Popup)
        {
            UIMgr.Instance.ShowPopupMask(gameObject, curUIInfo.lucencyType);
        }
    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hiding()
    {
        this.gameObject.SetActive(false);
        //隐藏弹窗遮罩
        if (curUIInfo.panelType == UIPanelType.Popup)
        {
            UIMgr.Instance.HidePopupMask();
        }
    }

    /// <summary>
    /// 重新显示状态
    /// </summary>
    public virtual void ReDisplay()
    {
        this.gameObject.SetActive(true);
        //设置弹窗遮罩
        if (curUIInfo.panelType == UIPanelType.Popup)
        {
            UIMgr.Instance.ShowPopupMask(gameObject, curUIInfo.lucencyType);
        }
    }

    /// <summary>
    /// 冻结状态(冻结窗口则表示当前窗体不是顶层显示,不需要设置其遮罩属性)
    /// </summary>
    public virtual void Freeze()
    {
        this.gameObject.SetActive(true);
    }



    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void ShowPanel()
    {

    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void HidePanel()
    {

    }
}