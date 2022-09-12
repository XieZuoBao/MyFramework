using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform扩展类
/// </summary>
public static class TransformExtensions
{
    /// <summary>
    /// 根据子孙名称childName查找并返回子孙
    /// </summary>
    /// <param name="target">操作对象</param>
    /// <param name="childName">子孙名称</param>
    /// <returns>查找到子孙的Transform</returns>
    public static Transform GetChildByName(this Transform target, string childName)
    {
        //先在target的子物体中查找
        Transform transChild = target.Find(childName);
        if (transChild != null)
            return transChild;
        for (int i = 0; i < target.childCount; i++)
        {
            //子物体中找不到,就到"孙"物体中去递归查找
            transChild = GetChildByName(target.GetChild(i), childName);
            if (transChild != null)
                return transChild;
        }
        return transChild;
    }
}