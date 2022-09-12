using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 生成AssetBundle的编辑器工具
/// 包名:GAssets文件夹里的所有子文件夹的"相对路径名称(以gassets开头,'/'替换为'_')" 
/// </summary>
public class ABEditor : MonoBehaviour
{
    /// <summary>
    /// 热更资源的根目录
    /// </summary>
    public static string rootPath = Application.dataPath + "/GAssets";
    /// <summary>
    /// 所有需要打包的AB包资源信息(包名+包名对应下的所有资源)
    /// </summary>
    public static List<AssetBundleBuild> assetBundleBuildList = new List<AssetBundleBuild>();
    /// <summary>
    /// AB包文件的输出路径
    /// </summary>
    public static string abOutputPath = Application.streamingAssetsPath;
    /// <summary>
    /// 记录哪个asset资源属于哪个AB包文件
    /// key:一个资源的相对路径(以Asset开头):Assets/GAssets/Launch/New Flare.flare   |   value:AB包包名:gassets_launch
    /// </summary>
    public static Dictionary<string, string> asset2bundle = new Dictionary<string, string>();
    /// <summary>
    /// 记录每个asset资源依赖的AB包文件列表
    /// key:一个资源的相对路径     |   value:依赖关系(AB包包名列表)
    /// </summary>
    public static Dictionary<string, List<string>> asset2Dependencies = new Dictionary<string, List<string>>();
    /// <summary>
    /// 临时存放需要加密的明文文件的根路径
    /// </summary>
    public static string tempGAssets;

    /// <summary>
    /// 打正式打大版本的版本资源(HotUpdate = false && BundleMode = true)
    /// </summary>
    [MenuItem("ABEditor/BuildAssetBundle_Base")]
    public static void BuildAssetBundle_Base()
    {
        //打大版本包时资源存储路径
        abOutputPath = Application.dataPath + "/../AssetBundle_Base";
        BuildAssetBundle();
    }

    /// <summary>
    /// 正式打热更版本包(HotUpdate = true && BundleMode = true)
    /// </summary>
    [MenuItem("ABEditor/BuildAssetBundle_Update")]
    public static void BuildAssetBundle_Update()
    {
        /*
         先把所有的bundle资源都生成到跟Assets平级的AssetBundle_Update文件夹,然后再跟AssetBundle_Base文件夹的bundle资源做比对
         把那些在AssetBundle_Base文件夹中已经存在的,并且内容完全相同的那些bundle文件从AssetBundle_Update文件夹中删除掉
         那么AssetBundle_Update文件夹中剩下的bundle文件就是这个热更版本包！
         另外,这个热更版本包的AB配置文件也要同时处理一下,也就是说只包含AssetBundle_Update文件夹中的bundle文件！
         */
        //打热更包时资源存储路径
        abOutputPath = Application.dataPath + "/../AssetBundle_Update";
        BuildAssetBundle();
        string baseABPath = Application.dataPath + "/../AssetBundle_Base";
        string updateABPath = abOutputPath;
        //AssetBundle_Base版本的资源
        DirectoryInfo baseDir = new DirectoryInfo(baseABPath);
        DirectoryInfo[] dirs = baseDir.GetDirectories();
        //遍历AssetBundle_Base下的所有模块
        foreach (DirectoryInfo moduleDir in dirs)
        {
            string moduleName = moduleDir.Name;
            ModuleABConfig baseABConfig = LoadABConfig(baseABPath + "/" + moduleName + "/" + moduleName.ToLower() + ".json");
            ModuleABConfig updateABConfig = LoadABConfig(updateABPath + "/" + moduleName + "/" + moduleName.ToLower() + ".json");
            //对比base版本,没有变化的bundle文件需要从热更包中删除
            List<BundleInfo> removeList = Calculate(baseABConfig, updateABConfig);
            foreach (BundleInfo bundleInfo in removeList)
            {
                string filePath = updateABPath + "/" + moduleName + "/" + bundleInfo.bundle_name;
                File.Delete(filePath);
                //同时要处理一下热更包的AB资源配置文件
                updateABConfig.BundleDict.Remove(bundleInfo.bundle_name);
            }
            string jsonPath = updateABPath + "/" + moduleName + "/" + moduleName.ToLower() + ".json";
            if (File.Exists(jsonPath) == true)
            {
                File.Delete(jsonPath);
            }
            File.Create(jsonPath).Dispose();
            string jsonData = JsonMapper.ToJson(updateABConfig);
            File.WriteAllText(jsonPath, ConvertJsonString(jsonData));
        }
    }

