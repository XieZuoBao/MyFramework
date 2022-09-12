using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : UIBasePanel
{
    public Dropdown dp;
    public Button btnRedPoint;
    public GameObject btnRedPointGo;
    public Text txtNum;

    public Button btnLeftCommon;
    public Button btnRightCommon;
    public Button alphaBtnLeft;
    public Button alphaBtnRight;
    public Button btnLeft;
    public Button btnRight;

    private void Awake()
    {
        //窗体信息
        CurUIInfo.panelType = UIPanelType.Panel;
        //CurUIInfo.showMode = UIPanelShowMode.HideOther;
        //CurUIInfo.lucencyType = UIPanelLucencyType.Lucency;
    }

    private void Start()
    {
        //多语言测试
        dp.onValueChanged.AddListener((value) =>
        {
            LanguageMgr.Instance.ChangeLanguageType(value);
        });
        //红点系统测试
        btnRedPoint.onClick.AddListener(OnBtnRedPointClick);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.Root, UpdateRedPoint);
        UpdateRedPoint(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.Root));

        btnLeftCommon.onClick.AddListener(() =>
        {
            GameLogger.LogCyan("left");
        });
        btnRightCommon.onClick.AddListener(() =>
        {
            GameLogger.LogCyan("right");
        });
        alphaBtnLeft.onClick.AddListener(() =>
        {
            GameLogger.LogYellow("alphaLeft");
        });
        alphaBtnRight.onClick.AddListener(() =>
        {
            GameLogger.LogYellow("alphaRight");
        });
        btnLeft.onClick.AddListener(() =>
        {
            GameLogger.LogGreen("left ok");
        });
        btnRight.onClick.AddListener(() =>
        {
            GameLogger.LogGreen("right ok");
        });
    }

    void OnBtnRedPointClick()
    {
        GameObject go = AssetLoader.Instance.Clone("Launch", "Assets/GAssets/Launch/Res/UI/Redpoint/Prefabs/RedPointPanel.prefab");
        //go.transform.SetParent(PanelMgr.Instance.canvasTrans, false);
    }

    void UpdateRedPoint(int redPointCount)
    {
        btnRedPointGo.SetActive(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.Root) > 0);
        txtNum.text = redPointCount.ToString();
    }

    public void InitInfo()
    {
        Debug.Log("初始化信息");
    }

    private void OnBtnLoginClick()
    {
        Debug.Log("Login");
    }

    private void OnBtnRegisterClick()
    {
        Debug.Log("Register");
    }

    private void OnBtnSetClick()
    {
        Debug.Log("Set");
    }


}