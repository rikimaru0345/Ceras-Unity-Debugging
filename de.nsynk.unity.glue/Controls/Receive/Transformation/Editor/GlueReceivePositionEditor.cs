#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Glue
{
  [CustomEditor(typeof(GlueReceivePosition))]
  public class GlueReceivePositionEditor : Editor
  {
    private GlueReceivePosition _script;

    private string[] _options = Enum.GetNames(typeof(Space));

    void OnEnable()
    {
      _script = (GlueReceivePosition)target;
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      var selectedOption = (int)_script._coordinateSpace;
      var userSelection = EditorGUILayout.Popup("Coordinate Space", selectedOption, _options);
      _script._coordinateSpace = (Space)userSelection;
      
      if(GUILayout.Button("Derive Key from Hierarchy"))
      {
        string derivedKey = GetParentsRecursive(_script.gameObject);
        _script.key = derivedKey;
      }

      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical();
      _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
      if (_script.overwrite)
      {
        _script._overwriteValue = EditorGUILayout.Vector3Field("Position", _script._overwriteValue);
      }
      EditorGUILayout.EndVertical();

    }

    private string GetParentsRecursive (GameObject o, string name = "")
    {
      if (name == string.Empty)
      {
        name = o.name;
      }

      if (o.transform.parent != null)
      {
        GameObject parent = o.transform.parent.gameObject;
        name = parent.name + "." + name;
        return GetParentsRecursive(parent, name);
      }

      return name + ".Position";
    }
  }
}
#endif
