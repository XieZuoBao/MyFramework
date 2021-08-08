/*
 * 
 *      Title:  基础框架
 * 
 *      Description: 
 *              单例模式基类
 *              减少单例模式重复代码的书写
 *  
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类(非继承Mono)
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSingleton<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}