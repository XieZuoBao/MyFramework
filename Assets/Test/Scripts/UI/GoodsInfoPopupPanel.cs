using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsInfoPopupPanel : UIBasePanel
{
    public Button btnClose;
    public Button btnSure;
    private void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Popup;
        //CurUIInfo.showMode = UIPanelShowMode.ReverseChange;
        CurUIInfo.lucencyType = UIPanelLucencyType.ImPenetrable;

        btnClose = transform.GetChildByName("Btn_Return").GetComponent<Button>();
        btnSure = transform.GetChildByName("BtnSure").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnCloseClick);
        btnSure.onClick.AddListener(OnBtnSureClick);
    }

    void OnBtnCloseClick()
    {
        UIMgr.Instance.HidePanel("GoodsInfoPopupPanel");
    }

    void OnBtnSureClick()
    {
        UIMgr.Instance.ShowPanel("APanel");
    }
}