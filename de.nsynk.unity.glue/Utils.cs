using UnityEngine;

namespace Glue
{
  public static class Utils
  {  
    public static void TransformArrayToUnity(float[] v, ref Vector3 s, ref Vector3 r, ref Vector3 p)
    {
      s.x = v[0];
      s.y = v[1];
      s.z = v[2];
      r.x = v[3];
      r.y = v[4];
      r.z = v[5];
      p.x = v[6];
      p.y = v[7];
      p.z = v[8];
    }

    public static void UnityToTransformArray(Vector3 s, Vector3 r, Vector3 p, ref float[] v)
    {
      v[0] = s.x;
      v[1] = s.y;
      v[2] = s.z;
      v[3] = r.x;
      v[4] = r.y;
      v[5] = r.z;
      v[6] = p.x;
      v[7] = p.y;
      v[8] = p.z;
    }

    public static void TransformArrayToUnity(float[] v, ref Vector3 s, ref Vector3 r, ref Vector3 p, int index)
    {
      s.x = v[index++];
      s.y = v[index++];
      s.z = v[index++];
      r.x = v[index++];
      r.y = v[index++];
      r.z = v[index++];
      p.x = v[index++];
      p.y = v[index++];
      p.z = v[index++];
    }

    public static void UnityToTransformArray(Vector3 s, Vector3 r, Vector3 p, ref float[] v, int index)
    {
      v[index++] = s.x;
      v[index++] = s.y;
      v[index++] = s.z;
      v[index++] = r.x;
      v[index++] = r.y;
      v[index++] = r.z;
      v[index++] = p.x;
      v[index++] = p.y;
      v[index++] = p.z;
    }

    public static Vector3 DXToUnityVector3(SharpDX.Vector3 dx, Vector3 v)
    {
      v.x = dx.X;
      v.y = dx.Y;
      v.z = dx.Z;
      return v;
    }

    public static void DXToUnityVector2(SharpDX.Vector2 dx, ref Vector2 v)
    {
      v.x = dx.X;
      v.y = dx.Y;
    }

    public static void DXToUnityVector3(SharpDX.Vector3 dx, ref Vector3 v)
    {
      v.x = dx.X;
      v.y = dx.Y;
      v.z = dx.Z;
    }

    public static void UnityToDXVector3(Vector3 v, ref SharpDX.Vector3 dx)
    {
      dx.X = v.x;
      dx.Y = v.y;
      dx.Z = v.z;
    }

    public static void DXToUnityVector4(SharpDX.Vector4 dx, ref Vector4 v)
    {
      v.x = dx.X;
      v.y = dx.Y;
      v.z = dx.Z;
      v.w = dx.W;
    }

    public static void DXToUnityQuaternion(SharpDX.Vector4 dx, ref Quaternion q)
    {
      q.x = dx.X;
      q.y = dx.Y;
      q.z = dx.Z;
      q.w = dx.W;
    }

    public static void UnityToDXVector4(Vector4 v, ref SharpDX.Vector4 dx)
    {
      dx.X = v.x;
      dx.Y = v.y;
      dx.Z = v.z;
      dx.W = v.w;
    }

    public static void UnityToDXVector4(Quaternion q, ref SharpDX.Vector4 dx)
    {
      dx.X = q.x;
      dx.Y = q.y;
      dx.Z = q.z;
      dx.W = q.w;
    }
  }
}
