using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btnLeft;
    public Button btnRight;
    private void Start()
    {
        //SingletonTest.Instance.Test();
        btnLeft.onClick.AddListener(() =>
        {
            GameLogger.Log("left");
        });
        btnRight.onClick.AddListener(() =>
        {
            GameLogger.Log("right");
        });
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    PoolMgr.Instance.GetGameObjectByPool("Test/Prefabs/Cube", (obj) =>
        //    {
        //        obj.transform.localScale = Vector3.one * 2;
        //    });

        //if (Input.GetMouseButtonDown(1))
        //    PoolMgr.Instance.GetGameObjectByPool("Test/Prefabs/Sphere", (obj) =>
        //    {
        //        obj.transform.localScale = Vector3.one * 2;
        //    });
    }
}