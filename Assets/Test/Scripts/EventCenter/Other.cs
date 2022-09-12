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

public class Other : MonoBehaviour
{
    private void Start()
    {
        EventCenter.Instance.AddEventListener("MonsterDead", OtherWithMonsterDeadDo);
        EventCenter.Instance.AddEventListener<float>("MonsterDead1", OtherWithMonsterDeadDo);
        EventCenter.Instance.AddEventListener<float, string>("MonsterDead2", OtherWithMonsterDeadDo);
    }
    public void OtherWithMonsterDeadDo()
    {
        GameLogger.LogGreen("其他 逻辑-----无参");
    }
    public void OtherWithMonsterDeadDo(float info)
    {
        GameLogger.LogGreen("其他 逻辑-----一个参数");
    }

    public void OtherWithMonsterDeadDo(float info, string info2)
    {
        GameLogger.LogGreen("其他 逻辑-----两个参数");
    }


    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("MonsterDead", OtherWithMonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float>("MonsterDead1", OtherWithMonsterDeadDo);
        EventCenter.Instance.RemoveEventListener<float, string>("MonsterDead2", OtherWithMonsterDeadDo);
    }
}