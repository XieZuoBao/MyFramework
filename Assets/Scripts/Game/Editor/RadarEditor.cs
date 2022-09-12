using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

/// <summary>
/// 雷达图属性可视面板拓展
/// </summary>
[CustomEditor(typeof(Radar), true)]
public class RadarEditor : GraphicEditor
{
    SerializedProperty m_Radius;
    SerializedProperty m_Values;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_Radius = serializedObject.FindProperty("radius");
        m_Values = serializedObject.FindProperty("values");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //serializedObject.Update();
        EditorGUILayout.PropertyField(m_Radius);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Values);
        if (EditorGUI.EndChangeCheck())
        {
            if (m_Values.arraySize <= 3)
            {
                m_Values.arraySize = 3;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}