using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APanel : UIBasePanel
{
    private Button btnClose;
    private void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Panel;
        CurUIInfo.showMode = UIPanelShowMode.HideOther;
        CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel("APanel");
    }
}