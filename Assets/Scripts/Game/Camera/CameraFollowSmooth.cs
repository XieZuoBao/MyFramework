using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机跟随脚本
/// </summary>
public class CameraFollowSmooth : MonoBehaviour
{
    //跟随对象
    private Transform targetPlayer;
    //相机与玩家之间保持的固定距离
    public Vector3 offset = Vector3.one;
    //相机跟随速度
    private Vector3 velocity = Vector3.one;
    //相机同步滞后时间
    public float smoothTime = 0.25f;
    private void Start()
    {
        targetPlayer = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPlayer.position + offset, ref velocity, smoothTime);
    }
}