
// Credits: JohannesMP (2018-08-12)

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
/// <summary>
/// Display a Scene Reference object in the editor.
/// If scene is valid, provides basic buttons to interact with the scene's role in Build Settings.
/// </summary>
[CustomPropertyDrawer(typeof(Scene))]
public class ScenePropertyDrawer : PropertyDrawer
{
    // The exact name of the asset Object variable in the SceneReference object
    const string sceneAssetPropertyString = "sceneAsset";
    // The exact name of  the scene Path variable in the SceneReference object
    const string scenePathPropertyString = "scenePath";

    static readonly RectOffset boxPadding = EditorStyles.helpBox.padding;
    static readonly float padSize = 2f;
    static readonly float lineHeight = EditorGUIUtility.singleLineHeight;
    static readonly float paddedLine = lineHeight + padSize;
    static readonly float footerHeight = 10f;

    /// <summary>
    /// Drawing the 'SceneReference' property
    /// </summary>

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var sceneAssetProperty = GetSceneAssetProperty(property);

        // Draw the Box Background
        position.height -= footerHeight;
        GUI.Box(EditorGUI.IndentedRect(position), GUIContent.none, EditorStyles.helpBox);
        position = boxPadding.Remove(position);
        position.height = lineHeight;

        // Draw the main Object field
        label.tooltip = "The actual Scene Asset reference.\nOn serialize this is also stored as the asset's path.";

        EditorGUI.BeginProperty(position, GUIContent.none, property);
        EditorGUI.BeginChangeCheck();
        int sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
        var selectedObject = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);
        SceneUtils.SubScene buildScene = SceneUtils.GetScene(selectedObject);

        if (EditorGUI.EndChangeCheck())
        {
            sceneAssetProperty.objectReferenceValue = selectedObject;

            //If no valid scene asset was selected, reset the stored path accordingly
            //if (buildScene.scene == null)
            //    GetScenePathProperty(property).stringValue = string.Empty;
        }
        position.y += paddedLine;

        if (buildScene.assetGUID.Empty() == false)
        {
            // Draw the Build Settings Info of the selected Scene
            DrawSceneInfoGUI(position, buildScene, sceneControlID + 1);
        }

        EditorGUI.EndProperty();
    }

    /// <summary>
    /// Ensure that what we draw in OnGUI always has the room it needs
    /// </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 2;
        SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);
        if (sceneAssetProperty.objectReferenceValue == null)
            lines = 1;

        return boxPadding.vertical + lineHeight * lines + padSize * (lines - 1) + footerHeight;
    }

    /// <summary>
    /// Draws info box of the provided scene
    /// </summary>
    private void DrawSceneInfoGUI(Rect position, SceneUtils.SubScene buildScene, int sceneControlID)
    {
        bool readOnly = SceneUtils.IsReadOnly();
        string readOnlyWarning = readOnly ? "\n\nWARNING: Build Settings is not checked out and so cannot be modified." : "";

        // Label Prefix
        GUIContent iconContent = new GUIContent();
        GUIContent labelContent = new GUIContent();


        // If scene is enabled
        bool isLoaded = false;
        if (Application.isPlaying)
        {
            isLoaded = SceneManager.GetSceneByPath(buildScene.assetPath).isLoaded;

        }
        else
        {
            isLoaded = EditorSceneManager.GetSceneByPath(buildScene.assetPath).isLoaded;
        }
        if (isLoaded)
        {
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_max");
            labelContent.text = "Enabled";
            labelContent.tooltip = "This scene is in build settings and ENABLED.\nIt will be included in builds." + readOnlyWarning;
        }
        // In build scenes and disabled
        else
        {
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_min");
            labelContent.text = "Disabled";
            labelContent.tooltip = "This scene is in build settings and DISABLED.\nIt will be NOT included in builds.";
        }

        // Left status label
        using (new EditorGUI.DisabledScope(readOnly))
        {
            Rect labelRect = DrawUtils.GetLabelRect(position);
            Rect iconRect = labelRect;
            iconRect.width = iconContent.image.width + padSize;
            labelRect.width -= iconRect.width;
            labelRect.x += iconRect.width;
            EditorGUI.PrefixLabel(iconRect, sceneControlID, iconContent);
            EditorGUI.PrefixLabel(labelRect, sceneControlID, labelContent);
        }

        // Right context buttons
        Rect buttonRect = DrawUtils.GetFieldRect(position);
        buttonRect.width = (buttonRect.width) / 3;

        string tooltipMsg = "";
        using (new EditorGUI.DisabledScope(readOnly))
        {
            // NOT loaded
            if (EditorSceneManager.GetSceneByPath(buildScene.assetPath).isSubScene && !Application.isPlaying)
            {
                SceneUtils.AddScene(buildScene);
            }
            // Loaded
            else
            {
                //bool isEnabled = buildScene.scene.enabled;
                buttonRect.width *= 2;

                bool isEnabled;
                if (Application.isPlaying)
                {
                    isEnabled = SceneManager.GetSceneByPath(buildScene.assetPath).isLoaded;
                }
                else
                {
                    isEnabled = EditorSceneManager.GetSceneByPath(buildScene.assetPath).isLoaded;
                }

                string stateString = isEnabled ? "Disable" : "Enable";
                tooltipMsg = stateString + " this scene in current scene.\n" + (isEnabled ? "It will no longer be included in scene" : "It will be included in scene") + "." + readOnlyWarning;

                if (DrawUtils.ButtonHelper(buttonRect, stateString, stateString + " In Scene", EditorStyles.miniButtonLeft, tooltipMsg))
                    SceneUtils.SetSceneState(buildScene, isEnabled);
                buttonRect.width /= 2;
                buttonRect.x += buttonRect.width;
            }
        }

        buttonRect.x += buttonRect.width;

        tooltipMsg = "Open the 'Build Settings' Window for managing scenes." + readOnlyWarning;
        if (DrawUtils.ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg))
        {
            SceneUtils.OpenBuildSettings();
        }

    }

    static SerializedProperty GetSceneAssetProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(sceneAssetPropertyString);
    }

    static SerializedProperty GetScenePathProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(scenePathPropertyString);
    }

    private static class DrawUtils
    {
        /// <summary>
        /// Draw a GUI button, choosing between a short and a long button text based on if it fits
        /// </summary>
        static public bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style, string tooltip = null)
        {
            GUIContent content = new GUIContent(msgLong);
            content.tooltip = tooltip;

            float longWidth = style.CalcSize(content).x;
            if (longWidth > position.width)
                content.text = msgShort;

            return GUI.Button(position, content, style);
        }

        /// <summary>
        /// Given a position rect, get its field portion
        /// </summary>
        static public Rect GetFieldRect(Rect position)
        {
            position.width -= EditorGUIUtility.labelWidth;
            position.x += EditorGUIUtility.labelWidth;
            return position;
        }
        /// <summary>
        /// Given a position rect, get its label portion
        /// </summary>
        static public Rect GetLabelRect(Rect position)
        {
            position.width = EditorGUIUtility.labelWidth - padSize;
            return position;
        }
    }
}
#endif
