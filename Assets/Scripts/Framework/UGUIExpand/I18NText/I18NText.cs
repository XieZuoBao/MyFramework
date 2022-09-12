using UnityEngine.UI;

/// <summary>
/// 拓展Text实现多语言效果
/// </summary>
public class I18NText : Text
{
    public int i18NId = -1;

    protected override void OnEnable()
    {
        base.OnEnable();

        Refresh();
        EventCenter.Instance.AddEventListener(EventNameDef.LANGUAGE_TYPE_CHANGED, OnLanguageChange);
    }

    void OnLanguageChange()
    {
        Refresh();
    }

    /// <summary>
    /// 根据当前的语言设置,比如中文,英语等,显示对应的语言的文本
    /// </summary>
    public void Refresh()
    {
        if (i18NId != -1)
        {
            text = I18N.Instance.GetString(i18NId);
        }
    }

    protected override void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener(EventNameDef.LANGUAGE_TYPE_CHANGED, OnLanguageChange);
        base.OnDestroy();
    }
}