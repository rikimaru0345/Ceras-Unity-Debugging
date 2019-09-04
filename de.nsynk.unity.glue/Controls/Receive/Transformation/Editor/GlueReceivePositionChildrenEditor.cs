#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceivePositionChildren))]
  public class GlueReceivePositionChildrenEditor : Editor
  {
    private GlueReceivePositionChildren _script;

    private string[] _options = Enum.GetNames(typeof(Space));

    void OnEnable()
    {
      _script = (GlueReceivePositionChildren)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      if (_script.overwrite)
      {
        for (var i = 0; i < _script._children.Count; i++)
        {
          var transform = _script._children[i];
          var update = EditorGUILayout.Vector3Field(transform.name, transform.position);
          if (_script._coordinateSpace == Space.World)
            transform.position = _script.offset ? transform.position + update : update;
          else if (_script._coordinateSpace == Space.Self)
            transform.localPosition = _script.offset ? transform.position + update : update;
        }
      }
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
