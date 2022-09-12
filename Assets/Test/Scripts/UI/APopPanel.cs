using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APopPanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenB;
    //private Button btnOpenC;
    private Button btnOpenBPopup;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenB = transform.GetChildByName("BtnOpenB").GetComponent<Button>();
        //btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
        btnOpenBPopup = transform.GetChildByName("BtnOpenBPopup").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenB.onClick.AddListener(OnBtnOpenBClick);
        //btnOpenC.onClick.AddListener(OnBtnOpenCClick);
        btnOpenBPopup.onClick.AddListener(OnBtnOpenBPopup);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<APopPanel>();
    }

    void OnBtnOpenBClick()
    {
        UIMgr.Instance.ShowPanel<BPanel>(UIPanelType.Panel);
    }

    void OnBtnOpenCClick()
    {
        //UIMgr.Instance.ShowPanel("CPanel");
    }

    void OnBtnOpenBPopup()
    {
        UIMgr.Instance.ShowPanel<BPopPanel>(UIPanelType.Popup);
    }
}
