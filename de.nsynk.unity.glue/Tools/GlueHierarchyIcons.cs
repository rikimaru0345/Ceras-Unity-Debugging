#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Glue;
 
[InitializeOnLoad]
public class GlueHierarchyIcons
{
  private static readonly Texture2D _iconTexture;
     
  static GlueHierarchyIcons()
  {
    var path = "Packages/de.nsynk.unity.glue/Assets/Icons/Glue Icon@0.1x.png";
    _iconTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
    if (_iconTexture == null) return;
    EditorApplication.hierarchyWindowItemOnGUI += DrawIconIntoHierarchy;
  }

  private static void DrawIconIntoHierarchy(int instanceID, Rect rect)
  {
    if (_iconTexture == null) return;
    GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    if (gameObject == null) return;
    bool belongsToGlue = gameObject.GetComponent<GlueBehaviour>();
    bool isGlue = gameObject.GetComponent<GlueManager>();
    if (belongsToGlue || isGlue)
    {
      int iconSize = 15;
      EditorGUIUtility.SetIconSize(new Vector2(iconSize, iconSize));
      var padding = new Vector2(0, 0);
      var iconDrawRect = new Rect(rect.xMax - (iconSize + padding.x),
                                  rect.yMin,
                                  rect.width,
                                  rect.height);
      var iconGUIContent = new GUIContent(_iconTexture);
      EditorGUI.LabelField(iconDrawRect, iconGUIContent);
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }
  }
}
#endif
