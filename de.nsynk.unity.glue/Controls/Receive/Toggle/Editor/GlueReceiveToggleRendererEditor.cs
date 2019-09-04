#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveToggleRenderer))]
  public class GlueReceiveToggleRendererEditor : Editor
  {
    private GlueReceiveToggleRenderer _script;

    void OnEnable()
    {
      _script = (GlueReceiveToggleRenderer)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      if (_script.overwrite)
      {
        _script._overwriteValue = EditorGUILayout.Toggle("With: ", _script._overwriteValue);
      }
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
