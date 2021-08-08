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
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{
    private Image imgFg;
    private Text txtProgress;
    private void Awake()
    {
        imgFg = transform.Find("Bg/Fg").GetComponent<Image>();
        txtProgress = transform.Find("Bg/txtProgress").GetComponent<Text>();
        //EventCenter.Instance.AddEventListener("Loading", updateProgress);
    }
    private void Start()
    {
        ScenesMgr.Instance.LoadSceneAsync("main2", null);
    }

    void updateProgress(object progress)
    {
        txtProgress.text = ((float)progress).ToString();
        imgFg.fillAmount = (float)progress;
    }

    private void OnDestroy()
    {
        //EventCenter.Instance.RemoveEventListener("Loading", updateProgress);
    }
}