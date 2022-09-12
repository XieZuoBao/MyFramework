using System.Data.Common;
using UnityEngine;

/// <summary>
/// 多语言类型
/// </summary>
public enum LanguageType
{
    // 中文简体
    ZH_CN = 0,
    // 中文繁体
    ZH_TW,
    // 英语
    English,
}

/// <summary>
/// 多语言管理器
/// </summary>
public class LanguageMgr : BaseSingleton<LanguageMgr>
{
    public LanguageType language = LanguageType.ZH_CN;
    public int languageIndex
    {
        get
        {
            return (int)language;
        }
    }

    public void Init()
    {
        int languangeIndex = int.Parse(Cache.Get(GlobalsDefine.LANGUAGE_TYPE, "0"));
        language = (LanguageType)languageIndex;
    }

    /// <summary>
    /// 切换语言类型
    /// </summary>
    /// <param name="index"></param>
    public void ChangeLanguageType(int index)
    {
        language = (LanguageType)index;
        Cache.Set(GlobalsDefine.LANGUAGE_TYPE, index.ToString());
        EventCenter.Instance.EventTrigger(EventNameDef.LANGUAGE_TYPE_CHANGED);
    }
}