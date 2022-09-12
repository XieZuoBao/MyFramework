using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenB;
    private Button btnOpenC;
    private Button btnOpenAPopup;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenB = transform.GetChildByName("BtnOpenB").GetComponent<Button>();
        btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
        btnOpenAPopup = transform.GetChildByName("BtnOpenAPopup").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenB.onClick.AddListener(OnBtnOpenBClick);
        btnOpenC.onClick.AddListener(OnBtnOpenCClick);
        btnOpenAPopup.onClick.AddListener(OnBtnOpenAPopup);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<APanel>();
    }

    void OnBtnOpenBClick()
    {
        UIMgr.Instance.ShowPanel<BPanel>(UIPanelType.Panel);
    }

    void OnBtnOpenCClick()
    {
        UIMgr.Instance.ShowPanel<CPanel>(UIPanelType.Panel);
    }

    void OnBtnOpenAPopup()
    {
        UIMgr.Instance.ShowPanel<APopPanel>(UIPanelType.Popup);
    }
}