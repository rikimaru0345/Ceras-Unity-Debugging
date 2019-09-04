using UnityEngine;

public static class Matrix4x4Extensions 
{
  // public static UnityEngine.Matrix4x4 FromGlue(this UnityEngine.Matrix4x4 m, Glue.Exchange.Matrix4x4 gm) {
  //   if (m != null && gm != null)
  //   {
  //     m.m00 = gm.m[0, 0];
  //     m.m01 = gm.m[0, 1];
  //     m.m02 = gm.m[0, 2];
  //     m.m03 = gm.m[0, 3];
  //     m.m10 = gm.m[1, 0];
  //     m.m11 = gm.m[1, 1];
  //     m.m12 = gm.m[1, 2];
  //     m.m13 = gm.m[1, 3];
  //     m.m20 = gm.m[2, 0];
  //     m.m21 = gm.m[2, 1];
  //     m.m22 = gm.m[2, 2];
  //     m.m23 = gm.m[2, 3];
  //     m.m30 = gm.m[3, 0];
  //     m.m31 = gm.m[3, 1];
  //     m.m32 = gm.m[3, 2];
  //     m.m33 = gm.m[3, 3];
  //   }
  //   return m;
  // }

  // public static Quaternion ExtractRotation(this Matrix4x4 matrix)
  // {
  //   Vector3 forward;
  //   forward.x = matrix.m02;
  //   forward.y = matrix.m12;
  //   forward.z = matrix.m22;
 
  //   Vector3 upwards;
  //   upwards.x = matrix.m01;
  //   upwards.y = matrix.m11;
  //   upwards.z = matrix.m21;
 
  //   return Quaternion.LookRotation(forward, upwards);
  // }
 
  // public static Vector3 ExtractPosition(this Matrix4x4 matrix)
  // {
  //   Vector3 position;
  //   position.x = matrix.m03;
  //   position.y = matrix.m13;
  //   position.z = matrix.m23;
  //   return position;
  // }
 
  // public static Vector3 ExtractScale(this Matrix4x4 matrix)
  // {
  //   Vector3 scale;
  //   scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
  //   scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
  //   scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
  //   return scale;
  // }
}
