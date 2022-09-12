using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓存池模块
/// </summary>
public class PoolMgr : BaseSingleton<PoolMgr>
{
    //new出来的缓存池容器根节点名称
    private string rootPoolGoName = GlobalsDefine.POOL_GO_NAME;
    //缓存池容器的根节点("Pool")
    private GameObject rootPoolGo;
    //缓存池容器<对象所属对象池名称(子池子),缓存对象集合>
    private Dictionary<string, PoolData> poolDict = new Dictionary<string, PoolData>();


    /// <summary>
    /// 从缓存池中取对象
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public void GetGameObjectByPool(string poolName, UnityAction<GameObject> callBack)
    {
        //GameObject go = null;
        //缓存池容器中已有对象所属的对象池
        if (poolDict.ContainsKey(poolName) && poolDict[poolName].poolGoList.Count > 0)
        {
            //go = poolDict[poolName].GetObj();
            callBack(poolDict[poolName].GetObj());
        }
        else//缓存池容器中还没有对象所属的对象池
        {
            //废弃的同步加载资源
            //go = GameObject.Instantiate(Resources.Load<GameObject>(poolName));
            //go.name = poolName;
            //用异步加载资源的方式替换
            //ResMgr.Instance.LoadResourceAsync<GameObject>(poolName, (o) =>
            //{
            //    o.name = poolName;
            //    callBack(o);
            //});
        }
    }

    /// <summary>
    /// 将不用的对象放回缓存池
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="obj"></param>
    public void RecoverGameObjectToPools(string poolName, GameObject obj)
    {
        if (rootPoolGo == null)
            rootPoolGo = new GameObject(rootPoolGoName);
        //缓存池容器中已有对象所属的对象池
        if (poolDict.ContainsKey(poolName))
        {
            poolDict[poolName].PushObj(obj);
        }
        else//缓存池容器中还没有对象所属的对象池
        {
            poolDict.Add(poolName, new PoolData(obj, rootPoolGo));
        }
    }

    /// <summary>
    /// 场景切换时清空缓存池
    /// </summary>
    public void Clear()
    {
        poolDict.Clear();
        rootPoolGo = null;
    }
}

/// <summary>
/// 对象池中一组容器(一个子池子)
/// </summary>
public class PoolData
{
    //子池子容器(要放回缓冲池对象的父节点)
    public GameObject parentGo;
    //子池子中对象集合
    public List<GameObject> poolGoList;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pushGo">放回子池子中的对象</param>
    /// <param name="rootGo">子池子容器的父物体</param>
    public PoolData(GameObject pushGo, GameObject rootGo)
    {
        //实例化放回池子中对象的父对象,父对象的名称同放回子池子中的对象名称
        parentGo = new GameObject(pushGo.name);
        //将子池子容器设置在根对象("Pool")下
        parentGo.transform.SetParent(rootGo.transform);
        //将要放回子池子中的对象添加到对应的子池子对象集合中
        poolGoList = new List<GameObject>() { };
        //将对象放回子池子中
        PushObj(pushGo);
    }

    /// <summary>
    /// 将对象放回子池子中
    /// </summary>
    /// <param name="pushGo"></param>
    public void PushObj(GameObject pushGo)
    {
        //隐藏对象
        pushGo.SetActive(false);
        //将对象添加到子池子中对象集合
        poolGoList.Add(pushGo);
        //将对象父节点设置为子池子容器
        pushGo.transform.SetParent(parentGo.transform);
    }

    /// <summary>
    /// 从子池子中取对象 
    /// </summary>
    public GameObject GetObj()
    {
        GameObject go = null;
        //取对象
        go = poolGoList[poolGoList.Count - 1];
        //子池子容器中移除取出的对象
        poolGoList.RemoveAt(poolGoList.Count - 1);
        //激活对象
        go.SetActive(true);
        //断开父对象
        go.transform.SetParent(null);
        return go;
    }
}