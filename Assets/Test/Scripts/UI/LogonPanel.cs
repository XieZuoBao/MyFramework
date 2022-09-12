using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogonPanel : UIBasePanel
{
    private Button btnLogon;

    void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Panel;
        CurUIInfo.showMode = UIPanelShowMode.HideOther;
        CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
        btnLogon = transform.GetChildByName("BtnLogon").GetComponent<Button>();

    }

    private void Start()
    {
        btnLogon.onClick.AddListener(OnBtnLogonClick);
    }
    void OnBtnLogonClick()
    {
        UIMgr.Instance.ShowPanel("SelectHeroPanel");
    }
}