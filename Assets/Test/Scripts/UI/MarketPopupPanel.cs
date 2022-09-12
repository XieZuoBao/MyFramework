using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketPopupPanel : UIBasePanel
{
    public Button btnCloth;
    public Button btnTrousers;
    public Button btnShoes;
    public Button btnWeapon;
    public Button btnRing;
    public Button btnClose;
    void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Popup;
        CurUIInfo.showMode = UIPanelShowMode.ReverseChange;
        CurUIInfo.lucencyType = UIPanelLucencyType.ImPenetrable;

        btnCloth = transform.GetChildByName("BtnCloth").GetComponent<Button>();
        btnTrousers = transform.GetChildByName("BtnTrousers").GetComponent<Button>();
        btnShoes = transform.GetChildByName("BtnShoes").GetComponent<Button>();
        btnWeapon = transform.GetChildByName("BtnWeapon").GetComponent<Button>();
        btnRing = transform.GetChildByName("BtnRing").GetComponent<Button>();
        btnClose = transform.GetChildByName("Btn_Return").GetComponent<Button>();
    }

    private void Start()
    {
        btnCloth.onClick.AddListener(OnBtnClothClick);
        btnTrousers.onClick.AddListener(OnBtnTrousersClick);
        btnShoes.onClick.AddListener(OnBtnShoesClick);
        btnWeapon.onClick.AddListener(OnBtnWeaponClick);
        btnRing.onClick.AddListener(OnBtnRingClick);
        btnClose.onClick.AddListener(OnBtnCloseClick);
    }

    void OnBtnClothClick()
    {
        UIMgr.Instance.ShowPanel("GoodsInfoPopupPanel");
    }

    void OnBtnTrousersClick()
    {

    }

    void OnBtnShoesClick()
    {

    }

    void OnBtnWeaponClick()
    {

    }

    void OnBtnRingClick()
    {

    }


    void OnBtnCloseClick()
    {
        UIMgr.Instance.HidePanel("MarketPopupPanel");
    }
}