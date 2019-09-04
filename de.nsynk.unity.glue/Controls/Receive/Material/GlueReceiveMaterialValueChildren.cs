using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveMaterialValue [Glue Children]")]
  public class GlueReceiveMaterialValueChildren : GlueReceiveMaterialValue
  {
    // Don't allow shared material on children, as it would
    // lead to a whole chain of backing up values
    [HideInInspector]
    public bool UseSharedMaterial = false;

    private List<Renderer> _childrenRenderer;

    #region Unity lifecycle
    void Start()
    {
      _childrenRenderer = new List<Renderer>();
      TraverseChildren(transform);
    }

    void Update()
    {
      for (var i = 0; i < _childrenRenderer.Count; i++)
      {
        UpdateMaterialProperties(_childrenRenderer[i]);
      }
    }
    #endregion

    private void TraverseChildren(Transform t)
    {
      if (t.childCount == 0)
      {
        if (HasRenderer(t))
          _childrenRenderer.Add(t.GetComponent<Renderer>());
      }
      else
      {
        if (HasRenderer(t))
          _childrenRenderer.Add(t.GetComponent<Renderer>());
        foreach (Transform ct in t)
          TraverseChildren(ct);
      }
    }

  }
}
