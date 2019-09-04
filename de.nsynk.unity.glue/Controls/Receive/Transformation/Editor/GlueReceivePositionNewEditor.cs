#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceivePositionNew))]
  public class GlueReceivePositionNewEditor : Editor
  {
    private GlueReceivePositionNew _script;

    void OnEnable()
    {
      _script = (GlueReceivePositionNew)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      if (_script.overwrite)
      {
      }
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
