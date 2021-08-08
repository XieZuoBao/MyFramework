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

/// <summary>
/// 格子类必须继承的接口,初始化格子信息
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IItemBase<T>
{
    void InitInfo(T info);
}

/// <summary>
/// 自定义无限背包工具类
/// </summary>
/// <typeparam name="T">数据来源类</typeparam>
/// <typeparam name="K">格子类</typeparam>
public class CustomKnapsackTool<T, K> where K : IItemBase<T>
{
    //背包格子容器-----背包格子的父对象
    private RectTransform contentTrans;
    //可视范围的高度
    private float ViewPortHeight;
    //格子尺寸
    private float itemWidth;
    private float itemHeight;
    //背包列数
    private int col;
    //上一次可视范围之内的最小索引
    private int lastMinIndex = -1;
    //上一次可是范围之内的最大索引
    private int lastMaxIndex = -1;

    //当前显示的格子对象
    private Dictionary<int, GameObject> currentShowItemsDict = new Dictionary<int, GameObject>();

    //背包数据来源
    private List<T> itemsList;
    //格子预设的路径
    private string knapsackItemPath;

    /// <summary>
    /// 初始化格子预设的路径
    /// </summary>
    /// <param name="path"></param>
    public void InitKnapsackItemPath(string path)
    {
        this.knapsackItemPath = path;
    }

    /// <summary>
    /// 初始化背包格子大小及列数
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="col"></param>
    public void InitItemSizeAndCol(int width, int height, int col)
    {
        this.itemWidth = width;
        this.itemHeight = height;
        this.col = col;
    }

    /// <summary>
    /// 初始化背包容器Content以及可视范围高度
    /// </summary>
    /// <param name="content">背包容器</param>
    /// <param name="height">可视范围高度</param>
    public void InitKnapsackviewPortHeight(RectTransform content, float height)
    {
        this.contentTrans = content;
        this.ViewPortHeight = height;
    }

    /// <summary>
    /// 初始化背包数据来源,并设置背包容器content高度
    /// </summary>
    /// <param name="itemsList"></param>
    public void InitKnapsackData(List<T> itemsList)
    {
        this.itemsList = itemsList;
        //初始化背包容器content的高度
        //contentTrans.sizeDelta = new Vector2(0, itemsList.Count / (float)col * sizeHeight);
        contentTrans.sizeDelta = new Vector2(0, Mathf.CeilToInt(itemsList.Count / (float)col) * itemHeight);
    }

    /// <summary>
    /// 帧更新格子显示/隐藏的方法
    /// </summary>
    public void CheckShowOrHide()
    {
        //检测哪些格子需要显示出来
        //可视范围内要显示格子的起始索引
        int minIndex = (int)(contentTrans.anchoredPosition.y / itemHeight) * col;
        //可是范围内要显示格子的最大索引
        int maxIndex = (int)((contentTrans.anchoredPosition.y + ViewPortHeight) / itemHeight) * col + col - 1;
        //最小值判断
        if (minIndex < 0)
            minIndex = 0;
        //最大值判断
        if (maxIndex >= itemsList.Count)
            maxIndex = itemsList.Count - 1;
        //可是范围内的索引发生变化时才更新删减
        if (minIndex != lastMinIndex || maxIndex != lastMaxIndex)
        {
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
                PoolMgr.Instance.GetGameObjectByPool(knapsackItemPath, (obj) =>
                {
                    //格子加载并创建完成后的逻辑
                    //设置格子父对象(将格子放入容器中)
                    obj.transform.SetParent(contentTrans);
                    //重置格子的缩放大小
                    obj.transform.localScale = Vector3.one;
                    //重置格子的相对位置
                    obj.transform.localPosition = new Vector3((index % col) * itemWidth, -(index / col) * itemHeight, 0);
                    //更新格子信息
                    obj.GetComponent<K>().InitInfo(itemsList[index]);
                    //更新当前显示的格子对象
                    //currentShowItemsDict.Add(index, obj);//对象是由对象池异步加载而来,有可能在当前帧并没有把对象给加载出来
                    if (currentShowItemsDict.ContainsKey(index))
                        currentShowItemsDict[index] = obj;
                    else//滑动操作过快,对象还没有创建出来(异步加载的影响)
                        PoolMgr.Instance.RecoverGameObjectToPools(knapsackItemPath, obj);
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
                PoolMgr.Instance.RecoverGameObjectToPools(knapsackItemPath, currentShowItemsDict[index]);
            currentShowItemsDict.Remove(index);
        }
    }
}