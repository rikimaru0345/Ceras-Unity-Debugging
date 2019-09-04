using System;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [AddComponentMenu("Glue/ReceiveRotation [Glue]")]
  public class GlueReceiveRotation : GlueBehaviour
  {
    public Space _coordinateSpace;
    public bool offset;

    Vector3 _rotation;
    Vector3 _initialRotation;
    SharpDX.Vector3[] _value;
    SharpDX.Vector3[] _default;
    static float _toUnity = 360;

    void Start()
    {
      _value = _default = new SharpDX.Vector3[1];

      if (_coordinateSpace == Space.World)
      {
        _initialRotation = transform.eulerAngles;
        Utils.UnityToDXVector3(offset ? Vector3.zero : transform.eulerAngles, ref _default[0]);
      }
      else if (_coordinateSpace == Space.Self)
      {
        _initialRotation = transform.localEulerAngles;
        Utils.UnityToDXVector3(offset ? Vector3.zero : transform.localEulerAngles, ref _default[0]);
      }
    }

    void Update()
    {
      _value = GlueValue(_default);
      _default = _value;
      Utils.DXToUnityVector3(_value[0], ref _rotation);
      _rotation *= _toUnity;

      if (_coordinateSpace == Space.World)
      {
        transform.eulerAngles = offset ? _rotation + _initialRotation
                                       : _rotation;
      }
      else if (_coordinateSpace == Space.Self)
      {
        transform.localEulerAngles = offset ? _rotation + _initialRotation
                                            : _rotation;
      }
    }
  }
}
