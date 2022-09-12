using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI窗体信息
/// </summary>
public class UIInfo
{
    /// <summary>
    /// 是否清空"栈集合":反向切换"UI窗体列表中的数据
    /// </summary>
    public bool IsClearStack = false;
    /// <summary>
    /// UI窗体位置类型
    /// </summary>
    public UIPanelType panelType = UIPanelType.Panel;
    /// <summary>
    /// UI窗体显示类型
    /// </summary>
    public UIPanelShowMode showMode = UIPanelShowMode.Normal;
    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public UIPanelLucencyType lucencyType = UIPanelLucencyType.Lucency;
}