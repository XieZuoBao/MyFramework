/*
 * 
 *      Title:  背包系统
 * 
 *             
 *      Description: 
 *              背包格子类,用于显示单个格子的显示内容
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnapsackItem : UIBase, IItemBase<Item>
{
    /// <summary>
    /// 初始化单个格子的信息
    /// </summary>
    public void InitInfo(Item info)
    {
        //商业项目在此处的逻辑是:先读取道具表,根据表中数据来更新信息,更新图标,更新名称
        //TODO...

        //测试代码------更新道具数量
        GetComponentAtChildren<Text>("TxtNum").text = info.Count.ToString();
    }
}