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

public class ResTest : MonoBehaviour
{
    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = ResMgr.Instance.LoadResource<GameObject>("Test/Prefabs/Cube1");
            go.transform.localScale = Vector3.one * 2;
        }

        if (Input.GetMouseButtonDown(1))
        {
            ResMgr.Instance.LoadResourceAsync<GameObject>("Test/Prefabs/Sphere1", (obj) =>
            {
                //处理资源异步加载完成后的逻辑
                obj.transform.localScale = Vector3.one * 2;
            });
        }
        */
    }

    private void CallBack(GameObject go)
    {
        go.transform.localScale = Vector3.one * 2;
    }
}