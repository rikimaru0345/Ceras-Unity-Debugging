#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveToggleGameObjectsChildren))]
  public class GlueReceiveToggleGameObjectsChildrenEditor : Editor
  {
    private GlueReceiveToggleGameObjectsChildren _script;

    void OnEnable()
    {
      _script = (GlueReceiveToggleGameObjectsChildren)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      if (_script.overwrite)
      {
        for (int i = 0; i < _script.gameObjects.Count; i++)
        {
          var gameObject = _script.gameObjects[i];
          if (gameObject)
          {
            bool change = EditorGUILayout.Toggle(gameObject.name, gameObject.activeSelf);
            gameObject.SetActive(change);
          }
        }
      }
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
