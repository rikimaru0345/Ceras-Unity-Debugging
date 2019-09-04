#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Glue
{
  public class GlueInspector : EditorWindow
  {
    private bool _updateEveryFrame = false;
    private bool _showGlueReceivingGroup = true;
    private bool _showGlueSendingGroup = true;

    private bool _receivedFrame = false;

    TextField _ui_label_fps;
    Label _ui_label_counter;
    Label _ui_label_counterDifference;
    Label _ui_label_sentBytes;
    Label _ui_label_connectedClientCount;

    public Dictionary<string, TextField> _textFieldsReceiving;
    public Dictionary<string, TextField> _textFieldsSender;

    [MenuItem("NSYNK/GlueInspector")]
    public static void ShowExample()
    {
      GlueInspector wnd = GetWindow<GlueInspector>();
      wnd.titleContent = new GUIContent("GlueInspector");
    }

    public void OnEnable()
    {
      GlueConnector.onReceiveFrame += ForcedRepaint;
      var path = "Packages/de.nsynk.unity.glue/Tools/GlueInspector.uxml";
      var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
      rootVisualElement.Add(visualTree.CloneTree());
      _textFieldsReceiving = new Dictionary<string, TextField>();

      var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/de.nsynk.unity.glue/Tools/GlueInspector.uss");
      rootVisualElement.styleSheets.Add(styleSheet);
    }

    void OnGUI()
    {
      try
      {
        _updateEveryFrame = rootVisualElement.Q<Toggle>("evaluateEveryFrame").value;
        rootVisualElement.Q<TextField>("fps").value = DataPool.Diagnostics.FPS.ToString();
        rootVisualElement.Q<TextField>("counter").value = DataPool.ReceivedFrame.Counter.ToString();
        rootVisualElement.Q<TextField>("counterDifference").value = DataPool.Diagnostics.ReceiveFrameDifference.ToString();
        rootVisualElement.Q<TextField>("sentBytes").value = DataPool.ReceivedFrame.Counter.ToString();
        rootVisualElement.Q<TextField>("timeBetween").value = DataPool.Diagnostics.TimeBetweenReceivedAndLateUpdate_ms.ToString();

        DrawReceivedFrameUI(DataPool.ReceivedFrame, rootVisualElement.Q<VisualElement>("receivedFrame"));
        DrawSendFrameUI(DataPool.SendFrame, rootVisualElement.Q<VisualElement>("sendFrame"));
      }
      catch (Exception e) {
        if (DataPool.Settings.IsVerbose)
          Debug.Log($"GlueInspector/Exception: {e.Message}");
      }
    }

    void DrawReceivedFrameUI(Frame frame, VisualElement root)
    {
      for (var i = 0; i < frame.Keys.Count; i++) {
        var key = frame.Keys.ToArray()[i];
        TextField field = new TextField();
        if (!_textFieldsReceiving.ContainsKey(key))
        {
          field.name = key;
          _textFieldsReceiving.Add(key, field);
          root.Add(field);
        }
        field = root.Q<TextField>(key);
        field.label = key;

        var value = frame.GetCargo<object>(key, null);
        if (value == null) continue;
        var valueType = value.GetType();
        if (valueType.IsArray)
        {
          Array arrayValue = value as Array;
          var stringRepresentation = $"[{arrayValue.Length}]: ";
          foreach (var v in arrayValue)
            stringRepresentation += $"{v.ToString()}, ";
          field.value = stringRepresentation;
        }
        else
          field.value = value.ToString();
      }
    }

    void DrawSendFrameUI(Frame frame, VisualElement root)
    {
      for (var i = 0; i < frame.Count; i++) {
        var key = frame.Keys.ToArray()[i];
        TextField field = new TextField();
        if (!_textFieldsSender.ContainsKey(key))
        {
          field.name = key;
          _textFieldsSender.Add(key, field);
          root.Add(field);
        }
        field = root.Q<TextField>(key);
        field.label = key;

        // if (!frame.ContainsKey(key))
        //   root.Rem

        var value = frame.GetCargo<object>(key, null);
        if (value == null) continue;
        var valueType = value.GetType();
        if (valueType.IsArray)
        {
          Array arrayValue = value as Array;
          var stringRepresentation = "";
          foreach (var v in arrayValue) stringRepresentation += $"{v.ToString()}, ";
          field.value = stringRepresentation;
        }
        else
          field.value = value.ToString();
      }
    }

    void Update()
    {
      if (_receivedFrame && _updateEveryFrame)
      {
        Repaint();
        _receivedFrame = false;
      }
    }

    public void ForcedRepaint()
    {
      _receivedFrame = true;
    }
  }
}
#endif
