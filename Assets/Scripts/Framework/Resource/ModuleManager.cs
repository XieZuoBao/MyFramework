using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 模块管理器 工具类
/// 管理AsserLoader和Downloader
/// </summary>
public class ModuleManager : BaseSingleton<ModuleManager>
{
    /// <summary>
    /// 加载一个模块 唯一的对外API
    /// </summary>
    /// <param name="moduleConfig"></param>
    /// <param name="moduleAction"></param> 
    public async Task<bool> Load(ModuleConfig moduleConfig)
    {
        //不是热更模式
        if (GlobalConfig.HotUpdate == false)
        {
            if (GlobalConfig.BundleMode == false)
            {
                return true;
            }
            else//打大版本整包模式
            {
                bool baseBundleOk = await LoadBase_Bundle(moduleConfig.moduleName);
                if (baseBundleOk == false)
                {
                    return false;
                }
                return await LoadBase(moduleConfig.moduleName);
            }
        }
        else//热更模式
        {
            await Downloader.Instance.Download(moduleConfig);

            bool updateBundleOk = await LoadUpdate_Bundle(moduleConfig.moduleName);
            if (updateBundleOk == false)
            {
                return false;
            }

            bool baseBundleOk = await LoadBase_Bundle(moduleConfig.moduleName);
            if (baseBundleOk == false)
            {
                return false;
            }

            bool updateOk = await LoadUpdate(moduleConfig.moduleName);
            return updateOk;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    async Task<bool> LoadBase(string moduleName)
    {
        ModuleABConfig moduleABConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Base, moduleName, moduleName.ToLower() + ".json");
        if (moduleABConfig == null)
        {
            return false;
        }
        Debug.Log($"模块{moduleName}的只读路径 包含的AB包总数量: { moduleABConfig.BundleDict.Count}");
        Hashtable Path2AssetRef = AssetLoader.Instance.ConfigAssembly(moduleABConfig);
        AssetLoader.Instance.base2Assets.Add(moduleName, Path2AssetRef);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    async Task<bool> LoadUpdate(string moduleName)
    {
        ModuleABConfig moduleABConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Update, moduleName, moduleName.ToLower() + ".json");
        if (moduleABConfig == null)
        {
            return false;
        }
        Debug.Log($"模块{moduleName}的可读可写路径 包含的AB包总数量: { moduleABConfig.BundleDict.Count}");
        Hashtable Path2AssetRef = AssetLoader.Instance.ConfigAssembly(moduleABConfig);
        AssetLoader.Instance.update2Assets.Add(moduleName, Path2AssetRef);
        return true;
    }

    /// <summary>
    /// 构建只读路径下的bundleRef对象,填充到AssetLoader类的name2BundleRef容器中
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    async Task<bool> LoadBase_Bundle(string moduleName)
    {
        ModuleABConfig moduleABConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Base, moduleName, moduleName.ToLower() + ".json");
        if (moduleABConfig == null)
        {
            Debug.LogError("本地构建BundleRef对象出错:moduleName=" + moduleName);
            return false;
        }
        foreach (KeyValuePair<string, BundleInfo> keyValue in moduleABConfig.BundleDict)
        {
            string bundleName = keyValue.Key;
            if (AssetLoader.Instance.name2BundleRef.ContainsKey(bundleName) == false)
            {
                BundleInfo bundleInfo = keyValue.Value;
                AssetLoader.Instance.name2BundleRef[bundleName] = new BundleRef(bundleInfo, BaseOrUpdate.Base);
            }
        }
        return true;
    }

    /// <summary>
    /// 构建可读可写路径下的bundleRef对象,填充到AssetLoader类的name2BundleRef容器中！
    /// </summary>
    /// <param name="moduelName"></param>
    /// <returns></returns>
    async Task<bool> LoadUpdate_Bundle(string moduleName)
    {
        ModuleABConfig moduleABConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Update, moduleName, moduleName.ToLower() + ".json");
        if (moduleABConfig == null)
        {
            Debug.LogError("热更构建BundleRef对象出错:moduleName=" + moduleName);
            return false;
        }
        foreach (KeyValuePair<string, BundleInfo> keyValue in moduleABConfig.BundleDict)
        {
            string bundleName = keyValue.Key;
            BundleInfo bundleInfo = keyValue.Value;
            AssetLoader.Instance.name2BundleRef[bundleName] = new BundleRef(bundleInfo, BaseOrUpdate.Update);
        }
        return true;
    }
}