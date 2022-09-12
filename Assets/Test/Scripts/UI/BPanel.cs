using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenC;
    private Button btnOpenCPopup;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
        btnOpenCPopup = transform.GetChildByName("BtnOpenCPopup").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenC.onClick.AddListener(OnBtnOpenCClick);
        btnOpenCPopup.onClick.AddListener(OnBtnOpenCPopupClick);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<BPanel>();
    }

    void OnBtnOpenCPopupClick()
    {
        UIMgr.Instance.ShowPanel<CPopPanel>(UIPanelType.Popup);
    }

    void OnBtnOpenCClick()
    {
        UIMgr.Instance.ShowPanel<CPanel>(UIPanelType.Panel);
    }
}