    /// <summary>
    /// 开发阶段打AB包(HotUpdate = false && BundleMode = false)
    /// </summary>
    [MenuItem("ABEditor/BuildAssetBundle_Dev")]
    public static void BuildAssetBundle_Dev()
    {
        abOutputPath = Application.streamingAssetsPath;
        BuildAssetBundle();
    }

    /// <summary>
    /// 打包AssetBundle资源
    /// </summary>
    static void BuildAssetBundle()
    {
        Debug.Log("开始--->>>对所有模块的Lua和Proto明文文件进行加密");
        EncyptLuaAndProto();

        Debug.Log("开始--->>>生成所有模块的AB包!");
        try
        {
            //每次打包时,都先将资源清理一边
            if (Directory.Exists(abOutputPath) == true)
            {
                Directory.Delete(abOutputPath, true);
            }

            //根目录的一级子文件夹
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);
            DirectoryInfo[] dirs = rootDir.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                string moduleName = dirs[i].Name;

                assetBundleBuildList.Clear();
                asset2bundle.Clear();
                asset2Dependencies.Clear();

                //遍历根目录下的所有一级子文件夹,针对没有子文件夹分别打包
                ScanChildDireations(dirs[i]);

                AssetDatabase.Refresh();

                string moduleOutputPath = abOutputPath + "/" + moduleName;
                if (Directory.Exists(moduleOutputPath) == true)
                {
                    Directory.Delete(moduleOutputPath, true);
                }
                Directory.CreateDirectory(moduleOutputPath);
                //压缩选项:
                /*1.BuildAssetBundleOptions.None:(一般选择使用这种模式)使用LZMA算法压缩,压缩的包更小,但是加载时间更长
                    使用之前需要整体解压.一旦被解压,这个包会使用LZ4重新压缩.使用资源的时候不需要整体解压
                    在下载的时候可以使用LZMA算法,一旦它被下载了之后,它会使用LZ4算法保存到本地上。
                  2.BuildAssetBundleOptions.UncompressedAssetBundle:不压缩,包大,加载快
                  3.BuildAssetBundleOptions.ChunkBasedCompression:使用LZ4压缩,压缩率没有LZMA高,但是我们可以加载指定资源而不用解压全部
                */
                BuildPipeline.BuildAssetBundles(moduleOutputPath, assetBundleBuildList.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression,
                    EditorUserBuildSettings.activeBuildTarget);

                //资源依赖
                CalculateDependencies();
                //将资源依赖保存到本地(json格式)
                SaveModuleABConfig(moduleName);
                AssetDatabase.Refresh();
                //删除给模块打AB包完成时生成的manifest文件
                DeleteManifest(moduleOutputPath);
                File.Delete(moduleOutputPath + "/" + moduleName);
            }
        }
        finally
        {
            Debug.Log("结束--->>>生成所有模块的AB包!");

            Debug.Log("开始--->>>对所有模块的Lua和Proto明文文件进行恢复");
            RestoreModules();
        }
    }

    /// <summary>
    /// 根据指定的文件夹
    /// 1.将这个文件夹下的所有一级子文件打成一个AssetBundle
    /// 2.并且递归遍历这个文件夹的所有子文件夹
    /// </summary>
    /// <param name="directoryInfo"></param>
    public static void ScanChildDireations(DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Name.EndsWith("CSProject~"))
        {
            return;
        }

        //收集当前路径下的文件,把他们打成一个AB包
        ScanCurrDirectory(directoryInfo);

        //递归遍历当前文件夹下的子文件夹
        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
        for (int i = 0; i < dirs.Length; i++)
        {
            ScanChildDireations(dirs[i]);
        }
    }

    /// <summary>
    /// 遍历当前路径下的文件,把他们打成一个AB包
    /// </summary>
    /// <param name="directoryInfo"></param>
    private static void ScanCurrDirectory(DirectoryInfo directoryInfo)
    {
        List<string> assetNameList = new List<string>();
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            //忽略mate文件
            if (fileInfos[i].FullName.EndsWith(".meta"))
            {
                continue;
            }
            //截取文件的全路径得到以Assets为根目录的路径: "Assets/GAssets/Launch/Sphere.prefab"
            string assetName = fileInfos[i].FullName.Substring(Application.dataPath.Length - "Assets".Length).Replace('\\', '/');
            assetNameList.Add(assetName);
        }

        if (assetNameList.Count > 0)
        {
            //AB包包名格式:gassets_launch
            string assetBundleName = directoryInfo.FullName.Substring(Application.dataPath.Length + 1).Replace('\\', '_').ToLower();
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = assetBundleName;
            build.assetNames = new string[assetNameList.Count];
            for (int i = 0; i < assetNameList.Count; i++)
            {
                build.assetNames[i] = assetNameList[i];
                //记录单个资源属于哪个bundle文件
                asset2bundle.Add(assetNameList[i], assetBundleName);
            }

            assetBundleBuildList.Add(build);
        }
    }

    /// <summary>
    /// 计算每个资源所依赖的ab包文件列表
    /// </summary>
    static void CalculateDependencies()
    {
        foreach (string asset in asset2bundle.Keys)
        {
            //资源所在的bundle
            string assetBundle = asset2bundle[asset];
            //获取资源asset的资源包列表(包含自己所在的资源包)
            string[] dependencies = AssetDatabase.GetDependencies(asset);
            List<string> assetList = new List<string>();
            if (dependencies != null && dependencies.Length > 0)
            {
                foreach (string oneAsset in dependencies)
                {
                    if (oneAsset == asset || oneAsset.EndsWith(".cs"))
                    {
                        continue;
                    }
                    assetList.Add(oneAsset);
                }
            }
            if (assetList.Count > 0)
            {
                List<string> abList = new List<string>();
                foreach (string oneAsset in assetList)
                {
                    if (asset2bundle.TryGetValue(oneAsset, out string bundle) == true)
                    {
                        //排除资源自己所属的资源包
                        if (bundle != assetBundle)
                        {
                            //排除重复添加包名
                            if (abList.Contains(bundle) == false)
                                abList.Add(bundle);
                        }
                    }
                }
                asset2Dependencies.Add(asset, abList);
            }
        }
    }

    /// <summary>
    /// 将一个模块(包)的资源依赖关系数据保存到本地(json格式)
    /// </summary>
    /// <param name="moduleName"></param>
    static void SaveModuleABConfig(string moduleName)
    {
        //处理AB包
        ModuleABConfig moduleABConfig = new ModuleABConfig(asset2bundle.Count);
        foreach (AssetBundleBuild build in assetBundleBuildList)
        {
            BundleInfo bundleInfo = new BundleInfo();
            bundleInfo.bundle_name = build.assetBundleName;
            bundleInfo.assets = new List<string>();
            foreach (string asset in build.assetNames)
            {
                bundleInfo.assets.Add(asset);
            }
            //bundle文件的CRC散列码
            string abFilePath = abOutputPath + "/" + moduleName + "/" + bundleInfo.bundle_name;
            using (FileStream stream = File.OpenRead(abFilePath))
            {
                bundleInfo.crc = AssetUtility.GetCRC32Hash(stream);
                //包大小
                bundleInfo.size = (int)stream.Length;
            }
            moduleABConfig.AddBundle(bundleInfo.bundle_name, bundleInfo);
        }
        //处理要打包的资源列表
        int assetIndex = 0;
        foreach (KeyValuePair<string, string> item in asset2bundle)
        {
            AssetInfo assetInfo = new AssetInfo();
            assetInfo.asset_path = item.Key;
            assetInfo.bundle_name = item.Value;
            assetInfo.dependencies = new List<string>();
            if (asset2Dependencies.TryGetValue(item.Key, out List<string> dependencies) == true)
            {
                for (int i = 0; i < dependencies.Count; i++)
                {
                    string bundleName = dependencies[i];
                    assetInfo.dependencies.Add(bundleName);
                }
            }
            moduleABConfig.AddAsset(assetIndex, assetInfo);
            assetIndex++;
        }
        //开始写入json文件
        string moduleConfigName = moduleName.ToLower() + ".json";
        string jsonPath = abOutputPath + "/" + moduleName + "/" + moduleConfigName;
        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
        }
        File.Create(jsonPath).Dispose();
        string jsonData = LitJson.JsonMapper.ToJson(moduleABConfig);
        File.WriteAllText(jsonPath, ConvertJsonString(jsonData));
    }

    /// <summary>
    /// 格式化json
    /// </summary>
    /// <param name="str">输入json字符串</param>
    /// <returns>返回格式化后的字符串</returns>
    static string ConvertJsonString(string str)
    {
        JsonSerializer serializer = new JsonSerializer();
        TextReader tr = new StringReader(str);
        JsonTextReader jtr = new JsonTextReader(tr);
        object obj = serializer.Deserialize(jtr);
        if (obj != null)
        {
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' ',
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }
        else
        {
            return str;
        }
    }


    /// <summary>
    /// 计算热更包中需要删除的bundle文件列表
    /// </summary>
    /// <param name="baseABConfig"></param>
    /// <param name="updateABPath"></param>
    /// <returns></returns>
    private static List<BundleInfo> Calculate(ModuleABConfig baseABConfig, ModuleABConfig updateABConfig)
    {
        //base版本的所有bundle列表
        Dictionary<string, BundleInfo> baseBundleDict = new Dictionary<string, BundleInfo>();
        if (baseABConfig != null)
        {
            foreach (BundleInfo bundleInfo in baseABConfig.BundleDict.Values)
            {
                string uniqueId = string.Format("{0}|{1}", bundleInfo.bundle_name, bundleInfo.crc);
                baseBundleDict.Add(uniqueId, bundleInfo);
            }
        }
        //遍历Update版本中的bundle文件，把那些需要删除的bundle放入下面的removeList容器中
        List<BundleInfo> removeList = new List<BundleInfo>();
        foreach (BundleInfo bundleInfo in updateABConfig.BundleDict.Values)
        {
            string uniqueId = string.Format("{0}|{1}", bundleInfo.bundle_name, bundleInfo.crc);
            if (baseBundleDict.ContainsKey(uniqueId) == true)
            {
                removeList.Add(bundleInfo);
            }
        }
        return removeList;
    }

    /// <summary>
    /// 加载指定路径的AB资源配置文件
    /// </summary>
    /// <param name="abConfigPath"></param>
    /// <returns></returns>
    private static ModuleABConfig LoadABConfig(string abConfigPath)
    {
        File.ReadAllText(abConfigPath);
        return JsonMapper.ToObject<ModuleABConfig>(File.ReadAllText(abConfigPath));
    }

    /// <summary>
    /// 删除给模块打AB包完成时生成的manifest文件
    /// </summary>
    /// <param name="moduleOutputPath">模块对应的ab文件输出路径</param>
    private static void DeleteManifest(string moduleOutputPath)
    {
        FileInfo[] files = new DirectoryInfo(moduleOutputPath).GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Name.EndsWith(".manifest"))
            {
                file.Delete();
            }
        }
    }

    /// <summary>
    /// 对每个模块中的Src/文件夹和Res/Proto/文件夹进行加密
    /// </summary>
    private static void EncyptLuaAndProto()
    {
        //遍历所有模块(GAssets下的所有文件夹Launch等),针对所有模块都进行lua和proto的明文加密
        DirectoryInfo rootDir = new DirectoryInfo(rootPath);
        DirectoryInfo[] dirs = rootDir.GetDirectories();
        //创建临时文件夹，用来存临时存放每个模块需要加密的明文文件
        CreateTempGAssets();
        //遍历Launch文件夹下的所有模块(Src/Res等)
        foreach (DirectoryInfo moduleDir in dirs)
        {
            //1.把Src/文件夹和Res/Proto/文件夹都复制到临时文件夹
            CopyOneModule(moduleDir);
            //2.把Src/文件夹和Res/Proto/文件夹进行就地加密
            EncryptOneModule(moduleDir);
            //3.把Src/文件夹和Res/Proto/文件夹的明文文件就地删除
            DeleteOneModule(moduleDir);
        }
        //加密完成后刷新下
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 创建临时文件夹，用来存临时存放每个模块需要加密的明文文件
    /// </summary>
    private static void CreateTempGAssets()
    {
        tempGAssets = Application.dataPath + "/../TempGAssets";
        if (Directory.Exists(tempGAssets) == true)
        {
            Directory.Delete(tempGAssets, true);
        }
        Directory.CreateDirectory(tempGAssets);
    }

    /// <summary>
    /// 把Src/文件夹和Res/Proto/文件夹都复制到临时文件夹
    /// </summary>
    /// <param name="moduleDir">模块的路径</param>
    private static void CopyOneModule(DirectoryInfo moduleDir)
    {
        //lua源文件路径
        string srcLuaPath = Path.Combine(moduleDir.FullName, "Src");
        //复制lua文件目标路径
        string destLuaPath = Path.Combine(tempGAssets, moduleDir.Name, "Src");
        CopyFolder(srcLuaPath, destLuaPath);

        //proto源文件路径
        string srcProtoPaht = Path.Combine(moduleDir.FullName, "Res/Proto");
        //复制proto文件目标路径
        string destProtoPath = Path.Combine(tempGAssets, moduleDir.Name, "Res/Proto");
        CopyFolder(srcProtoPaht, destProtoPath);
    }

    /// <summary>
    /// 对单个模块的Src/文件夹和Res/Proto/文件夹下的明文文件进行就地加密
    /// </summary>
    /// <param name="moduleDir"></param>
    private static void EncryptOneModule(DirectoryInfo moduleDir)
    {
        EncryptOnePath(Path.Combine(moduleDir.FullName, "Src"));
        EncryptOnePath(Path.Combine(moduleDir.FullName, "Res/Proto"));
    }

    /// <summary>
    /// 对单个路径下的所有资源进行加密，并生成对应的加密文件
    /// </summary>
    /// <param name="path"></param>
    private static void EncryptOnePath(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] fileInfoArray = directoryInfo.GetFiles();
        //遍历子文件
        foreach (FileInfo fileInfo in fileInfoArray)
        {
            //只对lua文件和proto文件需要加密
            if (fileInfo.FullName.EndsWith(".lua") == false && fileInfo.FullName.EndsWith(".proto") == false)
            {
                continue;
            }
            //读取明文数据
            string plainText = File.ReadAllText(fileInfo.FullName);
            //进行AES加密
            string cipherText = AESHelper.Encrypt(plainText, AESHelper.keyValue);
            //创建加密后的文件
            CreateEncryptFile(fileInfo.FullName + ".bytes", cipherText);
        }
        //递归遍历子文件夹
        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
        foreach (DirectoryInfo oneDirInfo in dirs)
        {
            EncryptOnePath(oneDirInfo.FullName);
        }
    }

    /// <summary>
    /// 把Src/文件夹和Res/Proto/文件夹的明文文件就地删除
    /// </summary>
    /// <param name="moduleDir"></param>
    private static void DeleteOneModule(DirectoryInfo moduleDir)
    {
        DeleteOnePath(Path.Combine(moduleDir.FullName, "Src"));
        DeleteOnePath(Path.Combine(moduleDir.FullName, "Res/Proto"));
    }

    /// <summary>
    /// 对单个路径下的lua或者proto明文文件进行删除
    /// </summary>
    /// <param name="path"></param>
    private static void DeleteOnePath(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] fileInfoArray = directoryInfo.GetFiles();
        //遍历子文件
        foreach (FileInfo fileInfo in fileInfoArray)
        {
            //只对lua文件和proto文件进行删除
            if (fileInfo.FullName.EndsWith(".lua") == false && fileInfo.FullName.EndsWith(".lua.meta") == false &&
                fileInfo.FullName.EndsWith(".proto") == false && fileInfo.FullName.EndsWith(".proto.meta") == false)
            {
                continue;
            }
            //删除明文文件和其对应的meta文件
            fileInfo.Delete();
        }
        //递归遍历子文件夹
        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
        foreach (DirectoryInfo oneDirInfo in dirs)
        {
            DeleteOnePath(oneDirInfo.FullName);
        }
    }

    /// <summary>
    /// 重置AB包中lua和proto文件
    /// <para>1.删除GAssets的各模块中的加密文件</para>
    /// <para>2.把存放再临时文件夹的明文文件再拷贝回GAssets的各个模块中</para>
    /// <para>3.删除临时文件夹</para>
    /// </summary>
    private static void RestoreModules()
    {
        DirectoryInfo rootDir = new DirectoryInfo(rootPath);
        DirectoryInfo[] Dirs = rootDir.GetDirectories();
        foreach (DirectoryInfo moduleDir in Dirs)
        {
            //处理Lua文件夹
            string luaPath = Path.Combine(moduleDir.FullName, "Src");
            Directory.Delete(luaPath, true);
            string tempLuaPath = Path.Combine(tempGAssets, moduleDir.Name, "Src");
            CopyFolder(tempLuaPath, luaPath);

            //处理Proto文件夹
            string protoPath = Path.Combine(moduleDir.FullName, "Res/Proto");
            Directory.Delete(protoPath, true);
            string tempProtoPath = Path.Combine(tempGAssets, moduleDir.Name, "Res/Proto");
            CopyFolder(tempProtoPath, protoPath);
        }
        //删除临时文件夹
        Directory.Delete(tempGAssets, true);
    }

    /// <summary>
    /// 创建加密后的文件
    /// </summary>
    /// <param name="filePath">密文文件的路径</param>
    /// <param name="fileText">密文的内容</param>
    private static void CreateEncryptFile(string filePath, string fileText)
    {
        FileStream fs = new FileStream(filePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(fileText);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="sourceFolder">原文件夹路径</param>
    /// <param name="destFloder">目标文件夹路径</param>
    private static void CopyFolder(string sourceFolder, string destFloder)
    {
        try
        {
            if (Directory.Exists(destFloder) == true)
            {
                Directory.Delete(destFloder, true);
            }
            Directory.CreateDirectory(destFloder);
            //源文件夹下的子文件列表
            string[] filePathList = Directory.GetFiles(sourceFolder);
            foreach (string filePath in filePathList)
            {
                string fileName = Path.GetFileName(filePath);
                string destPath = Path.Combine(destFloder, fileName);
                File.Copy(filePath, destPath);
            }
            //源文件夹下的子文件夹列表
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string srcPath in folders)
            {
                string folderName = Path.GetFileName(srcPath);
                string destPath = Path.Combine(destFloder, folderName);
                //构建目标路径,递归复制文件
                CopyFolder(srcPath, destPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("复制文件夹出错: " + e.ToString());
        }
    }
}