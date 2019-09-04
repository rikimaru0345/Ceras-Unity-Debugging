using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [AddComponentMenu("Glue/ReceiveScale [Glue Children]")]
  public class GlueReceiveScaleChildren : GlueBehaviour
  {
    public bool offset;

    List<Transform> _children;
    SharpDX.Vector3[] _values;
    SharpDX.Vector3[] _default;
    Vector3[] _initialScale;
    Vector3[] _unityValues;
    int _childCount;
    int _valuesLength;
    int _x;

    void Start()
    {
      _childCount = transform.childCount;
      _children = new List<Transform>(_childCount);
      _values = new SharpDX.Vector3[_childCount];
      _default = new SharpDX.Vector3[_childCount];
      _initialScale = new Vector3[_childCount];
      _unityValues = new Vector3[_childCount];

      int i = 0;
      foreach (Transform child in transform)
      {
        _children.Add(child);

        _initialScale[i] = child.localScale;
        _unityValues[i] = _initialScale[i];
        _values[i] = new SharpDX.Vector3(_initialScale[i].x,
                                         _initialScale[i].y,
                                         _initialScale[i].z);
        _default[i] = _values[i];

        i++;
      }
    }

    void Update()
    {
      _values = GlueValue(_default);
      _valuesLength = _values.Length;
      _default = _values;

      _x = 0;
      for (int i = 0; i < _childCount; i++)
      {
        _x = i % _valuesLength;
        Utils.DXToUnityVector3(_values[_x], ref _unityValues[_x]);

        _children[i].localScale = offset ? _unityValues[_x] + _initialScale[i]
                                         : _unityValues[_x];
      }
    }
  }
}
