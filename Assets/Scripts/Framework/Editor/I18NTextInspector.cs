using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(I18NText), false)]
public class I18NTextInspector : Editor
{
    SerializedProperty m_i18NId;
    Text m_self;

    private void OnEnable()
    {
        I18N.Instance.Init();
        m_self = target as Text;
        m_i18NId = serializedObject.FindProperty("i18NId");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        string str = I18N.Instance.GetString(m_i18NId.intValue);
        EditorGUILayout.LabelField("I18N文本:");
        if (m_i18NId.intValue != -1)
            m_self.text = str != null ? str : "";
        EditorGUILayout.TextArea(str != null ? str : "");

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打开I18N配置"))
        {
            FastOpenTools.OpenI18NCfg();
        }
        if (GUILayout.Button("重新加载 I18N配置"))
        {
            I18N.Instance.Reload();
        }
        GUILayout.EndHorizontal();
    }
}