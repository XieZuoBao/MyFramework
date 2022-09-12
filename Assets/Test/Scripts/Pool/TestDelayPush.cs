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

public class TestDelayPush : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Push", 1);
    }

    void Push()
    {
        PoolMgr.Instance.RecoverGameObjectToPools(this.gameObject.name, this.gameObject);
    }
}