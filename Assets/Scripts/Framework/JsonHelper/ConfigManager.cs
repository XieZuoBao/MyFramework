using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ConfigManager : IConfigManager
{
    private Dictionary<string, string> appSetting;
    public Dictionary<string, string> AppSetting
    {
        get { return appSetting; }
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jsonPath">json配置文件路径</param>
    public ConfigManager(string jsonPath)
    {
        appSetting = new Dictionary<string, string>();
        ParseJson(jsonPath);
    }

    public int GetAppSettingCount()
    {
        if (appSetting == null)
            return 0;
        return appSetting.Count;
    }

    /// <summary>
    /// 解析Json配置文件
    /// </summary>
    /// <param name="jsonPath"></param>
    void ParseJson(string jsonPath)
    {
        TextAsset configInfo;
        KeyValueInfoList list;

        if (string.IsNullOrEmpty(jsonPath))
            return;

        try
        {
            configInfo = AssetLoader.Instance.CreateAsset<TextAsset>("", "", null);
            list = JsonMapper.ToObject<KeyValueInfoList>(configInfo.text);
            for (int i = 0; i < list.JsonConfig.Count; i++)
            {
                appSetting.Add(list.JsonConfig[i].Key, list.JsonConfig[i].Value);
            }
        }
        catch
        {
            throw new JsonParseException(GetType() + ".ParseJson()/Json Parse Exception!!!jsonPath=" + jsonPath);
        }
    }
}
