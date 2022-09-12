using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroPanel : UIBasePanel
{
    public Button btnEnter;
    public Button btnReturn;

    void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Panel;
        CurUIInfo.showMode = UIPanelShowMode.HideOther;
        CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
        btnEnter = transform.GetChildByName("BtnOK").GetComponent<Button>();
        btnReturn = transform.GetChildByName("ImgReturn").GetComponent<Button>();
    }

    private void Start()
    {
        btnEnter.onClick.AddListener(OnBtnEnterClick);
        btnReturn.onClick.AddListener(OnBtnReturnClick);
    }

    void OnBtnEnterClick()
    {
        UIMgr.Instance.ShowPanel("MainPanel");
        UIMgr.Instance.ShowPanel("HeroInfo");
    }

    void OnBtnReturnClick()
    {
        UIMgr.Instance.HidePanel("SelectHeroPanel");
    }
}