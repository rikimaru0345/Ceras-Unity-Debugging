using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [ExecuteAlways]
  [AddComponentMenu("Glue/ReceivePosition.NEW [Glue Children]")]
  public class GlueReceivePositionChildrenNew : GlueReceivePositionNew
  {
    [Tooltip("Overwrite is not supported yet.")]
    public new bool overwrite;

    private List<Transform> _childTransforms = new List<Transform>();

    private SharpDX.Vector3[] _fallback = new SharpDX.Vector3[] { SharpDX.Vector3.Zero };

    #region Unity lifecycle
    void Start()
    {
      foreach (Transform ct in this.transform)
        TraverseChildren(ct);
      _fallback = new SharpDX.Vector3[_childTransforms.Count];
    }


    void Update()
    {
      if (overwrite) return;
      for (var i = 0; i < _childTransforms.Count; i++) {
        if (Application.IsPlaying(gameObject))
          UpdateFromGlue(_childTransforms[i], i, _childTransforms.Count, false);
      }
    }
    #endregion

    private void TraverseChildren(Transform t)
    {
      if (t.childCount == 0)
        _childTransforms.Add(t);
      else
      {
        _childTransforms.Add(t);
        foreach (Transform ct in t)
          TraverseChildren(ct);
      }
    }
  }
}
