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

public class SingletonTest : MonoSingleton<SingletonTest>
{
    public void Test()
    {
        Debug.Log(SingletonTest.Instance.name);
    }
}