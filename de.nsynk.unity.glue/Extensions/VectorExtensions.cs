using System.Collections;
using UnityEngine;

public static class VectorExtensions 
{
  public static UnityEngine.Vector2 FromGlue(this Vector2 v, Glue.Exchange.Vector2 gv) {
    if (v != null && gv != null)
    {
      v.x = gv.x;
      v.y = gv.y;
    }
    return v;
  }
  
  public static UnityEngine.Vector3 FromGlue(this Vector3 v, Glue.Exchange.Vector3 gv) {
    if (v != null && gv != null)
    {
      v.x = gv.x;
      v.y = gv.y;
      v.z = gv.z;
    }
    return v;
  }

  public static UnityEngine.Vector4 FromGlue(this Vector4 v, Glue.Exchange.Vector4 gv) {
    if (v != null && gv != null)
    {
      v.x = gv.x;
      v.y = gv.y;
      v.z = gv.z;
      v.w = gv.w;
    }
    return v;
  }
}
