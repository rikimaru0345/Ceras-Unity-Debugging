using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [AddComponentMenu("Glue/ReceiveTransform [Glue Children]")]
  public class GlueReceiveTransformChildren : GlueBehaviour
  {
    public Space _coordinateSpace;
    public bool _offset;

    float[] _values;
    float[] _default;
    int _childCount;
    int _valuesLength;
    int _x;
    
    List<Transform> _children;
    Vector3[] _sca;
    Vector3[] _rot;
    Vector3[] _pos;
    Vector3[] _initialSca;
    Vector3[] _initialRot;
    Vector3[] _initialPos;

    static int _size = 9;
    static float _toUnity = 360;

    void Start()
    {
      _children = new List<Transform>();
      _childCount = transform.childCount;
      _values = new float[_childCount * _size];
      _default = new float[_childCount * _size];
      _sca = new Vector3[_childCount];
      _rot = new Vector3[_childCount];
      _pos = new Vector3[_childCount];
      _initialSca = new Vector3[_childCount];
      _initialRot = new Vector3[_childCount];
      _initialPos = new Vector3[_childCount];

      int i = 0;
      foreach (Transform child in transform)
      {
        _children.Add(child);

        _sca[i] = _initialSca[i] = child.localScale;
        if (_coordinateSpace == Space.World)
        {
          _rot[i] = _initialRot[i] = child.eulerAngles;
          _pos[i] = _initialPos[i] = child.position;
        }
        else if (_coordinateSpace == Space.Self)
        {
          _rot[i] = _initialRot[i] = child.localEulerAngles;
          _pos[i] = _initialPos[i] = child.localPosition;
        }

        Utils.UnityToTransformArray(_sca[i], _rot[i], _pos[i], ref _values, i * _size);
        i++;
      }
      
      _default = _values;
    }

    void Update()
    {
      _values = GlueValue(_default);
      _valuesLength = _values.Length;
      _default = _values;

      _x = 0;
      for (int i = 0; i < _childCount; i++)
      {
        _x = i % (_valuesLength / _size);
        Utils.TransformArrayToUnity(_values, ref _sca[_x], ref _rot[_x], ref _pos[_x], _x * _size);
        
        _children[i].localScale = _offset ? _sca[_x] + _initialSca[i] : _sca[_x];
        if (_coordinateSpace == Space.World)
        {
          _children[i].eulerAngles = _offset ? _rot[_x] + _initialRot[i] : _rot[_x];
          _children[i].eulerAngles *= _toUnity;
          _children[i].position = _offset ? _pos[_x] + _initialPos[i] : _pos[_x];
        }
        else if (_coordinateSpace == Space.Self)
        {
          _children[i].localEulerAngles = _offset ? _rot[_x] + _initialRot[i] : _rot[_x];
          _children[i].localEulerAngles *= _toUnity;
          _children[i].localPosition = _offset ? _pos[_x] + _initialPos[i] : _pos[_x];
        }
      }
    }
  }
}