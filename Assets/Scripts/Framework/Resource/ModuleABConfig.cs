using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个包数据[用于系列化json文件]
/// 每个包都有若干资源
/// </summary>
public class BundleInfo
{
    /// <summary>
    /// 包名
    /// </summary>
    public string bundle_name;
    /// <summary> 
    /// 包名对应的散列码
    /// </summary>
    public string crc;
    /// <summary>
    /// 包大小(单位是子节)
    /// </summary>
    public int size;
    /// <summary>
    /// 对应包名下所有资源的相对路径列表
    /// </summary>
    public List<string> assets;
}

/// <summary>
/// 一个Asset数据(资源)[用于系列和json文件]
/// 每个资源都归属于某一个包,这个资源可能有若干依赖
/// </summary>
public class AssetInfo
{
    /// <summary>
    /// 这个资源的相对路径
    /// </summary>
    public string asset_path;
    /// <summary>
    /// 这个资源所属的包名
    /// </summary>
    public string bundle_name;
    /// <summary>
    /// 这个资源依赖的包列表
    /// </summary>
    public List<string> dependencies;
}

/// <summary>
/// 存在本地的AB包配置文件(json格式)
/// 1.包名部分数据
/// 2.资源部分数据
/// </summary>
public class ModuleABConfig
{
    /// <summary>
    /// 所有的AB包列表
    /// key:包名  value:包存储的数据
    /// </summary>
    public Dictionary<string, BundleInfo> BundleDict;
    /// <summary>
    /// 要打包的所有资源列表
    /// </summary>
    public AssetInfo[] AssetArray;

    public ModuleABConfig()
    {

    }

    public ModuleABConfig(int assetCount)
    {
        BundleDict = new Dictionary<string, BundleInfo>();
        AssetArray = new AssetInfo[assetCount];
    }

    /// <summary>
    /// 新增一个包
    /// </summary>
    /// <param name="bundleName">包名</param>
    /// <param name="bundleInfo">包数据</param>
    public void AddBundle(string bundleName, BundleInfo bundleInfo)
    {
        BundleDict[bundleName] = bundleInfo;
    }

    /// <summary>
    /// 新增一个资源
    /// </summary>
    /// <param name="index">资源索引</param>
    /// <param name="assetInfo">资源数据</param>
    public void AddAsset(int index, AssetInfo assetInfo)
    {
        AssetArray[index] = assetInfo;
    }
}
