using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心模块
/// <para>观察者设计模式:降低程序耦合性,减少程序复杂度</para>
/// </summary>
public class EventCenter : BaseSingleton<EventCenter>
{
    //事件名称-监听此事件对应的委托们(可以是多个)
    //private Dictionary<string, UnityAction<object>> eventDict = new Dictionary<string, UnityAction<object>>();//有装修拆箱
    private Dictionary<string, IEventInfo> eventDict = new Dictionary<string, IEventInfo>();

    #region 不带参数的事件类型
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">处理事件的委托函数</param>
    public void AddEventListener(string eventName, UnityAction action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] += action;
            (eventDict[eventName] as EventInfo).action += action;
        }

        else//没有对应的事件监听
        {
            //eventDict.Add(eventName, action);
            eventDict.Add(eventName, new EventInfo(action));
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] -= action;
            (eventDict[eventName] as EventInfo).action -= action;
        }

        else//没有对应的事件监听
            GameLogger.LogError("事件移除有误,请检测----" + eventName);
    }
    /// <summary>
    /// 事件分发
    /// </summary>
    /// <param name="eventName">触发的事件名称</param>
    public void EventTrigger(string eventName)
    {
        //分发事件
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName].Invoke(info);
            if (eventDict[eventName] as EventInfo != null)
                (eventDict[eventName] as EventInfo).action.Invoke();
        }

    }
    #endregion

    #region 带一个参数的事件类型
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">处理事件的委托函数</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] += action;
            (eventDict[eventName] as EventInfo<T>).action += action;
        }

        else//没有对应的事件监听
        {
            //eventDict.Add(eventName, action);
            eventDict.Add(eventName, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] -= action;
            (eventDict[eventName] as EventInfo<T>).action -= action;
        }

        else//没有对应的事件监听
            GameLogger.LogError("事件移除有误,请检测----" + eventName);
    }
    /// <summary>
    /// 事件分发
    /// </summary>
    /// <param name="eventName">触发的事件名称</param>
    public void EventTrigger<T>(string eventName, T info)
    {
        //分发事件
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName].Invoke(info);
            if (eventDict[eventName] as EventInfo<T> != null)
                (eventDict[eventName] as EventInfo<T>).action.Invoke(info);
        }

    }
    #endregion

    #region 带两个个泛型参数的事件类型
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">处理事件的委托函数</param>
    public void AddEventListener<T, W>(string eventName, UnityAction<T, W> action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] += action;
            (eventDict[eventName] as EventInfo<T, W>).action += action;
        }

        else//没有对应的事件监听
        {
            //eventDict.Add(eventName, action);
            eventDict.Add(eventName, new EventInfo<T, W>(action));
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T, W>(string eventName, UnityAction<T, W> action)
    {
        //有对应的事件监听
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName] -= action;
            (eventDict[eventName] as EventInfo<T, W>).action -= action;
        }

        else//没有对应的事件监听
            GameLogger.LogError("事件移除有误,请检测----" + eventName);
    }
    /// <summary>
    /// 事件分发
    /// </summary>
    /// <param name="eventName">触发的事件名称</param>
    public void EventTrigger<T, W>(string eventName, T tInfo, W wInfo)
    {
        //分发事件
        if (eventDict.ContainsKey(eventName))
        {
            //eventDict[eventName].Invoke(info);
            if (eventDict[eventName] as EventInfo<T, W> != null)
                (eventDict[eventName] as EventInfo<T, W>).action.Invoke(tInfo, wInfo);
        }

    }
    #endregion

    #region 若有必要,可以继续拓展带更多参数的事件类型
    #endregion

    /// <summary>
    /// 过场景的时候清空所有的事件监听
    /// </summary>
    public void Clear()
    {
        eventDict.Clear();
    }
}

/// <summary>
/// 基类接口----辅助实现在事件中心模块中,事件的分发时出现装箱和拆箱的性能损耗
/// </summary>
public interface IEventInfo
{

}

/// <summary>
/// 普通类
/// </summary>
public class EventInfo : IEventInfo
{
    public UnityAction action;

    public EventInfo(UnityAction callBack)
    {
        action += callBack;
    }
}

/// <summary>
/// 泛型类一
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> action;

    public EventInfo(UnityAction<T> callBack)
    {
        action += callBack;
    }
}

/// <summary>
/// 泛型类二
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventInfo<T, W> : IEventInfo
{
    public UnityAction<T, W> action;

    public EventInfo(UnityAction<T, W> callBack)
    {
        action += callBack;
    }
}