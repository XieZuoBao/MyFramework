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

public class UpdateTest
{
    public UpdateTest()
    {
        MonoMgr.Instance.StartCoroutine(Test());
    }

    public void Update()
    {
        Debug.Log("没有继承Mono的类执行Update更新");
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("---");
    }
}