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

public class KnapsackTest : MonoBehaviour
{
    private void Awake()
    {
        KnapsackMgr.Instance.InitItemsInfo();
        UIMgr.Instance.ShowPanel<KnapsackPanel>("Prefabs/UI/Knapsack/KnapsackPanel", E_UI_LAYER.MIDDLE);
    }
}