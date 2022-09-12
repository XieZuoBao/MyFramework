using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

/// <summary>
/// 模块资源加载器
/// </summary>
public class AssetLoader : BaseSingleton<AssetLoader>
{
    /// <summary>
    /// 平台对应的只读路径下的资源(非热更资源)
    /// key:模块(包)名字
    /// value:模块所有的资源
    /// </summary>
    public Dictionary<string, Hashtable> base2Assets;
    /// <summary>
    /// 平台对应的可读可写路径下的资源(热更资源)   
    /// key:模块(包)名字
    /// value:模块所有的资源
    /// </summary>
    public Dictionary<string, Hashtable> update2Assets;
    /// <summary>
    /// 所有的Bundref对象(BuildAssetBundle_Base+BuildAssetBundle_Update)
    /// key:包名
    /// </summary>
    public Dictionary<string, BundleRef> name2BundleRef;

    /// <summary>
    /// 模块资源加载器的构造函数
    /// </summary>
    public AssetLoader()
    {
        base2Assets = new Dictionary<string, Hashtable>();
        update2Assets = new Dictionary<string, Hashtable>();
        name2BundleRef = new Dictionary<string, BundleRef>();
    }

    /// <summary>
    /// 根据模块的json配置文件(资源依赖树) 创建 内存中的资源容器
    /// </summary>
    /// <param name="moduleABConfig"></param>  
    /// <returns></returns>
    public Hashtable ConfigAssembly(ModuleABConfig moduleABConfig)
    {
        /*
        //包数据
        Dictionary<string, BundleRef> name2BundleRef = new Dictionary<string, BundleRef>();
        foreach (KeyValuePair<string, BundleInfo> item in moduleABConfig.BundleDict)
        {
            string bundleName = item.Key;
            BundleInfo bundleInfo = item.Value;
            name2BundleRef[bundleName] = new BundleRef(bundleInfo);
        }
        */
        Hashtable path2AssetRef = new Hashtable();
        //资源数据
        for (int i = 0; i < moduleABConfig.AssetArray.Length; i++)
        {
            AssetInfo assetInfo = moduleABConfig.AssetArray[i];
            AssetRef assetRef = new AssetRef(assetInfo);
            assetRef.bundleRef = name2BundleRef[assetInfo.bundle_name];
            int count = assetInfo.dependencies.Count;
            assetRef.dependencies = new BundleRef[count];
            for (int index = 0; index < count; index++)
            {
                string bundleName = assetInfo.dependencies[index];
                assetRef.dependencies[index] = name2BundleRef[bundleName];
            }
            path2AssetRef.Add(assetInfo.asset_path, assetRef);
        }
        return path2AssetRef;
    }

