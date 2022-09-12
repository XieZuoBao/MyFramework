/// <summary>
/// 全局配置
/// 1.开发阶段使用:HotUpdate = false && BundleMode = false(编辑器使用:既不热更,也不使用ab包方式加载)
/// 2.正式热更包打包:HotUpdate = true && BundleMode = true
/// 3.大版本整包打包:HotUpdate = false && BundleMode = true
/// 4.错误模式:HotUpdate = true && BundleMode = false(不要这样配置)
/// </summary>
public static class GlobalConfig
{
    /// <summary>
    /// 是否开启热更
    /// </summary>
    public static bool HotUpdate;
    /// <summary>
    /// 是否采用Bundle方式加载
    /// <para>编辑器false;真机true</para>
    /// </summary>
    public static bool BundleMode;
    /// <summary>
    /// 全局配置的构造函数
    /// </summary>
    static GlobalConfig()
    {
        HotUpdate = false;
        BundleMode = false;
    }
}

/// <summary>
/// 单个模块的配置对象
/// </summary>
public class ModuleConfig
{
    /// <summary>
    /// 模块的名字
    /// </summary>
    public string moduleName;
    /// <summary>
    /// 模块的版本号
    /// </summary>
    public string moduleVersion;
    /// <summary>
    /// 模块的热更服务器地址
    /// </summary>
    public string moduleUrl;
    /// <summary>
    /// 模块资源在远程服务器上的基础地址(服务器地址+模块名+版本号)
    /// </summary>
    public string DownloadURL
    {
        get
        {
            return moduleUrl + "/" + moduleName + "/" + moduleVersion;
        }
    }
}

/// <summary>
/// 选择 原始只读路径 还是 可读可写路径
/// </summary>
public enum BaseOrUpdate
{
    /// <summary>
    /// 本地资源只读路径(Application.streamingAssetsPath)
    /// </summary>
    Base,
    /// <summary>
    /// 热更资源用的可读可写路径(Application.persistentDataPath)
    /// </summary>
    Update,
}
