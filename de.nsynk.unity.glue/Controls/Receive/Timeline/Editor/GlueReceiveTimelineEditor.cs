#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveTimeline))]
  public class GlueReceiveTimelineEditor : Editor
  {
    private GlueReceiveTimeline _script;

    void OnEnable()
    {
      _script = (GlueReceiveTimeline)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
