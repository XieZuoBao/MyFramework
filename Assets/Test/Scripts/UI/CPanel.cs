using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPanel : UIBasePanel
{
    private Button btnClose;
    private Button btnOpenCPopup;
    private Button btnOpenA;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        btnOpenCPopup = transform.GetChildByName("BtnOpenCPopup").GetComponent<Button>();
        btnOpenA = transform.GetChildByName("BtnOpenA").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        btnOpenCPopup.onClick.AddListener(OnBtnOpenCPopupClick);
        btnOpenA.onClick.AddListener(OnBtnOpenAClick);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<CPanel>();
    }

    void OnBtnOpenCPopupClick()
    {
        UIMgr.Instance.ShowPanel<CPopPanel>(UIPanelType.Popup);
    }

    void OnBtnOpenAClick()
    {
        UIMgr.Instance.ShowPanel<APanel>(UIPanelType.Panel);
    }
}
