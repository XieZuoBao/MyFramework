using System;
using System.Collections.Generic;

/// <summary>
/// 解析"键值对"形式的json配置文件的接口
/// </summary>
public interface IConfigManager
{
    /// <summary>
    /// "键值对"json配置文件中存储的数据(只读)
    /// </summary>
    Dictionary<string, string> AppSetting { get; }
    /// <summary>
    /// 数据数量
    /// </summary>
    /// <returns></returns>
    int GetAppSettingCount();
}

/// <summary>
/// 整个配置文件对应的的信息
/// </summary>
[Serializable]
public class KeyValueInfoList
{
    public List<KeyValueInfo> JsonConfig;
}

/// <summary>
/// 键值对(配置文件中的一条数据)
/// </summary>
[Serializable]
public class KeyValueInfo
{
    //键
    public string Key;
    //值
    public string Value;
}
