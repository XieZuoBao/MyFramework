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

public class UITest : MonoBehaviour
{
    private void Start()
    {
        UIMgr.Instance.ShowPanel<LoginPanel>("Prefabs/UI/Login/LoginPanel", E_UI_LAYER.MIDDLE, ShowPanelOver);

    
    }



    private void ShowPanelOver(LoginPanel panel)
    {
        panel.InitInfo();
        //Invoke("Hide", 3);
    }

    private void Hide()
    {
        UIMgr.Instance.HidePanel("Prefabs/UI/Login/LoginPanel");
    }
}