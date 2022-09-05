/*
 * 
 *      Title:  基础框架
 *             
 *      Description: 
 *              资源加载模块
 *              给外部提供同步,异步加载资源的方法
 *                      
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseSingleton<ResMgr>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadResource<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //预制类资源
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//文本,音效类资源
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public void LoadResourceAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(WaitLoadResourceAsync(path, callBack));
    }

    /// <summary>
    /// 协程--异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="callBack">加载完资源后的处理逻辑</param>
    /// <returns></returns>
    private IEnumerator WaitLoadResourceAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        //预制类资源
        if (request.asset is GameObject)
            callBack(GameObject.Instantiate(request.asset) as T);//将对象直接实例化出来
        else//文本,音效类资源
            callBack(request.asset as T);//处理资源异步加载完成后的逻辑
    }
}