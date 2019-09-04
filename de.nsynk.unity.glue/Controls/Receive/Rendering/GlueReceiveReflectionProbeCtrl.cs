using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
namespace Glue
{
 [ExecuteAlways]
 [System.Serializable]
 [AddComponentMenu("Glue/ReceiveReflectionProbeCtrl [Glue]")]
 public class GlueReceiveReflectionProbeCtrl : GlueBehaviour
 {
   public Vector2 _overwrite = Vector2.zero;
   private Vector2 _backup = Vector2.zero;
   private Vector2 _glue = Vector2.zero;
   private bool _isBackuped = false;
   private SharpDX.Vector2[] _fallback = new SharpDX.Vector2[1] { SharpDX.Vector2.Zero };
   HDProbe _hdProbe;
   void Start()
   {
     _hdProbe = GetComponent<HDProbe>();
   }
   void Update()
   {
     if (_hdProbe == null)
       return;
     if (Application.IsPlaying(gameObject))
     {
       if (key == null || key.Length == 0) return;
       _fallback = GlueValue(_fallback);
       Utils.DXToUnityVector2(_fallback[0], ref _glue);
       _hdProbe.weight = LimitedWeight(_glue.x);
       _hdProbe.multiplier = _glue.y;
     }
     if (overwrite)
     {
       StoreWeight();
       _hdProbe.weight = LimitedWeight(_overwrite.x);
       _hdProbe.multiplier = _overwrite.y;
     }
     else if (_isBackuped)
       RestoreWeight();
   }
   void StoreWeight()
   {
     if (!_isBackuped)
     {
       _backup = new Vector2(_hdProbe.weight, _hdProbe.multiplier);
       _isBackuped = true;
     }
   }
   void RestoreWeight()
   {
     if (_isBackuped)
     {
       _hdProbe.weight = _backup.x;
       _hdProbe.multiplier = _backup.y;
       _isBackuped = false;
     }
   }
   float LimitedWeight(float w)
   {
     return Mathf.Clamp(w, 0.0f, 1.0f);
   }
 }
}
