using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPointPanel : MonoBehaviour
{
    public GameObject pageA;
    public GameObject pageB;
    public Toggle toggleA;
    public GameObject toggleARedGo;
    public GameObject toggleBRedGo;
    public Text textA;
    public Text textB;

    public Button btnA1;
    public GameObject btnA1RedGo;
    public Button btnA2;
    public GameObject btnA2RedGo;
    public Button btnB1;
    public GameObject btnB1RedGo;
    public Button btnB2;
    public GameObject btnB2RedGo;

    private void Start()
    {
        toggleA.onValueChanged.AddListener(OnToggleAValueChanged);
        btnA1.onClick.AddListener(OnBtnA1Click);
        btnA2.onClick.AddListener(OnBtnA2Click);
        btnB1.onClick.AddListener(OnBtnB1Click);
        btnB2.onClick.AddListener(OnBtnB2Click);

        //红点更新回调
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelA, UpdateModelA);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelA_Sub_1, UpdateModelA_1);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelA_Sub_2, UpdateModelA_2);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelB, UpdateModelB);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelB_Sub_1, UpdateModelB_1);
        RedPointTestData.Instance.RedTree.SetCallback(RedPointTestData.Instance.ModelB_Sub_2, UpdateModelB_2);

        UpdateModelA(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelA));
        UpdateModelA_1(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelA_Sub_1));
        UpdateModelA_2(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelA_Sub_2));
        UpdateModelB(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelB));
        UpdateModelB_1(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelB_Sub_1));
        UpdateModelB_2(RedPointTestData.Instance.RedTree.GetRedPointCount(RedPointTestData.Instance.ModelB_Sub_2));
    }

    void OnToggleAValueChanged(bool isOn)
    {
        pageA.SetActive(isOn);
        pageB.SetActive(!isOn);
    }

    void OnBtnA1Click()
    {
        RedPointTestData.Instance.RedTree.ChangeRedPointCount(RedPointTestData.Instance.ModelA_Sub_1, -1);
    }

    void OnBtnA2Click()
    {
        RedPointTestData.Instance.RedTree.ChangeRedPointCount(RedPointTestData.Instance.ModelA_Sub_2, -1);
    }

    void OnBtnB1Click()
    {
        RedPointTestData.Instance.RedTree.ChangeRedPointCount(RedPointTestData.Instance.ModelB_Sub_1, -1);
    }

    void OnBtnB2Click()
    {
        RedPointTestData.Instance.RedTree.ChangeRedPointCount(RedPointTestData.Instance.ModelB_Sub_2, -1);
    }

    private void UpdateModelA(int redCount)
    {
        toggleARedGo.SetActive(redCount > 0);
        ShowText(textA, redCount);
    }
    private void UpdateModelA_1(int redCount)
    {
        btnA1RedGo.SetActive(redCount > 0);
    }
    private void UpdateModelA_2(int redCount)
    {
        btnA2RedGo.SetActive(redCount > 0);
    }
    private void UpdateModelB(int redCount)
    {
        toggleBRedGo.SetActive(redCount > 0);
        ShowText(textB, redCount);
    }
    private void UpdateModelB_1(int redCount)
    {
        btnB1RedGo.SetActive(redCount > 0);
    }
    private void UpdateModelB_2(int redCount)
    {
        btnB2RedGo.SetActive(redCount > 0);
    }
    private void ShowText(Text txt, int count)
    {
        txt.text = count.ToString();
    }
}