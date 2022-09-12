using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPopPanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenB;
    //private Button btnOpenC;
    //private Button btnOpenDPopup;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenB = transform.GetChildByName("BtnOpenB").GetComponent<Button>();
        //btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
        //btnOpenDPopup = transform.GetChildByName("BtnOpenAPopup").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenB.onClick.AddListener(OnBtnOpenBClick);
        //btnOpenC.onClick.AddListener(OnBtnOpenCClick);
        //btnOpenDPopup.onClick.AddListener(OnBtnOpenDPopup);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<DPopPanel>();
    }

    void OnBtnOpenBClick()
    {
        UIMgr.Instance.ShowPanel<BPanel>(UIPanelType.Panel);
    }

    void OnBtnOpenCClick()
    {
        //UIMgr.Instance.ShowPanel("CPanel");
    }

    void OnBtnOpenDPopup()
    {
        //UIMgr.Instance.ShowPanel("DPopPanel");
    }
}