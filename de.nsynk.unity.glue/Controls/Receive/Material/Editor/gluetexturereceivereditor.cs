#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace Glue
{
  [CustomEditor(typeof(GlueReceiveTexture))]
  public class GlueReceiveTextureEditor : Editor 
  {
    List<(string, int)> _receivers;

    string[] _textureSlotOptions;

    GlueReceiveTexture _receiverScript;

    private string[] ConvertTextureSlotNamesToHumanReadable(string[] slots)
    {
      return slots.Select(s => s.Replace("_", " "))
        .Select(s => Regex.Replace(s, "(\\B[A-Z])", " $1"))
        .ToArray();
    }

    void FetchDataFromSerialized()
    {
      for (int i = 0; i < _receiverScript.keys.Count; i++) {
        var entry = (_receiverScript.keys[i], _receiverScript.textureSlotIndices[i]);
        _receivers.Add(entry);
      }
    }

    void OnEnable()
    {
      _receiverScript = (GlueReceiveTexture)target;
      _receivers = new List<(string, int)>();
      _textureSlotOptions = _receiverScript.GetTextureSlots();
      FetchDataFromSerialized();
    }
    
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      if (GUILayout.Button("Add"))
      {
        _receivers.Add(("", 0));
        _receiverScript.keys.Add("");
        _receiverScript.textureSlotIndices.Add(0);
      }
      for (int i = 0; i < _receivers.Count; i++) {
        var key = _receivers[i].Item1;
        var selectedSlot = _receivers[i].Item2;
        var options = ConvertTextureSlotNamesToHumanReadable(_textureSlotOptions);
        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Texture Slot");
        var newKey = EditorGUILayout.TextField("Key", key);
        var selected = EditorGUILayout.Popup("Texture Slot", selectedSlot, options);
        if (GUILayout.Button("Remove"))
        {
          _receivers.RemoveAt(i);
          _receiverScript.keys.RemoveAt(i);
          _receiverScript.textureSlotIndices.RemoveAt(i);
        } else {
          _receivers[i] = (newKey, selected);
          _receiverScript.keys[i] = newKey;
          _receiverScript.textureSlotIndices[i] = selected;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
      }
    }
  }
}
#endif
