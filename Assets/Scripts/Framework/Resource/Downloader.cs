using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 下载器 热更新下载工具类
/// 热更的核心逻辑是:先从服务器下载模块对应的AB资源配置文件,然后和本地的Update路径下的AB配置文件做对比,
///                 生成那些内容有变化的(通过Bundle的CRC值判断)或者是新增的Bundle文件列表,我们根据这个列表,一个一个的下载Bundle文件！
/// </summary>
public class Downloader : BaseSingleton<Downloader>
{
    /// <summary>
    /// 根据模块的配置,下载对应的模块
    /// </summary>
    /// <param name="moduleConfig">模块的配置对象</param>
    /// <returns>是否下载资源成功</returns>
    public async Task Download(ModuleConfig moduleConfig)
    {
        //用来存放热更下来的资源的本地路径
        string updatePath = GetUpdatePath(moduleConfig.moduleName);
        //远程服务器上这个模块的AB资源配置文件的URL
        string configURL = GetServerURL(moduleConfig, moduleConfig.moduleName.ToLower() + ".json");
        //下载远程配置文件到指定路径,命名后缀不上temp标签与本地做区分
        UnityWebRequest request = UnityWebRequest.Get(configURL);
        //下载文件并将文件保存到磁盘
        request.downloadHandler = new DownloadHandlerFile(string.Format("{0}/{1}_temp.json", updatePath, moduleConfig.moduleName.ToLower()));
        Debug.Log("下载到本地路径:" + updatePath);
        await request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error) == false)
        {
            Debug.LogWarning($"下载模块{moduleConfig.moduleName}的AB配置文件:{request.error}");
            //下载出错,断点续传
            bool result = await ShowMessageBox("网络异常,请检查网络后点击 继续下载", "继续下载", "退出游戏");
            if (result == false)
            {
                Application.Quit();
                return;
            }
            await Download(moduleConfig);
            return;
        }
        //需要下载的差异包和需要删除的本地包
        Tuple<List<BundleInfo>, BundleInfo[]> tuple = await GetDownloadList(moduleConfig.moduleName);
        List<BundleInfo> downloadList = tuple.Item1;
        BundleInfo[] removeList = tuple.Item2;
        long downloadSize = CalculateSize(downloadList);
        if (downloadSize == 0)
        {
            //热更时,发现没有新内容需要更新时,也需要调用Clear函数
            Clear(moduleConfig, removeList);
            return;
        }

        bool boxResult = await ShowMessageBox(moduleConfig, downloadSize);
        if (boxResult == false)
        {
            Application.Quit();
            return;
        }

        //下载差异包
        await ExecuteDownload(moduleConfig, downloadList);

        //下载完成清理本地需要删除的包
        Clear(moduleConfig, removeList);
        return;
    }

    /// <summary>
    /// 异步执行下载行为
    /// </summary>
    /// <param name="moduleConfig"></param>
    /// <param name="bundleInfoList">返回的List包含的是还未下载的Bundle</param>
    /// <returns></returns>
    async Task ExecuteDownload(ModuleConfig moduleConfig, List<BundleInfo> bundleInfoList)
    {
        while (bundleInfoList.Count > 0)
        {
            BundleInfo bundleInfo = bundleInfoList[0];
            UnityWebRequest request = UnityWebRequest.Get(GetServerURL(moduleConfig, bundleInfo.bundle_name));
            string updatePath = GetUpdatePath(moduleConfig.moduleName);
            request.downloadHandler = new DownloadHandlerFile(string.Format("{0}/" + bundleInfo.bundle_name, updatePath));
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("下载资源:" + bundleInfo.bundle_name + " 成功!!!");
                bundleInfoList.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
        //断点续传
        if (bundleInfoList.Count > 0)
        {
            bool result = await ShowMessageBox("网络异常,请检查网络后点击 继续下载", "继续下载", "退出游戏");
            if (result == false)
            {
                Application.Quit();
                return;
            }
            await ExecuteDownload(moduleConfig, bundleInfoList);
            return;
        }
    }

    /// <summary>
    /// 对于给定模块，返回其所有需要下载的BundleInfo组成的List和需要删除的本地Bundle数组
    /// </summary>
    /// <param name="moduelName"></param>
    /// <returns></returns>
    async Task<Tuple<List<BundleInfo>, BundleInfo[]>> GetDownloadList(string moduleName)
    {
        //从服务器下载的热更配置数据
        ModuleABConfig serverConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Update, moduleName, moduleName.ToLower() + "_temp.json");
        if (serverConfig == null)
        {
            return null;
        }
        //注意:这里不用判断localConfig是否存在 本地的localConfig确实可能不存在,比如在此模块第一次热更新之前,本地update路径下啥都没有
        //本地的热更配置数据
        ModuleABConfig localConfig = await AssetLoader.Instance.LoadAssetBundleConfig(BaseOrUpdate.Update, moduleName, moduleName.ToLower() + ".json");
        //返回其所有需要下载的BundleInfo组成的List和需要删除的本地Bundle数组
        return CalculateDiff(moduleName, localConfig, serverConfig);
    }

    /// <summary>
    /// 通过两个AB资源配置文件,返回出有差异需要下载的Bundle列表和需要删除的本地Bundle数组
    /// Tuple<T1,T2>可以让C#返回多个返回值！
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="localConfig"></param>
    /// <param name="serverConfig"></param>
    /// <returns></returns>
    private Tuple<List<BundleInfo>, BundleInfo[]> CalculateDiff(string moduleName, ModuleABConfig localConfig, ModuleABConfig serverConfig)
    {
        //需要下载的bundle文件列表
        List<BundleInfo> bundleInfoList = new List<BundleInfo>();
        //需要删除的本地bundle文件列表
        Dictionary<string, BundleInfo> localBundleDict = new Dictionary<string, BundleInfo>();
        if (localConfig != null)
        {
            foreach (BundleInfo bundleInfo in localConfig.BundleDict.Values)
            {
                string uniqueId = string.Format("{0}|{1}", bundleInfo.bundle_name, bundleInfo.crc);
                localBundleDict.Add(uniqueId, bundleInfo);
            }
        }

        //1.对比服务器热更资源与本地资源,找到有差异的bundle文件,放到bundleList容器中
        //2.对于那些遗留在本地的无用的bundle文件,把它过滤在localBundleDic容器里
        foreach (BundleInfo bundleInfo in serverConfig.BundleDict.Values)
        {
            string uniqueId = string.Format("{0}|{1}", bundleInfo.bundle_name, bundleInfo.crc);
            if (localBundleDict.ContainsKey(uniqueId) == false)
            {
                bundleInfoList.Add(bundleInfo);
            }
            //else
            //{
            //    localBundleDict.Remove(uniqueId);
            //}
        }
        //对于那些遗留在本地的无用的bundle文件,要清除,不然本地文件越积累越多
        //BundleInfo[] removeArray = localBundleDict.Values.ToArray();
        List<BundleInfo> removeList = new List<BundleInfo>();
        if (localConfig != null)
        {
            foreach (KeyValuePair<string, BundleInfo> localBundle in localConfig.BundleDict)
            {
                if (serverConfig.BundleDict.ContainsKey(localBundle.Key) == false)
                {
                    removeList.Add(localBundle.Value);
                }
            }
        }
        //返回 需要下载的Bundle列表和需要删除的本地Bundle数组
        return new Tuple<List<BundleInfo>, BundleInfo[]>(bundleInfoList, removeList.ToArray());
    }

    /// <summary>
    /// 客户端给定模块的热更资源存放地址
    /// </summary>
    /// <param name="moduleName"></param>
    /// <returns></returns>
    string GetUpdatePath(string moduleName)
    {
        return Application.persistentDataPath + "/Bundles/" + moduleName;
    }

    /// <summary>
    /// 返回 给定模块的给定文件在服务器端的完整URL
    /// </summary>
    /// <param name="moduleConfig">模块配置对象</param>
    /// <param name="fileName">文件名字</param>
    /// <returns></returns>
    string GetServerURL(ModuleConfig moduleConfig, string fileName)
    {
#if UNITY_ANDROID
        return string.Format("{0}/{1}/{2}", moduleConfig.DownloadURL, "Android", fileName);
#elif UNITY_IOS
        return string.Format("{0}/{1}/{2}", moduleConfig.DownloadURL, "IOS", fileName);
#elif UNITY_STANDALONE_WIN
        return string.Format("{0}/{1}/{2}", moduleConfig.DownloadURL, "StandaloneWindows64", fileName);
#endif
    }

    /// <summary>
    /// 计算需要下载的资源大小 单位是字节
    /// </summary>
    /// <param name="bundleInfoList"></param>
    /// <returns></returns>
    static long CalculateSize(List<BundleInfo> bundleInfoList)
    {
        long totalSize = 0;
        for (int i = 0; i < bundleInfoList.Count; i++)
        {
            totalSize += bundleInfoList[i].size;
        }
        return totalSize;
    }

    /// <summary>
    /// 弹出热更对话框
    /// </summary>
    /// <param name="moduleConfig"></param>
    /// <param name="totalSize"></param>
    /// <returns></returns>
    static async Task<bool> ShowMessageBox(ModuleConfig moduleConfig, long totalSize)
    {
        string downloadSize = SizeToString(totalSize);
        string messageInfo = $"发现新版本,版本号为:{moduleConfig.moduleVersion}\n需要下载热更包,大小为:{downloadSize}";
        MessageBox messageBox = new MessageBox(messageInfo, "开始下载", "退出游戏");
        MessageBox.BoxResult result = await messageBox.GetReplyAsync();
        messageBox.Close();
        if (result == MessageBox.BoxResult.First) 
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 给网络失败时的弹出对话框
    /// </summary>
    /// <param name="messageInfo"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    static async Task<bool> ShowMessageBox(string messageInfo, string first, string second)
    {
        MessageBox messageBox = new MessageBox(messageInfo, first, second);
        MessageBox.BoxResult result = await messageBox.GetReplyAsync();
        messageBox.Close();
        if (result == MessageBox.BoxResult.First)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 工具函数 把字节数转换成字符串形式
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    static string SizeToString(long size)
    {
        string sizeStr = "";

        if (size >= 1024 * 1024)
        {
            long m = size / (1024 * 1024);

            size = size % (1024 * 1024);

            sizeStr += $"{m}[M]";
        }

        if (size >= 1024)
        {
            long k = size / 1024;

            size = size % 1024;

            sizeStr += $"{k}[K]";
        }

        long b = size;

        sizeStr += $"{b}[B]";

        return sizeStr;
    }

    /// <summary>
    /// 模块热更新完成后的善后工作
    /// </summary>
    /// <param name="moduleConfig"></param>
    /// <param name="removeArray"></param>
    void Clear(ModuleConfig moduleConfig, BundleInfo[] removeArray)
    {
        string moduleName = moduleConfig.moduleName;
        //本地存放热更资源的地址
        string updatePath = GetUpdatePath(moduleName);
        //删除旧资源
        for (int i = removeArray.Length - 1; i >= 0; i--)
        {
            BundleInfo bundleInfo = removeArray[i];
            string filePath = string.Format("{0}/" + bundleInfo.bundle_name, updatePath);
            File.Delete(filePath);
        }
        //删除旧的配置文件
        string oldFile = string.Format("{0}/{1}.json", updatePath, moduleName.ToLower());
        if (File.Exists(oldFile))
        {
            File.Delete(oldFile);
        }
        //用新的配置文件替代
        string newFile = string.Format("{0}/{1}_temp.json", updatePath, moduleName.ToLower());
        File.Move(newFile, oldFile);
    }
}
