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

public class Monster : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    public string monsterName = "Wolf";
    public GameObject go;
    private void Start()
    {
        Invoke("Dead", 3f);
    }
    void Dead()
    {
        GameLogger.Log("Monster dead", go);
        //其他对象在怪物死亡后需要处理的逻辑
        /*
        //1.玩家得奖励
        GameObject.Find("Player").GetComponent<Player>().MonsterDeadDo();
        //2.任务记录
        GameObject.Find("Task").GetComponent<Task>().TaskWithMonsterDeadDo();
        //3.其他
        GameObject.Find("Other").GetComponent<Other>().OtherWithMonsterDeadDo();
        //新增逻辑
        */
        EventCenter.Instance.EventTrigger("MonsterDead");
        EventCenter.Instance.EventTrigger<float>("MonsterDead1", moveSpeed);
        EventCenter.Instance.EventTrigger<float, string>("MonsterDead2", moveSpeed, monsterName);

    }
}