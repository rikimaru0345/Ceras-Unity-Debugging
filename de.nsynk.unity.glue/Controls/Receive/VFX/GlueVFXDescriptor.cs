using System;
using UnityEngine;

namespace Glue
{
  [Serializable]
  public enum GlueVFXParameterType { Bool, Float, Vector2, Vector3, Vector4 };

  [Serializable]
  public struct GlueVFXDescriptor
  {
    [Header("Glue")]
    public string key;

    [Header("Visual Effects Graph")]
    public string parameterName;
    public GlueVFXParameterType parameterType;

    [Header("Overwrite")]
    public bool useOverwrite;
    public bool overwriteBool;
    public Vector4 overwriteVector;

    [HideInInspector]
    public bool[] _fallbackBools;
    [HideInInspector]
    public float[] _fallbackFloats;
    [HideInInspector]
    public SharpDX.Vector2[] _fallbackVectors2;
    [HideInInspector]
    public SharpDX.Vector3[] _fallbackVectors3;
    [HideInInspector]
    public SharpDX.Vector4[] _fallbackVectors4;
  }
}
