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

public class InputTest : MonoBehaviour
{
    private void Start()
    {
        InputMgr.Instance.StartOrEndCheck(true);
        EventCenter.Instance.AddEventListener<KeyCode>("按键按下", KeyDown);
        EventCenter.Instance.AddEventListener<KeyCode>("按键抬起", KeyUp);
    }

    private void KeyDown(KeyCode key)
    {
        //KeyCode keyCode = (KeyCode)key;
        switch (key)
        {
            case KeyCode.W:
                Debug.Log("前进");
                break;
            case KeyCode.A:
                Debug.Log("左转");
                break;
            case KeyCode.S:
                Debug.Log("后退");
                break;
            case KeyCode.D:
                Debug.Log("右转");
                break;
        }
    }

    private void KeyUp(KeyCode key)
    {
        //KeyCode keyCode = (KeyCode)key;
        switch (key)
        {
            case KeyCode.W:
                Debug.Log("停止前进");
                break;
            case KeyCode.A:
                Debug.Log("停止左转");
                break;
            case KeyCode.S:
                Debug.Log("停止后退");
                break;
            case KeyCode.D:
                Debug.Log("停止右转");
                break;
        }
    }
}