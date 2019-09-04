using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  public struct GameObjectMaterial
  {
    public UnityEngine.GameObject go;
    public Renderer renderer;
    public bool clear;
  }

  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveMaterialValueGameObjects [Glue]")]
  public class GlueReceiveMaterialValueToGameObjects: GlueReceiveMaterialValue
  {
    // Don't allow shared material on children, as it would
    // lead to a whole chain of backing up values
    [HideInInspector]
    public bool UseSharedMaterial = false;

    [HideInInspector]
    public List<GameObjectMaterial> children = new List<GameObjectMaterial>();

    #region Unity lifecycle
    void Update()
    {
      for (var i = 0; i < children.Count; i++)
      {
        var gameObject = children[i].go;
        var renderer = children[i].renderer;
        var shouldClear = children[i].clear;
        if (gameObject != null)
          UpdateChild(renderer, shouldClear);
        if (shouldClear)
          children.RemoveAt(i);
      }
    }
    #endregion

    private void UpdateChild(Renderer renderer, bool clear = false)
    {
      if (renderer != null)
      {
        // Debug.Log($"{renderer.name} - {clear}");
        UpdateMaterialProperties(renderer, clear);
      }
    }
  }
}
