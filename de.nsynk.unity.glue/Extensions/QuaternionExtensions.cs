using System.Collections;
using UnityEngine;

public static class QuaternionExtensions
{
  public static UnityEngine.Quaternion FromGlue(this UnityEngine.Quaternion q, Glue.Exchange.Vector4 gv)
  {
    q.x = gv.x;
    q.y = gv.y;
    q.z = gv.z;
    q.w = gv.w;
    return q;
  }
}
