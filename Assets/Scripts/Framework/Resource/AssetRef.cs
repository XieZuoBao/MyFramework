using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 内存中的单个资源对象
/// </summary>
public class AssetRef
{
    /// <summary>
    /// 这个资源的静态配置信息(资源的相对路径,所属包名,依赖的包列表) 
    /// ModuleABConfig配置文件中的AssetArray
    /// </summary>
    public AssetInfo assetInfo;
    /// <summary>
    /// 这个资源所属的BundleRef对象
    /// </summary>
    public BundleRef bundleRef;
    /// <summary>
    /// 这个资源所依赖的BundleRef对象列表
    /// </summary>
    public BundleRef[] dependencies;
    /// <summary>
    /// 从bundle文件中提取出来的资源对象(null)
    /// </summary>
    public Object asset;
    /// <summary>
    /// 这个资源是否是prefab
    /// </summary>
    public bool isGameObject;
    /// <summary>
    /// 这个AssetRef对象被哪些GameObject依赖(null)
    /// </summary>
    public List<GameObject> children;
    /// <summary>
    /// AssetRef对象的构造函数
    /// </summary>
    /// <param name="assetInfo"></param>
    public AssetRef(AssetInfo assetInfo)
    {
        this.assetInfo = assetInfo;
    }
}
