using System;
using System.Collections.Generic;

/// <summary>
/// 前缀树节点
/// </summary>
public class RedPointNode
{
    /// <summary>
    /// 节点名
    /// </summary>
    public string Name;
    /// <summary>
    /// 节点被经过的次数
    /// </summary>
    public int PassCount;
    /// <summary>
    /// 节点作为末尾节点的次数
    /// </summary>
    public int EndCount;
    /// <summary>
    /// 红点数(子节点的红点数之和)
    /// </summary>
    public int RedPointCount;
    /// <summary>
    /// 子节点
    /// </summary>
    public Dictionary<string, RedPointNode> Children;
    /// <summary>
    /// 红点更新时的回调
    /// </summary>
    public Dictionary<string, Action<int>> UpdateCallback;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name"></param>
    public RedPointNode(string name)
    {
        this.Name = name;
        this.PassCount = 0;
        this.EndCount = 0;
        this.RedPointCount = 0;
        Children = new Dictionary<string, RedPointNode>();
        UpdateCallback = new Dictionary<string, Action<int>>();
    }
}