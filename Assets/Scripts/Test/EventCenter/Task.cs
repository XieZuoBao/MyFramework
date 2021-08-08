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

public class Task : MonoBehaviour
{
    private void Start()
    {
        EventCenter.Instance.AddEventListener("MonsterDead", TaskWithMonsterDeadDo);
        EventCenter.Instance.AddEventListener<float>("MonsterDead1", TaskWithMonsterDeadDo);
        EventCenter.Instance.AddEventListener<float, string>("MonsterDead2", TaskWithMonsterDeadDo);
    }

    public void TaskWithMonsterDeadDo()
    {
        Debug.Log("刷新任务记录-----无参");
    }

    public void TaskWithMonsterDeadDo(float info)
    {
        Debug.Log("刷新任务记录-----一个参数");
    }

    public void TaskWithMonsterDeadDo(float info, string info2)
    {
        Debug.Log("刷新任务记录-----两个参数");
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("MonsterDead", TaskWithMonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float>("MonsterDead1", TaskWithMonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float, string>("MonsterDead2", TaskWithMonsterDeadDo);
    }
}