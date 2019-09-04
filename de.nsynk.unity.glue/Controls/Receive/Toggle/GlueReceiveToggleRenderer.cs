using System;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveToggleRenderer [Glue]")]
  public class GlueReceiveToggleRenderer : GlueBehaviour
  {
    bool[] _defaults;
    bool[] _values;
    Renderer _renderer;

    [HideInInspector]
    public bool _overwriteValue;

    void Start()
    {
      _values = _defaults = new bool[1];
      _renderer = GetComponent<Renderer>();
      _defaults[0] = _renderer.enabled;
    }

    void Update()
    {
      _values = GlueValue(_defaults);
      _defaults = _values;
      if (overwrite)
      {
        _renderer.enabled = _overwriteValue;
      }
      else {
        _renderer.enabled = _values[0];
      }
    }
  }
}
