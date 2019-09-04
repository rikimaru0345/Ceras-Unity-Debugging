using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [AddComponentMenu("Glue/ReceiveRotation [Glue Children]")]
  public class GlueReceiveRotationChildren : GlueBehaviour
  {
    public Space _coordinateSpace;
    public bool offset;

    List<Transform> _children;
    SharpDX.Vector3[] _values;
    SharpDX.Vector3[] _default;
    Vector3[] _unityValues;
    Vector3[] _initialRotation;
    int _valuesLength;
    int _childCount;
    int _x;
    static float _toUnity = 360;

    void Start()
    {
      _childCount = transform.childCount;
      _values = new SharpDX.Vector3[_childCount];
      _default = new SharpDX.Vector3[_childCount];
      _unityValues = new Vector3[_childCount];
      _initialRotation = new Vector3[_childCount];
      _children = new List<Transform>(_childCount);

      int i = 0;
      foreach (Transform child in transform)
      {
        _children.Add(child);

        if (_coordinateSpace == Space.World)
          _initialRotation[i] = child.eulerAngles;
        else if (_coordinateSpace == Space.Self)
          _initialRotation[i] = child.localEulerAngles;

        _values[i] = new SharpDX.Vector3(_initialRotation[i].x,
                                         _initialRotation[i].y,
                                         _initialRotation[i].z);
        _default[i] = _values[i];
        _unityValues[i] = _initialRotation[i];

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
        _unityValues[_x] *= _toUnity;

        if (_coordinateSpace == Space.World)
        {
          _children[i].eulerAngles = offset ? _initialRotation[i] + _unityValues[_x]
                                            : _unityValues[_x];
        }
        else if (_coordinateSpace == Space.Self)
        {
          _children[i].localEulerAngles = offset ? _initialRotation[i] + _unityValues[_x]
                                                 : _unityValues[_x];
        }
      }
    }
  }
}
