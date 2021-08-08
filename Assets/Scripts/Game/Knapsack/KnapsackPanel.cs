/*
 * 
 *      Title:  背包系统
 * 
 *             
 *      Description: 
 *              背包面板,用户更新背包的业务逻辑
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnapsackPanel : UIBase
{
    public RectTransform content;
    CustomKnapsackTool<Item, KnapsackItem> ckTool;

    private void Start()
    {
        ckTool = new CustomKnapsackTool<Item, KnapsackItem>();
        ckTool.InitKnapsackItemPath("Prefabs/UI/Knapsack/KnapsackItem");
        ckTool.InitKnapsackviewPortHeight(content, 925);
        ckTool.InitItemSizeAndCol(190, 190, 2);
        ckTool.InitKnapsackData(KnapsackMgr.Instance.itemsList);
    }

    private void Update()
    {
        ckTool.CheckShowOrHide();
    }

    /*
    //背包格子容器-----背包格子的父对象
    private RectTransform contentTrans;
    //可视范围的高度
    private float ViewPortHeight;
    //格子尺寸
    private float sizeWidth = 190;
    private float sizeHeight = 190;
    //背包列数
    private int col = 3;
    //上一次可视范围之内的最小索引
    private int lastMinIndex = -1;
    //上一次可是范围之内的最大索引
    private int lastMaxIndex = -1;

    //当前显示的格子对象
    private Dictionary<int, GameObject> currentShowItemsDict = new Dictionary<int, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        contentTrans = GetComponentAtChildren<RectTransform>("Content");
        ViewPortHeight = GetComponentAtChildren<RectTransform>("Scroll View").rect.height;
    }

    private void Update()
    {
        CheckShowOrHide();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();

        //初始化背包容器content的高度
        //contentTrans.sizeDelta = new Vector2(0, KnapsackMgr.Instance.itemsList.Count / (float)col * sizeHeight);
        contentTrans.sizeDelta = new Vector2(0, Mathf.CeilToInt(KnapsackMgr.Instance.itemsList.Count / (float)col) * sizeHeight);
        //显示面板时,更新格子信息
        CheckShowOrHide();
    }

    /// <summary>
    /// 帧更新格子显示/隐藏的方法
    /// </summary>
    void CheckShowOrHide()
    {
        //防止往上滑动时溢出报错
        if (contentTrans.anchoredPosition.y < 0)
            return;
        //检测哪些格子需要显示出来
        //可视范围内要显示格子的起始索引
        int minIndex = (int)(contentTrans.anchoredPosition.y / sizeHeight) * col;
        //可是范围内要显示格子的最大索引
        int maxIndex = (int)((contentTrans.anchoredPosition.y + ViewPortHeight) / sizeHeight) * col + col - 1;
        //防止往下滑动时溢出报错
        if (maxIndex >= KnapsackMgr.Instance.itemsList.Count)
            maxIndex = KnapsackMgr.Instance.itemsList.Count - 1;

        //删减往上滑动时,上部要删减的格子对象
        for (int i = lastMinIndex; i < minIndex; i++)
        {
            DisposeInvisibleItem(i);
        }
        //删减往下滑动时,下部要删减的对象
        for (int i = maxIndex + 1; i <= lastMaxIndex; i++)
        {
            DisposeInvisibleItem(i);
        }
        //记录下本次滑动索引
        lastMinIndex = minIndex;
        lastMaxIndex = maxIndex;
        //根据起始,最大索引范围,创建格子
        for (int i = minIndex; i <= maxIndex; i++)
        {
            //若在索引范围内的格子仍在可视范围之内,则不必重新创建
            if (currentShowItemsDict.ContainsKey(i))
                continue;
            else
            {
                int index = i;
                currentShowItemsDict.Add(index, null);
                PoolMgr.Instance.GetGameObjectByPool("Prefabs/UI/Knapsack/KnapsackItem", (obj) =>
                {
                    //格子加载并创建完成后的逻辑
                    //设置格子父对象(将格子放入容器中)
                    obj.transform.SetParent(contentTrans);
                    //重置格子的缩放大小
                    obj.transform.localScale = Vector3.one;
                    //重置格子的相对位置
                    obj.transform.localPosition = new Vector3((index % col) * sizeWidth, -(index / 3) * sizeHeight, 0);
                    //更新格子信息
                    obj.GetComponent<KnapsackItem>().InitItemInfo(KnapsackMgr.Instance.itemsList[index]);
                    //更新当前显示的格子对象
                    //currentShowItemsDict.Add(index, obj);//对象是由对象池异步加载而来,有可能在当前帧并没有把对象给加载出来
                    if (currentShowItemsDict.ContainsKey(index))
                        currentShowItemsDict[index] = obj;
                    else//滑动操作过快,对象还没有创建出来(异步加载的影响)
                        PoolMgr.Instance.RecoverGameObjectToPools("Prefabs/UI/Knapsack/KnapsackItem", obj);
                });
            }//else_end
        }//for_end
    }

    /// <summary>
    /// 删减由于滑动操作后,不在可视范围内的格子
    /// </summary>
    /// <param name="index"></param>
    private void DisposeInvisibleItem(int index)
    {
        //判断当前显示的格子对象是否包含当前索引的对象,若有则删除(放回缓冲池)
        if (currentShowItemsDict.ContainsKey(index))
        {
            //当前索引的对象有可能还没有创建出来就要进行删除(滑动操作过于快)
            if (currentShowItemsDict[index] != null)
                PoolMgr.Instance.RecoverGameObjectToPools("Prefabs/UI/Knapsack/KnapsackItem", currentShowItemsDict[index]);
            currentShowItemsDict.Remove(index);
        }
    }
    */
}