/*
 * 
 *      Title:  背包系统
 * 
 *             
 *      Description: 
 *              管理背包的公共数据和公共方法
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackMgr : BaseSingleton<KnapsackMgr>
{
    //背包格子数量
    public List<Item> itemsList = new List<Item>();

    /// <summary>
    /// 测试方法,模拟背包数据的初始化
    /// </summary>
    public void InitItemsInfo()
    {
        for (int i = 0; i < 100000; i++)
        {
            Item item = new Item();
            item.Id = i;
            item.Count = i;

            itemsList.Add(item);
        }
    }
}