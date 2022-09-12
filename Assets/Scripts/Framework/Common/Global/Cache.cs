using System.Collections;
using UnityEngine;
using LitJson;

/// <summary>
/// 本地缓存数据
/// </summary>
public class Cache
{
    /// <summary>
    /// 封装PlayerPrefs.GetString
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string Get(string cacheKey, string defaultValue)
    {
        return PlayerPrefs.GetString(cacheKey, defaultValue);
    }

    /// <summary>
    /// 封装PlayerPrefs.SetString
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="value"></param>
    public static void Set(string cacheKey, string value)
    {
        PlayerPrefs.SetString(cacheKey, value);
    }

    /// <summary>
    /// 获取本地Json格式的数据
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="jsonKey"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string JsonGet(string cacheKey, string jsonKey, string defaultValue)
    {
        string old = Get(cacheKey, "[]");

        if (old.Length > 0 && !old.StartsWith("["))
        {
            JsonClear(cacheKey);
            return defaultValue;
        }

        JsonData jsonData = JsonMapper.ToObject(old);
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (((IDictionary)jsonData[i]).Contains(jsonKey))
            {
                return jsonData[i][jsonKey].ToString();
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// 设置本地Json格式的数据
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="jsonKey"></param>
    /// <param name="value"></param>
    public static void JsonSet(string cacheKey, string jsonKey, string value)
    {
        string old = Get(cacheKey, "[]");

        if (old.Length > 0 && !old.StartsWith("["))
        {
            old = "[]";
        }

        JsonData jsonData = JsonMapper.ToObject(old);
        bool keyInJson = false;
        for (int i = 0; i < jsonData.Count; i++)
        {
            if (((IDictionary)jsonData[i]).Contains(jsonKey))
            {
                jsonData[cacheKey][jsonKey] = value;
                keyInJson = true;
                break;
            }
        }

        if (!keyInJson)
        {
            JsonData newData = new JsonData();
            newData[jsonKey] = value;
            jsonData.Add(newData);
        }
        Set(cacheKey, jsonData.ToJson());
    }

    /// <summary>
    /// 清理本地Json格式的单个数据
    /// </summary>
    /// <param name="cacheKey"></param>
    public static void JsonClear(string cacheKey)
    {
        Set(cacheKey, "[]");
    }
}
