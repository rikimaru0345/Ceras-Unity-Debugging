using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [AddComponentMenu("Glue/ReceiveTransform [Glue]")]
  public class GlueReceiveTransform : GlueBehaviour
  {
    public Space _coordinateSpace;
    public bool _offset;

    float[] _values;
    float[] _default;

    // honestly not sure if this is nice like that. prolly not
    // sadly can't pass ref transform to the static method
    Vector3 _sca;
    Vector3 _rot;
    Vector3 _pos;
    Vector3 _initialSca;
    Vector3 _initialRot;
    Vector3 _initialPos;

    static int _size = 9;
    static float _toUnity = 360;

    void Start()
    {
      _values = new float[_size];
      _default = new float[_size];

      _sca = _initialSca = transform.localScale;
      if (_coordinateSpace == Space.World)
      {
        _rot = _initialRot = transform.eulerAngles;
        _pos = _initialPos = transform.position;
      }
      else if (_coordinateSpace == Space.Self)
      {
        _rot = _initialRot = transform.localEulerAngles;
        _pos = _initialPos = transform.localPosition;
      }

      Utils.UnityToTransformArray(_sca, _rot, _pos, ref _values);
      _default = _values;
    }

    void Update()
    {
      _values = GlueValue(_default);
      _default = _values;
      Utils.TransformArrayToUnity(_values, ref _sca, ref _rot, ref _pos);

      // update values
      transform.localScale = _offset ? _sca + _initialSca : _sca;
      if (_coordinateSpace == Space.World)
      {
        transform.eulerAngles = _offset ? _rot + _initialRot : _rot;
        transform.eulerAngles *= _toUnity;
        transform.position = _offset ? _pos + _initialPos : _pos;
      }
      else if (_coordinateSpace == Space.Self)
      {
        transform.localEulerAngles = _offset ? _rot + _initialRot : _rot;
        transform.localEulerAngles *= _toUnity;
        transform.localPosition = _offset ? _pos + _initialPos : _pos;
      }
    }
  }
}
