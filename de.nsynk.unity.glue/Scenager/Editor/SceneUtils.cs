
// Credits: JohannesMP (2018-08-12)
// source: https://gist.github.com/JohannesMP/ec7d3f0bcf167dab3d0d3bb480e0e07b

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
/// Various BuildSettings interactions
/// </summary>
static public class SceneUtils
{
  // time in seconds that we have to wait before we query again when IsReadOnly() is called.
  public static float minCheckWait = 3;

  static float lastTimeChecked = 0;
  static bool cachedReadonlyVal = true;

  /// <summary>
  /// A small container for tracking scene data BuildSettings
  /// </summary>
  public struct SubScene
  {
    public int buildIndex;
    public GUID assetGUID;
    public string assetPath;
  }

  /// <summary>
  /// Check if the build settings asset is readonly.
  /// Caches value and only queries state a max of every 'minCheckWait' seconds.
  /// </summary>
  static public bool IsReadOnly()
  {
    float curTime = Time.realtimeSinceStartup;
    float timeSinceLastCheck = curTime - lastTimeChecked;

    if (timeSinceLastCheck > minCheckWait)
    {
      lastTimeChecked = curTime;
      cachedReadonlyVal = QueryBuildSettingsStatus();
    }

    return cachedReadonlyVal;
  }

  /// <summary>
  /// A blocking call to the Version Control system to see if the build settings asset is readonly.
  /// Use BuildSettingsIsReadOnly for version that caches the value for better responsivenes.
  /// </summary>
  static private bool QueryBuildSettingsStatus()
  {
    // If no version control provider, assume not readonly
    if (UnityEditor.VersionControl.Provider.enabled == false)
      return false;

    // If we cannot checkout, then assume we are not readonly
    if (UnityEditor.VersionControl.Provider.hasCheckoutSupport == false)
      return false;

    //// If offline (and are using a version control provider that requires checkout) we cannot edit.
    //if (UnityEditor.VersionControl.Provider.onlineState == UnityEditor.VersionControl.OnlineState.Offline)
    //    return true;

    // Try to get status for file
    var status = UnityEditor.VersionControl.Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
    status.Wait();

    // If no status listed we can edit
    if (status.assetList == null || status.assetList.Count != 1)
      return true;

    // If is checked out, we can edit
    if (status.assetList[0].IsState(UnityEditor.VersionControl.Asset.States.CheckedOutLocal))
      return false;

    return true;
  }

  /// <summary>
  /// For a given Scene Asset object reference, extract its settings data.
  /// </summary>
  static public SubScene GetScene(Object sceneObject)
  {
    SubScene entry = new SubScene()
      {
        buildIndex = -1,
        assetGUID = new GUID(string.Empty)
      };

    if (sceneObject as SceneAsset == null)
      return entry;

    entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
    entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

    return entry;
  }

  /// <summary>
  /// Enable/Disable a given scene
  /// </summary>
  static public void SetSceneState(SubScene buildScene, bool enable)
  {
    if (enable)
    {
      if (EditorApplication.isPlaying)
      {
        SceneManager.UnloadSceneAsync(EditorSceneManager.GetSceneByPath(buildScene.assetPath));
        Debug.Log("Disable " + SceneManager.GetSceneByPath(buildScene.assetPath).name);
      }
      else
      {
        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(buildScene.assetPath), false);
        Debug.Log("Disable " + EditorSceneManager.GetSceneByPath(buildScene.assetPath).name);
      }
    }
    else
    {
      if (EditorApplication.isPlaying)
      {
        SceneManager.LoadSceneAsync(buildScene.assetPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        Debug.Log("Enable " + SceneManager.GetSceneByPath(buildScene.assetPath).name);
      }
      else
      {
        EditorSceneManager.OpenScene(buildScene.assetPath, OpenSceneMode.Additive);
        Debug.Log("Enable " + EditorSceneManager.GetSceneByPath(buildScene.assetPath).name);
      }
    }
  }

  /// <summary>
  /// Add Scene to opened scenes
  /// </summary>
  static public void AddScene(SubScene subScene, bool active = true)
  {
    if (EditorApplication.isPlaying)
    {
      SceneManager.LoadSceneAsync(subScene.assetPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
      Debug.Log("Add " + SceneManager.GetSceneByPath(subScene.assetPath).name);

    }
    else
    {
      EditorSceneManager.OpenScene(subScene.assetPath, OpenSceneMode.Additive);
      if (active)
      {
        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(subScene.assetPath), false);
      }
      Debug.Log("Add " + EditorSceneManager.GetSceneByPath(subScene.assetPath).name);
    }
  }

  /// <summary>
  /// Open the default Unity Build Settings window
  /// </summary>
  static public void OpenBuildSettings()
  {
    EditorWindow.GetWindow(typeof(BuildPlayerWindow));
  }
}
#endif
