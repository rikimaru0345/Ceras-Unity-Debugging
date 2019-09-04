#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Glue
{
    [CustomEditor(typeof(Scenager))]
    public class CustomListEditor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("key"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("root"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideGlueInPlayMode"), true);
            ArrayGUI(serializedObject.FindProperty("scenes"));
            serializedObject.ApplyModifiedProperties();

            Scenager script = (Scenager)target;

            if (EditorGUI.EndChangeCheck())
            {
                script.Check();
            }
        }
        private void ArrayGUI(SerializedProperty property)
        {
            EditorGUI.indentLevel++;
            SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("Children"));

            for (int i = 0; i < arraySizeProp.intValue; i++)
            {
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent("Scene " + i.ToString()), true);
            }
            EditorGUI.indentLevel--;
        }
    }
}
#endif
