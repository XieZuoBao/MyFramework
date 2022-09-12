using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPopPanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenBPopup;
    //private Button btnOpenC;
    private Button btnOpenDPopup;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenBPopup = transform.GetChildByName("BtnOpenBPopup").GetComponent<Button>();
        //btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
        btnOpenDPopup = transform.GetChildByName("BtnOpenDPopup").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenBPopup.onClick.AddListener(OnBtnOpenBPopupClick);
        //btnOpenC.onClick.AddListener(OnBtnOpenCClick);
        btnOpenDPopup.onClick.AddListener(OnBtnOpenDPopup);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<CPopPanel>();
    }

    void OnBtnOpenBPopupClick()
    {
        UIMgr.Instance.ShowPanel<BPopPanel>(UIPanelType.Popup);
    }

    void OnBtnOpenCClick()
    {
        //UIMgr.Instance.ShowPanel("CPanel");
    }

    void OnBtnOpenDPopup()
    {
        UIMgr.Instance.ShowPanel<DPopPanel>(UIPanelType.Popup);
    }
}
