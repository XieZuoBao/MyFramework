using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity工具类:提供程序用户一些常用功能
/// </summary>
public class UnityUtil
{
    /// <summary>
    /// 根据子孙名称childName查找并返回子孙
    /// </summary>
    /// <param name="target">操作对象</param>
    /// <param name="childName">子孙名称</param>
    /// <returns>查找到子孙的Transform</returns>
    public static Transform GetChildByName(Transform target, string childName)
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
    /// <summary>
    /// 根据子孙名称childName查找并返回子孙
    /// </summary>
    /// <param name="target">操作对象</param>
    /// <param name="childName">子孙名称</param>
    /// <returns>查找到子孙的Transform</returns>
    public static Transform GetChildByName(GameObject target, string childName)
    {
        return GetChildByName(target.transform, childName);
    }

    /// <summary>
    /// 设置child与parent的父子关系
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    public static void SetParent(Transform parent, Transform child)
    {
        child.SetParent(parent, false);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
        child.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 设置child与parent的父子关系
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    public static void SetParent(GameObject parent, Transform child)
    {
        SetParent(parent.transform, child);
    }

    /// <summary>
    /// 清理内存(一般在切换场景的时候调用)
    /// </summary>
    public static void ClearMemory()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    /// <summary>
    /// uint转换为Color
    /// </summary>
    public static Color ParseColor(uint input)
    {
        return new Color(
            ((input >> 16) & 0xff) / 255.0f,
            ((input >> 8) & 0xff) / 255.0f,
            (input & 0xff) / 255.0f,
            ((input >> 24) & 0xff) / 255.0f
        );
    }

    /// <summary>
    /// uint转换为Color, 但alpha值为1
    /// </summary>
    public static Color ParseSolidColor(uint input)
    {
        return new Color(
            ((input >> 16) & 0xff) / 255.0f,
            ((input >> 8) & 0xff) / 255.0f,
            (input & 0xff) / 255.0f,
            1.0f
        );
    }

    /// <summary>
    /// uint(为BGR格式)转换为Color, 但alpha值为1
    /// </summary>
    public static Color ParseSolidColorBGR(uint input)
    {
        return new Color(
            (input & 0xff) / 255.0f,
            ((input >> 8) & 0xff) / 255.0f,
            ((input >> 16) & 0xff) / 255.0f,
            1.0f
        );
    }

    /// <summary>
    /// 将Unix时间戳转为DateTime
    /// </summary>
    public static System.DateTime ConverToDateTime(double gmt)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        time = startTime.AddSeconds(gmt);

        return time;
    }

    /// <summary>
    /// dateTime转utc
    /// </summary>
    public static double DateTimeConvertToUTC(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }

    /// <summary>
    /// 总秒数 转成 H:M:S 
    /// </summary>
    public static string Second2Time(int second, string spl = ":")
    {
        second = Mathf.Max(second, 0);
        //int day = second / 86400;
        int hour = second / 3600;
        int min = (second % 3600) / 60;
        int sec = second % 60;

        return string.Format("{0:D2}{3}{1:D2}{3}{2:D2}", hour, min, sec, spl);
    }

    /// <summary>
    /// 获取本地时间(时分秒)
    /// </summary>
    public static string GetCurTime()
    {
        return DateTime.Now.ToString("MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 数组的拷贝Clone
    /// </summary>
    public static T[] CloneList<T>(T[] resArr) where T : ICloneable
    {
        T[] cloneArr = resArr == null ? null : new T[resArr.Length];
        if (cloneArr != null)
        {
            for (int i = 0, count = resArr.Length; i < count; ++i)
            {
                cloneArr[i] = (T)(resArr[i].Clone());
            }
        }

        return cloneArr;
    }

    /// <summary>
    /// 链表List的拷贝Clone
    /// </summary>
    public static List<T> CloneList<T>(List<T> resList) where T : ICloneable
    {
        List<T> cloneList = resList == null ? null : new List<T>();
        if (cloneList != null)
        {
            for (int i = 0, count = resList.Count; i < count; ++i)
            {
                T res = resList[i];
                T dest = (T)(res.Clone());
                cloneList.Add(dest);
            }
        }
        return cloneList;
    }

    /// <summary>
    /// 字典Dictionary的拷贝Clone
    /// </summary>
    public static Dictionary<T1, T2> CloneDictionary<T1, T2>(Dictionary<T1, T2> resDic) where T2 : ICloneable
    {
        Dictionary<T1, T2> cloneDic = resDic == null ? null : new Dictionary<T1, T2>();
        if (cloneDic != null)
        {
            foreach (KeyValuePair<T1, T2> keyValue in resDic)
            {
                T1 reskey = keyValue.Key;
                T1 deskey = reskey;
                ICloneable rescl = reskey as ICloneable;
                if (rescl != null)
                {
                    deskey = (T1)(rescl.Clone());
                }
                T2 desValue = (T2)(keyValue.Value.Clone());
                cloneDic.Add(deskey, desValue);
            }
        }
        return cloneDic;
    }

    /// <summary>
    /// 重置随机数种子
    /// </summary>
    public static void ResetRandomSeed()
    {
        int second = (int)(DateTimeConvertToUTC(System.DateTime.Now));
        string secondeStr = second.ToString();
        char[] cs = secondeStr.ToCharArray();
        Array.Reverse(cs);
        string newsecondeStr = new string(cs);
        newsecondeStr = newsecondeStr.Substring(0, newsecondeStr.Length - 1);
        second = int.Parse(newsecondeStr);

        // UnityEngine.Random.seed = second; // 该属性已弃用
        UnityEngine.Random.InitState(second);
    }
}