using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitMap : MonoBehaviour
{
    private RectTransform rectTrans;
    private float timer = 0;
    private float UpdateTime = 0.1f;
    private float xRatio;
    private float yRatio;
    private Transform player;

    private float planeWidth;
    private float planeHeight;
    private MeshCollider mc;
    private RectTransform rectTransParent;

    private float litMapWidth;
    private float litMapHeight;
    Vector2 litMapPos = Vector2.zero;
    private Vector3 towards;//小地图朝向



    private void Awake()
    {
        rectTrans = transform.GetComponent<RectTransform>();

        mc = GameObject.Find("Plane").GetComponent<MeshCollider>();
        planeWidth = mc.bounds.size.x;
        planeHeight = mc.bounds.size.z;

        rectTransParent = transform.parent as RectTransform;
        litMapWidth = rectTransParent.sizeDelta.x;
        litMapHeight = rectTransParent.sizeDelta.y;

        player = GameObject.FindWithTag("Player").transform;
        UpdateLitMapPos();
    }

    private void Update()
    {
        timer++;
        if (timer > UpdateTime)
        {
            timer = 0;
            UpdateLitMapPos();
        }
    }

    /// <summary>
    /// 更新小地图位置和旋转信息
    /// </summary>
    void UpdateLitMapPos()
    {
        //更新位置信息
        xRatio = (player.localPosition.x + planeWidth / 2) / planeWidth;
        yRatio = (player.localPosition.z + planeHeight / 2) / planeHeight;
        litMapPos.x = litMapWidth * xRatio;
        litMapPos.y = litMapHeight * yRatio;
        rectTrans.anchoredPosition = litMapPos;
        //更新旋转角度
        towards.z = player.localEulerAngles.y;
        rectTrans.localEulerAngles = -towards;
    }
}