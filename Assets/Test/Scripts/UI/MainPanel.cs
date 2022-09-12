using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : UIBasePanel
{
    private Button btnMarket;
    private Button btnPackage;
    private Button btnTask;
    private Button btnInfo;
    private Button btnExit;

    private void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Panel;
        CurUIInfo.showMode = UIPanelShowMode.HideOther;
        CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
        btnMarket = transform.GetChildByName("Btn_Market").GetComponent<Button>();
        btnPackage = transform.GetChildByName("Btn_Package").GetComponent<Button>();
        btnTask = transform.GetChildByName("Btn_Task").GetComponent<Button>();
        btnInfo = transform.GetChildByName("Btn_Info").GetComponent<Button>();
        btnExit = transform.GetChildByName("Btn_Exit").GetComponent<Button>();
    }

    private void Start()
    {
        btnMarket.onClick.AddListener(OnBtnMarketClick);
        btnPackage.onClick.AddListener(OnBtnPackageClick);
        btnTask.onClick.AddListener(OnBtnTaskClick);
        btnInfo.onClick.AddListener(OnBtnInfoClick);
        btnExit.onClick.AddListener(OnBtnExitClick);
    }

    void OnBtnMarketClick()
    {
        UIMgr.Instance.ShowPanel("MarketPopupPanel");
    }

    void OnBtnPackageClick()
    {

    }

    void OnBtnTaskClick()
    {

    }

    void OnBtnInfoClick()
    {

    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel("MainPanel");
    }
}