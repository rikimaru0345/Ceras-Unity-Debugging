#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveToggleGameObjects))]
  public class GlueReceiveToggleGameObjectsEditor : Editor
  {
    private GlueReceiveToggleGameObjects _script;

    void OnEnable()
    {
      _script = (GlueReceiveToggleGameObjects)target;
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
          if (gameObject != null)
          {
            bool change = EditorGUILayout.Toggle(gameObject.name, gameObject.active);
            gameObject.SetActive(change);
          }
        }
      }
      EditorGUILayout.EndVertical();
    }
  }
}
#endif
