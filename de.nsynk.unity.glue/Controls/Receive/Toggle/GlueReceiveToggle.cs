using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveToggle [Glue]")]
  public class GlueReceiveToggle : GlueBehaviour
  {
    public bool executeInEditMode = false;
    public bool initialState = false;
    bool[] _defaults = { false };
    bool[] _glueBools = { false };

    public GameObject toggledGameObject;

    void Start()
    {
      _defaults[0] = _glueBools[0] = initialState;
    }

    void Update()
    {
      if (!Application.isPlaying && !executeInEditMode) return;
      
      bool[] _glueBools = GlueValue(_defaults);
      toggledGameObject?.SetActive(_glueBools[0]);
      _defaults = _glueBools;
    }
  }
}
