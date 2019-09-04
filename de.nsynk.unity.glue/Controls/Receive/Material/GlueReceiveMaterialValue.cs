using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveMaterialValue [Glue]")]
  public class GlueReceiveMaterialValue : GlueAbstractReceiveMaterialValue
  {
    public bool _useSharedMaterial;

    #region Unity lifecycle
    void Start()
    {
      if (HasRenderer(transform))
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
      if (_renderer != null)
        UpdateMaterialProperties(_renderer);
    }
    #endregion
  }
}
