/*
 * 
 *      Title:��������ű�
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Awake()
    {
        //�ȸ���֮ǰ��ʼ��һЩģ��
        InitBeforeHotUpdate();
    }

    /// <summary>
    /// �ȸ���֮ǰ��ʼ��һЩģ��
    /// </summary>
    private void InitBeforeHotUpdate()
    {
        // ������Ϸ֡��
        Application.targetFrameRate = GlobalsDefine.GAME_FRAME_RATE;
        // �ֻ�����
        Screen.sleepTimeout = -1;
        // ��̨����
        Application.runInBackground = true;

        //��־
        GameLogger.Init();
        LogCat.Init();
    }
}
