#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveMaterialValueToGameObjects))]
  public class GlueReceiveMaterialValueToGameObjectsEditor : Editor
  {
    private GlueReceiveMaterialValueToGameObjects _script;

    void OnEnable()
    {
      _script = (GlueReceiveMaterialValueToGameObjects)target;
    }

    public override void OnInspectorGUI()
    {
      _script = (GlueReceiveMaterialValueToGameObjects)target;
      DrawDefaultInspector();
      EditorGUILayout.LabelField("Game Objects");
      for (var i = 0; i < _script.children.Count; i++)
      {
        EditorGUILayout.BeginHorizontal();
        GameObjectMaterial gom = _script.children[i];
        var go = (UnityEngine.GameObject)EditorGUILayout.ObjectField(gom.go, typeof(UnityEngine.GameObject));
        if (go != null)
        {
          gom.go = go;
          gom.renderer = go.GetComponent<Renderer>();
          _script.children[i] = gom;
        }
        if(GUILayout.Button("Remove"))
        {
          gom.clear = true;
          _script.children[i] = gom;
        }
        EditorGUILayout.EndHorizontal();
      }
      if(GUILayout.Button("Add Game Object"))
      {
        _script.children.Add(new GameObjectMaterial());
      }
    }
  }
}
#endif
