using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPointTestData : BaseSingleton<RedPointTestData>
{
    public RedPointTree RedTree;

    public void Init()
    {
        RedTree = new RedPointTree();
        //初始化红点系统测试数据
        nodeList.Add(Root);
        nodeList.Add(ModelA);
        nodeList.Add(ModelA_Sub_1);
        nodeList.Add(ModelA_Sub_2);
        nodeList.Add(ModelB);
        nodeList.Add(ModelB_Sub_1);
        nodeList.Add(ModelB_Sub_2);
        for (int i = 0; i < nodeList.Count; i++)
        {
            RedTree.InsertNode(nodeList[i]);
        }
        //塞入红点测试数据
        RedTree.ChangeRedPointCount(ModelA_Sub_1, 1);
        RedTree.ChangeRedPointCount(ModelA_Sub_2, 1);
        RedTree.ChangeRedPointCount(ModelB_Sub_1, 1);
        RedTree.ChangeRedPointCount(ModelB_Sub_2, 1);
    }

    //测试数据
    public string Root = "Root";
    public string ModelA = "Root|ModelA";
    public string ModelA_Sub_1 = "Root|ModelA|ModelA_Sub_1";
    public string ModelA_Sub_2 = "Root|ModelA|ModelA_Sub_2";
    public string ModelB = "Root|ModelB";
    public string ModelB_Sub_1 = "Root|ModelB|ModelB_Sub_1";
    public string ModelB_Sub_2 = "Root|ModelB|ModelB_Sub_2";
    public List<string> nodeList = new List<string>();
}
