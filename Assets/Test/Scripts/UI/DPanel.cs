using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPanel : UIBasePanel
{
    private Button btnClose;
    //private Button btnOpenB;
    //private Button btnOpenC;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
        //btnOpenB = transform.GetChildByName("BtnOpenB").GetComponent<Button>();
        //btnOpenC = transform.GetChildByName("BtnOpenC").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
        //btnOpenB.onClick.AddListener(OnBtnOpenBClick);
        //btnOpenC.onClick.AddListener(OnBtnOpenCClick);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<DPanel>();
    }

    void OnBtnOpenBClick()
    {
        //UIMgr.Instance.ShowPanel("BPanel");
    }

    void OnBtnOpenCClick()
    {
        //UIMgr.Instance.ShowPanel("CPanel");
    }
}
