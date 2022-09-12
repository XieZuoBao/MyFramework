using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

/// <summary>
/// 在内存中的一个Bundle对象
/// </summary>
public class BundleRef
{
    /// <summary>
    /// 这个 bundle 的静态配置信息(包名,包的散列码,包名下的所有资源的相对路径)
    /// ModuleABConfig配置文件中的BundleDict
    /// </summary>
    public BundleInfo bundleInfo;
    /// <summary>
    /// 加载到内存的Bundle对象(null)
    /// </summary>
    public AssetBundle bundle;
    /// <summary>
    /// BundleRef对象被哪些AssetRef对象依赖(null)
    /// </summary>
    public List<AssetRef> children;
    /// <summary>
    /// BundleRef对应的AB配置文件需要从哪里下载(只读文件夹还是热更可读可写文件夹)
    /// </summary>
    public BaseOrUpdate witch;

    /// <summary>
    /// BundleRef的构造函数
    /// </summary>
    /// <param name="bundleInfo"></param>
    /// <param name="witch"></param>
    public BundleRef(BundleInfo bundleInfo,BaseOrUpdate witch)
    {
        this.bundleInfo = bundleInfo;
        this.witch = witch;
    }
}
