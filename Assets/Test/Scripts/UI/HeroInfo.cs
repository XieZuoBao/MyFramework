using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfo : UIBasePanel
{
    private void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Fixed;
        CurUIInfo.showMode = UIPanelShowMode.Normal;
        CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
    }
}