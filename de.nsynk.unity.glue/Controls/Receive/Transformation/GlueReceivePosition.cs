using System;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceivePosition [Glue]")]
  public class GlueReceivePosition : GlueBehaviour
  {
    [HideInInspector]
    public Space _coordinateSpace;
    public bool offset;

    Vector3 _position;
    Vector3 _initialPosition;
    SharpDX.Vector3[] _value = new SharpDX.Vector3[1] { SharpDX.Vector3.Zero };
    SharpDX.Vector3[] _default = new SharpDX.Vector3[1] { SharpDX.Vector3.Zero };

    [HideInInspector]
    public Vector3 _overwriteValue;

    void Start()
    {
      if (_coordinateSpace == Space.World)
      {
        _initialPosition = transform.position;
        Utils.UnityToDXVector3(offset ? Vector3.zero : transform.position, ref _default[0]);
      }
      else if (_coordinateSpace == Space.Self)
      {
        _initialPosition = transform.localPosition;
        Utils.UnityToDXVector3(offset ? Vector3.zero : transform.localPosition, ref _default[0]);
      }
    }

    Vector3 GetPosition()
    {
      _value = GlueValue(_default);
      _default = _value;
      Utils.DXToUnityVector3(_value[0], ref _position);

      if (overwrite)
        return _overwriteValue;

      if (offset)
        return _initialPosition + _position;
      else
        return _position;
    }

    void Update()
    {
      if (_coordinateSpace == Space.World)
        transform.position = GetPosition();
      else if (_coordinateSpace == Space.Self)
        transform.localPosition = GetPosition();
    }
  }
}
