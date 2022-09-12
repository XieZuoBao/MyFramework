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


public class MonoTest : MonoBehaviour
{
    private void Start()
    {
        UpdateTest ut = new UpdateTest();
        MonoMgr.Instance.AddUpdateListener(ut.Update);
        //IEnumerator routine = Test();
        //StartCoroutine(routine);
        //StopCoroutine(routine);
        //StartCoroutine("Test");
        //StopCoroutine("Test");
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("aaaaaaa");
    }
}