/*
 * 
 *      Title:
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        //GetComponentAtChildren<Button>("BtnLogin").onClick.AddListener(OnBtnLoginClick);
        //GetComponentAtChildren<Button>("BtnRegister").onClick.AddListener(OnBtnRegisterClick);
        //GetComponentAtChildren<Button>("BtnSet").onClick.AddListener(OnBtnSetClick);
        UIMgr.AddCustomEventListener(GetComponentAtChildren<Button>("BtnLogin"), EventTriggerType.PointerEnter, (data) =>
        {
            Debug.Log("进入");
        });

        UIMgr.AddCustomEventListener(GetComponentAtChildren<Button>("BtnLogin"), EventTriggerType.PointerExit, (data) =>
        {
            Debug.Log("离开");
        });
    }


    protected override void OnClick(string btnName)
    {
        //base.OnClick(btnName);
        Debug.Log(btnName);
    }

    protected override void OnToggleValueChanged(string toggleName, bool value)
    {
        //base.OnToggleValueChanged(toggleName, value);
        Debug.Log(toggleName + "===" + value);
    }

    protected override void OnSliderValueChanged(string sliderName, float value)
    {
        //base.OnSliderValueChanged(sliderName, value);
        Debug.Log(sliderName + "---" + value);
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