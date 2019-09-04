using UnityEngine;

namespace Glue
{
  [AddComponentMenu("Glue/ReceiveScale [Glue]")]
  public class GlueReceiveScale : GlueBehaviour
  {
    public bool offset;
    
    Vector3 _scale;
    Vector3 _initialValue;
    SharpDX.Vector3[] _value;
    SharpDX.Vector3[] _default;

    void Start()
    {
      _initialValue = transform.localScale;
    }

    void Update()
    {
      _value = GlueValue(_default);
      _default = _value;
      Utils.DXToUnityVector3(_value[0], ref _scale);

      transform.localScale = offset ? _scale + _initialValue
                                    : _scale;
    }
  }
}
