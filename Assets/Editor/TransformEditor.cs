using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 拓展Transform类显示世界坐标
/// </summary>
[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    Transform transform;

    private void OnEnable()
    {
        transform = (Transform)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        transform.localPosition = EditorGUILayout.Vector3Field("LocalPosition", transform.localPosition);
        transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", transform.localEulerAngles);
        transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
        transform.position = EditorGUILayout.Vector3Field("WorldPosition", transform.position);
        EditorGUILayout.EndVertical();
    }
}