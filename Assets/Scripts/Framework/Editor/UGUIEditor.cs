using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// UGUI常用组件的拓展
/// </summary>
public class UGUIEditor
{
    [MenuItem("GameObject/UI/I18NText (多语言文本)", false)]
    static void CreateI18NText()
    {
        if (!CanCreate())
            return;

        I18N.Instance.Init();
        GameObject go = new GameObject("I18NText");
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 30);
        go.transform.SetParent(Selection.activeTransform, false);
        I18NText text = go.AddComponent<I18NText>();
        text.color = new Color(50.0f / 255, 50.0f / 255, 50.0f / 255, 1.0f);
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.raycastTarget = false;
        text.supportRichText = false;
        text.resizeTextForBestFit = false;
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    [MenuItem("GameObject/UI/Text", false)]
    static void CreateText()
    {
        if (!CanCreate())
            return;

        GameObject go = new GameObject("Text");
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 30);
        go.transform.SetParent(Selection.activeTransform, false);
        Text text = go.AddComponent<Text>();
        text.text = "Text";
        text.color = new Color(50.0f / 255, 50.0f / 255, 50.0f / 255, 1.0f);
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.raycastTarget = false;
        text.supportRichText = false;
        text.resizeTextForBestFit = false;
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    [MenuItem("GameObject/UI/Image", false)]
    static void CreateImage()
    {
        if (!CanCreate())
            return;

        GameObject go = new GameObject("Image");
        RectTransform rect = go.AddComponent<RectTransform>();
        go.transform.SetParent(Selection.activeTransform, false);
        Image img = go.AddComponent<Image>();
        img.raycastTarget = false;
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    [MenuItem("GameObject/UI/RawImage", false)]
    static void CreateRawImage()
    {
        if (!CanCreate())
            return;

        GameObject go = new GameObject("RawImage");
        RectTransform rect = go.AddComponent<RectTransform>();
        go.transform.SetParent(Selection.activeTransform, false);
        RawImage img = go.AddComponent<RawImage>();
        img.raycastTarget = false;
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    [MenuItem("GameObject/UI/Button", false)]
    static void CreateButton()
    {
        if (!CanCreate())
            return;

        GameObject go = new GameObject("Button");
        RectTransform rect = go.AddComponent<RectTransform>();
        go.transform.SetParent(Selection.activeTransform, false);
        go.AddComponent<Image>();
        go.AddComponent<Button>();
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    [MenuItem("GameObject/UI/NonRectButton", false)]
    static void CreateNonRectButton()
    {
        if (!CanCreate())
            return;

        GameObject go = new GameObject("NonRectButton");
        RectTransform rect = go.AddComponent<RectTransform>();
        go.transform.SetParent(Selection.activeTransform, false);
        go.AddComponent<NonRectButtonImage>();
        go.AddComponent<Button>();
        go.AddComponent<PolygonCollider2D>();
        go.layer = LayerMask.NameToLayer("UI");
        Selection.SetActiveObjectWithContext(go, null);
    }

    static bool CanCreate()
    {
        if (!Selection.activeTransform || !Selection.activeTransform.GetComponentInParent<Canvas>())
        {
            GameLogger.LogError("请在Hierarchy视图中根节点为Canvas上右键鼠标");
            return false;
        }
        return true;
    }
}