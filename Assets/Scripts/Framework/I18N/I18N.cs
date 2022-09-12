using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;

/// <summary>
/// I18国际化文字
/// </summary>
public class I18N : MonoSingleton<I18N>
{
    /// <summary>
    /// 键:翻译id;值:多语言翻译内容
    /// </summary>
    private Dictionary<int, I18NCfgItem> m_i18nCfg = new Dictionary<int, I18NCfgItem>();
    /// <summary>
    /// 是否已经初始化
    /// </summary>
    private bool m_isInited = false;

    public void Init()
    {
        if (m_isInited)
            return;
        m_i18nCfg.Clear();

        try
        {
            TextAsset textAsset = AssetLoader.Instance.CreateAsset<TextAsset>("Launch", GlobalsDefine.I18N_APP_STRINGS_PATH, gameObject);
            JsonWrapper<I18NCfgItem> jsonObj = JsonMapper.ToObject<JsonWrapper<I18NCfgItem>>(textAsset.text);
            for (int i = 0; i < jsonObj.JsonConfig.Length; i++)
            {
                I18NCfgItem item = new I18NCfgItem();
                item.id = jsonObj.JsonConfig[i].id;
                item.zhCn = jsonObj.JsonConfig[i].zhCn;
                item.zhTw = jsonObj.JsonConfig[i].zhTw;
                item.english = jsonObj.JsonConfig[i].english;
                m_i18nCfg.Add(item.id, item);
            }
            m_isInited = true;
        }
        catch
        {
            throw new JsonParseException(GetType() + ".Init()/Json Parse Exception!!!jsonPath=" + GlobalsDefine.I18N_APP_STRINGS_PATH);
        }
    }

    public void Reload()
    {
        m_isInited = false;
        Init();
    }

    public string GetString(int id)
    {
        if (m_i18nCfg.ContainsKey(id))
        {
            if (!Application.isPlaying)
            {
                return m_i18nCfg[id].zhCn;
            }
            //根据语言设置，返回对应的语言文本
            switch (LanguageMgr.Instance.language)
            {
                case LanguageType.ZH_CN:
                    return m_i18nCfg[id].zhCn;
                case LanguageType.ZH_TW:
                    return m_i18nCfg[id].zhTw;
                case LanguageType.English:
                    return m_i18nCfg[id].english;
                default:
                    return m_i18nCfg[id].zhCn;
            }
        }
        else
            return string.Empty;
    }

    public string GetSeparateResString(int id1, int id2, char sp1 = ';', char sp2 = ':', bool default_output = false)
    {
        string str = GetString(id1);
        if (string.IsNullOrEmpty(str))
            return string.Empty;
        string[] strArray = str.Split(sp1);
        for (int i = 0; i < strArray.Length; i++)
        {
            string[] subStrArray = strArray[i].Trim().Split(sp2);
            if (subStrArray.Length.Equals(2))
            {
                int first = int.Parse(subStrArray[0].Trim());
                if (first.Equals(id2))
                {
                    return subStrArray[1].Trim();
                }
            }
        }
        if (default_output)
            return string.Format(I18N.Instance.GetResString(14701), id2);
        return str;
    }

    public string GetResString(int key, params object[] args)
    {
        return string.Format(GetResString(key), args);
    }

    public string GetResString(bool[] indices, params object[] values)
    {
        if (indices.Length != values.Length)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < indices.Length; i++)
        {
            sb.Append(indices[i] ? GetResString((int)(values[i])) : values[i]);
        }
        return sb.ToString();
    }
}