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

public class Player : MonoBehaviour
{
    private void Start()
    {
        EventCenter.Instance.AddEventListener("MonsterDead", MonsterDeadDo);
        EventCenter.Instance.AddEventListener<float>("MonsterDead1", MonsterDeadDo);
        EventCenter.Instance.AddEventListener<float, string>("MonsterDead2", MonsterDeadDo);
    }
    public void MonsterDeadDo()
    {
        Debug.Log("玩家得奖励-----无参");
    }

    public void MonsterDeadDo(float moveSpeed)
    {
        Debug.Log("玩家得奖励-----一个参数");
    }


    public void MonsterDeadDo(float moveSpeed, string monsterNama)
    {
        Debug.Log("玩家得奖励-----两个参数");
    }


    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("MonsterDead", MonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float>("MonsterDead1", MonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float, string>("MonsterDead2", MonsterDeadDo);
    }
}