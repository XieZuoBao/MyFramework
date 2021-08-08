/*
 * 
 *      Title:
 *             
 * 
 * 
 *      Description: 
 *              
 * 
 * 
 * 
 * 
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        SingletonTest.Instance.Test();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PoolMgr.Instance.GetGameObjectByPool("Test/Prefabs/Cube", (obj) =>
            {
                obj.transform.localScale = Vector3.one * 2;
            });

        if (Input.GetMouseButtonDown(1))
            PoolMgr.Instance.GetGameObjectByPool("Test/Prefabs/Sphere", (obj) =>
            {
                obj.transform.localScale = Vector3.one * 2;
            });
    }
}