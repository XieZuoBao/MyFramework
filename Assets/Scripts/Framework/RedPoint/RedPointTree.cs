using System;
using UnityEngine;

/// <summary>
/// 红点系统树,前缀树结构
/// </summary>
public class RedPointTree
{
    /// <summary>
    /// 根节点
    /// </summary>
    private RedPointNode root;

    /// <summary>
    /// 构造函数
    /// </summary>
    public RedPointTree()
    {
        root = new RedPointNode("Root");
    }

    /// <summary>
    /// 插入节点
    /// </summary>
    /// <param name="name"></param>
    public void InsertNode(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;

        if (SearchNode(name) != null)
        {
            GameLogger.LogWarning("你已插入过节点了,name:" + name);
            return;
        }

        // node从根节点出发
        RedPointNode node = root;
        node.PassCount++;
        //将名字按|符号分割
        string[] pathList = name.Split('|');
        for (int i = 0; i < pathList.Length; i++)
        {
            if (!node.Children.ContainsKey(pathList[i]))
            {
                node.Children.Add(pathList[i], new RedPointNode(pathList[i]));
            }
            node = node.Children[pathList[i]];
            node.PassCount++;
        }
        node.EndCount++;
    }

    /// <summary>
    /// 查询节点是否在树中并返回节点
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public RedPointNode SearchNode(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        RedPointNode node = root;
        string[] pathList = name.Split('|');
        for (int i = 0; i < pathList.Length; i++)
        {
            if (!node.Children.ContainsKey(pathList[i]))
            {
                return null;
            }
            node = node.Children[pathList[i]];
        }

        if (node.EndCount > 0)
        {
            return node;
        }
        return null;
    }

    /// <summary>
    /// 删除某个节点
    /// </summary>
    /// <param name="name"></param>
    public void DeleteNode(string name)
    {
        if (SearchNode(name) == null)
        {
            return;
        }

        RedPointNode node = root;
        node.PassCount--;
        string[] pathList = name.Split('|');
        for (int i = 0; i < pathList.Length; i++)
        {
            RedPointNode childNode = node.Children[pathList[i]];
            childNode.PassCount--;
            if (childNode.PassCount == 0)
            {
                node.Children.Remove(pathList[i]);
            }
            node = childNode;
        }
        node.EndCount--;
    }

    /// <summary>
    /// 修改节点的红点数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delta"></param>
    public void ChangeRedPointCount(string name, int delta)
    {
        RedPointNode targetNode = SearchNode(name);
        if (targetNode == null)
            return;
        //如果是减红点,并且红点数不够减,则调整delta,使其不减为0
        if (delta < 0 && targetNode.RedPointCount + delta < 0)
        {
            delta = -targetNode.RedPointCount;
        }

        RedPointNode node = root;
        string[] pathList = name.Split('|');
        for (int i = 0; i < pathList.Length; i++)
        {
            RedPointNode childNode = node.Children[pathList[i]];
            childNode.RedPointCount += delta;
            node = childNode;
            //调用回调函数
            foreach (Action<int> action in childNode.UpdateCallback.Values)
            {
                action(node.RedPointCount);
            }
        }
    }

    /// <summary>
    /// 设置红点更新回调函数
    /// </summary>
    /// <param name="name">节点名</param>
    /// <param name="callback">回调函数</param>
    public void SetCallback(string name, Action<int> callback)
    {
        RedPointNode node = SearchNode(name);
        if (node == null)
        {
            return;
        }

        node.UpdateCallback[name] = callback;
    }

    /// <summary>
    /// 查询节点的红点数
    /// </summary>
    /// <param name="name"></param>
    public int GetRedPointCount(string name)
    {
        RedPointNode node = SearchNode(name);
        if (node == null)
        {
            return 0;
        }

        return node.RedPointCount;
    }
}