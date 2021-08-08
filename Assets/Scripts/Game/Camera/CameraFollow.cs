using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机跟随脚本
/// </summary>
public class CameraFollow : MonoBehaviour
{
    //游戏人物的位置
    private Transform player;
    //游戏人物与相机的距离差
    private Vector3 offset;
    //相机的速度
    private float cameraSpeed = 3;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        //世界坐标转换为局部坐标
        Vector3 targetPos = player.position + player.TransformDirection(offset);
        //计算相机位置和目标位置的插值
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
        //相机看向人物
        transform.LookAt(player.position);
    }
}