    /// <summary>
    /// 异步加载模块的AB资源配置文件
    /// </summary>
    /// <param name="baseOrUpdate">只读路径还是可读可写路径</param>
    /// <param name="moduleName">模块的名字</param>
    /// <param name="bundleConfigName">AB资源配置文件的名字</param>
    /// <returns></returns>
    public async Task<ModuleABConfig> LoadAssetBundleConfig(BaseOrUpdate baseOrUpdate, string moduleName, string bundleConfigName)
    {
        string url = BundlePath(baseOrUpdate, moduleName, bundleConfigName);
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error) == true)
        {
            return JsonMapper.ToObject<ModuleABConfig>(request.downloadHandler.text);
        }
        return null;
    }

    /// <summary>
    /// 克隆一个GameObject对象
    /// </summary>
    /// <param name="moduleName">资源的模块名称(包名)</param>
    /// <param name="path">资源的相对路径:Assets_GAssets_Launch_Res_UI_Home_Prefab_HomeUI.prefab</param>
    /// <returns></returns>
    public GameObject Clone(string moduleName, string path)
    {
        AssetRef assetRef = LoadAssetRef<GameObject>(moduleName, path);
        if (assetRef == null || assetRef.asset == null)
        {
            return null;
        }

        GameObject go = UnityEngine.Object.Instantiate(assetRef.asset) as GameObject;
        //依赖绑定
        if (assetRef.children == null)
        {
            assetRef.children = new List<GameObject>();
        }
        assetRef.children.Add(go);
        return go;
    }

    /// <summary>
    /// 创建资源对象,并且将其赋予游戏对象go
    /// </summary>
    /// <typeparam name="T">资源的类型:如Sprite,AudioClip,Animation等</typeparam>
    /// <param name="moduleName">模块的名字</param>
    /// <param name="assetPath">资源的相对路径</param>
    /// <param name="go">资源加载后,要挂载到的游戏对象</param>
    /// <returns></returns>
    public T CreateAsset<T>(string moduleName, string assetPath, GameObject go) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject) || (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".prefab")))
        {
            Debug.LogError("不可以加载GameObject类型,请直接使用AssetLoader.Instance.Clone接口,assetPath=" + assetPath);
            return null;
        }

        if (go == null)
        {
            Debug.LogError("CreateAsset必须传递一个go作为挂载资源的对象!!!");
            return null;
        }

        AssetRef assetRef = LoadAssetRef<T>(moduleName, assetPath);
        if (assetRef == null || assetRef.asset == null)
        {
            return null;
        }
        //设置assetRef的依赖关系
        if (assetRef.children == null)
        {
            assetRef.children = new List<GameObject>();
        }
        assetRef.children.Add(go);
        return assetRef.asset as T;
    }

    /// <summary>
    /// 全局卸载函数
    /// </summary>
    /// <param name="module2Assets"></param>
    public void Unload(Dictionary<string, Hashtable> module2Assets)
    {
        foreach (string moduleName in module2Assets.Keys)
        {
            Hashtable path2AssetRef = module2Assets[moduleName];
            if (path2AssetRef == null)
            {
                continue;
            }
            foreach (AssetRef assetRef in path2AssetRef.Values)
            {
                if (assetRef.children == null || assetRef.children.Count == 0)
                {
                    continue;
                }
                for (int i = assetRef.children.Count - 1; i >= 0; i--)
                {
                    GameObject go = assetRef.children[i];
                    if (go == null)
                    {
                        //如果依赖这个assetRef资源的游戏对象为空了,解除关系
                        assetRef.children.RemoveAt(i);
                    }
                }
                //如果这个资源assetRef已经没有被任何GameObject所依赖,那么此assetRef就可以被卸载了
                if (assetRef.children.Count == 0)
                {
                    assetRef.asset = null;
                    Resources.UnloadUnusedAssets();
                    //对于assetRef所属的bundle,解除关系
                    assetRef.bundleRef.children.Remove(assetRef);
                    if (assetRef.bundleRef.children.Count == 0)
                    {
                        assetRef.bundleRef.bundle.Unload(true);
                    }
                    //对于assetRef所依赖的那些bundle列表解除关系
                    foreach (BundleRef bundleRef in assetRef.dependencies)
                    {
                        bundleRef.children.Remove(assetRef);
                        if (bundleRef.children.Count == 0)
                        {
                            bundleRef.bundle.Unload(true);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 加载 AssetRef 对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleName">模块名称</param>
    /// <param name="assetPath">资源的相对路径</param>
    /// <returns></returns>
    private AssetRef LoadAssetRef<T>(string moduleName, string assetPath) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (GlobalConfig.BundleMode == false)
        {
            return LoadAssetRef_Editor<T>(moduleName, assetPath);
        }
        else
        {
            return LoadAssetRef_Runtime<T>(moduleName, assetPath);
        }
#else
        return LoadAssetRef_Runtime<T>(moduleName, assetPath);
#endif
    }

    /// <summary>
    /// 在编辑器模式下加载 AssetRef 对象 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="moduleName">模块名字</param>
    /// <param name="assetPath">资源的相对路径</param>
    /// <returns></returns>
    private AssetRef LoadAssetRef_Editor<T>(string moduleName, string assetPath) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(assetPath))
        {
            return null;
        }
        AssetRef assetRef = new AssetRef(null);
        assetRef.asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        return assetRef;
#else
        return null;
#endif
    }

    /// <summary>
    /// 在AB包模式下加载 AssetRef 对象
    /// </summary>
    /// <typeparam name="T">要加载的资源类型</typeparam>
    /// <param name="moduleName">模块名字</param>
    /// <param name="assetPath">资源的相对路径</param>
    /// <returns>加载到内存中的资源数据内容</returns>
    private AssetRef LoadAssetRef_Runtime<T>(string moduleName, string assetPath) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(assetPath))
        {
            return null;
        }

        Hashtable module2AssetRef;
        ////先查找热更路径下的容器,再查找本地路径下容器
        //BaseOrUpdate witch = BaseOrUpdate.Update;
        //if (!update2Assets.TryGetValue(moduleName, out module2AssetRef))
        //{
        //    witch = BaseOrUpdate.Base;
        //    if (!base2Assets.TryGetValue(moduleName, out module2AssetRef))
        //    {
        //        Debug.LogError("未找到资源对应的模块: moduleName=" + moduleName + "    assetPath=" + assetPath);
        //        return null;
        //    }
        //}

        if (GlobalConfig.HotUpdate == true)
        {
            module2AssetRef = update2Assets[moduleName];
        }
        else
        {
            module2AssetRef = base2Assets[moduleName];
        }

        AssetRef assetRef = (AssetRef)module2AssetRef[assetPath];
        if (assetRef == null)
        {
            Debug.LogError("未找到资源: moduleName=" + moduleName + "    assetPath=" + assetPath);
            return null;
        }
        if (assetRef.asset != null)
        {
            return assetRef;
        }
        //1.处理assetRef依赖的BundleRef列表
        foreach (BundleRef oneBundleRef in assetRef.dependencies)
        {
            if (oneBundleRef.bundle == null)
            {
                string bundlePath = BundlePath(oneBundleRef.witch, moduleName, oneBundleRef.bundleInfo.bundle_name);
                oneBundleRef.bundle = AssetBundle.LoadFromFile(bundlePath);
            }

            if (oneBundleRef.children == null)
            {
                oneBundleRef.children = new List<AssetRef>();
            }
            oneBundleRef.children.Add(assetRef);
        }
        //2.处理assetRef属于哪个BundleRef对象
        BundleRef bundleRef = assetRef.bundleRef;
        if (bundleRef.bundle == null)
        {
            bundleRef.bundle = AssetBundle.LoadFromFile(BundlePath(bundleRef.witch, moduleName, bundleRef.bundleInfo.bundle_name));
        }

        if (bundleRef.children == null)
        {
            bundleRef.children = new List<AssetRef>();
        }
        bundleRef.children.Add(assetRef);
        //3.从bundle中提取asset
        assetRef.asset = assetRef.bundleRef.bundle.LoadAsset<T>(assetRef.assetInfo.asset_path);
        if (typeof(T) == typeof(GameObject) && assetRef.assetInfo.asset_path.EndsWith(".prefab"))
        {
            assetRef.isGameObject = true;
        }
        else
        {
            assetRef.isGameObject = false;
        }
        return assetRef;
    }

    /// <summary>
    /// 根据模块名字和bundle名字，返回其实际资源路径
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="bundle_name"></param>
    /// <returns></returns>
    private string BundlePath(BaseOrUpdate baseOrUpdate, string moduleName, string bundleName)
    {
        if (baseOrUpdate == BaseOrUpdate.Update)
        {
#if UNITY_EDITOR
            return Application.persistentDataPath + "/Bundles/" + moduleName + "/" + bundleName;
#elif UNITY_ANDROID
            return "file://" + Application.persistentDataPath + "/Bundles/" + moduleName + "/" + bundleName;
#elif UNITY_IOS
            return "file:///" + Application.persistentDataPath + "/Bundles/" + moduleName + "/" + bundleName;
#else
            return "file:///" + Application.persistentDataPath + "/Bundles/" + moduleName + "/" + bundleName;
#endif
        }
        else
        {
            return Application.streamingAssetsPath + "/" + moduleName + "/" + bundleName;
        }
    }
}