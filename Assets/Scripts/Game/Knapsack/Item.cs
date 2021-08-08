/*
 * 
 *      Title:  背包系统
 * 
 *             
 *      Description: 
 *           
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 数据来源类
/// </summary>
public class Item
{
    private int id;
    private int count;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int Count
    {
        get { return count; }
        set { count = value; }
    }
}