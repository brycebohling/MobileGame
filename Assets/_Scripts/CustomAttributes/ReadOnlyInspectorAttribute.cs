using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyInspector : PropertyAttribute
{
    
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyInspector))]
public class ReadOnlyInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}
#endif