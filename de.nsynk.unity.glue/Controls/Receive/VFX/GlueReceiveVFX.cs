using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  public class GlueReceiveVFX : GlueSimpleBehaviour
  {
    public List<GlueVFXDescriptor> _descriptors;
    private VisualEffect _vfx;
  
    void Start()
    {
      _vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
      for (var i = 0; i < _descriptors.Count; i++)
      {
        var descriptor = _descriptors[i];
        UpdateParameter(_vfx, descriptor);
      }
    }

    void UpdateParameter(VisualEffect vfx, GlueVFXDescriptor descriptor)
    {
      GlueVFXParameterType type = descriptor.parameterType;
      bool isInGameMode = Application.IsPlaying(gameObject);
      if (name.Length == 0) return;
      switch (type)
      {
        case GlueVFXParameterType.Bool:
          UpdateBool(vfx, isInGameMode, descriptor);
          break;

        case GlueVFXParameterType.Float:
          UpdateFloat(vfx, isInGameMode, descriptor);
          break;

        case GlueVFXParameterType.Vector2:
          UpdateVector2(vfx, isInGameMode, descriptor);
          break;

        case GlueVFXParameterType.Vector3:
          UpdateVector3(vfx, isInGameMode, descriptor);
          break;

        case GlueVFXParameterType.Vector4:
          UpdateVector4(vfx, isInGameMode, descriptor);
          break;
      }
    }

    #region GlueVFXParameterType update methods
    void UpdateBool(VisualEffect vfx, bool isInGameMode, GlueVFXDescriptor descriptor)
    {
      if (!vfx.HasBool(descriptor.parameterName)) return;
      bool glueBoolValue;
        try {
          if (isInGameMode)
          {
            bool[] glueBoolValues = GlueValue(descriptor.key, descriptor._fallbackBools);
            descriptor._fallbackBools = glueBoolValues;
            glueBoolValue = glueBoolValues[0];
          }
          else
            glueBoolValue = _vfx.GetBool(descriptor.parameterName);
          if (descriptor.useOverwrite)
            glueBoolValue = descriptor.overwriteBool;
          vfx.SetBool(descriptor.parameterName, glueBoolValue);
        } catch (IndexOutOfRangeException) {
          Debug.Log($"Could not find a Gluee value for {descriptor.key}. Did you misspelled the key?");
        }
    }

    void UpdateFloat(VisualEffect vfx, bool isInGameMode, GlueVFXDescriptor descriptor)
    {
      if (!vfx.HasFloat(descriptor.parameterName)) return;
      float glueFloatValue;
      try {
        if (isInGameMode)
        {
          float[] glueFloatValues = GlueValue(descriptor.key, descriptor._fallbackFloats);
          descriptor._fallbackFloats = glueFloatValues;
          glueFloatValue = glueFloatValues[0];
        }
        else
          glueFloatValue = _vfx.GetFloat(descriptor.parameterName);
        if (descriptor.useOverwrite)
          glueFloatValue = descriptor.overwriteVector.x;
        vfx.SetFloat(descriptor.parameterName, glueFloatValue);
      } catch (IndexOutOfRangeException) {
        Debug.Log($"Could not find a Gluee value for {descriptor.key}. Did you misspelled the key?");
      }
    }

    void UpdateVector2(VisualEffect vfx, bool isInGameMode, GlueVFXDescriptor descriptor)
    {
      if (!vfx.HasVector2(descriptor.parameterName)) return;
      Vector2 glueVector2Value;
      if (isInGameMode)
      {
        SharpDX.Vector2[] glueVector2Values = GlueValue(descriptor.key, descriptor._fallbackVectors2);
        descriptor._fallbackVectors2 = glueVector2Values;
        Vector2 castedVector2 = Vector2.zero;
        Utils.DXToUnityVector2(glueVector2Values[0], ref castedVector2);
        glueVector2Value = castedVector2;
      }
      else
        glueVector2Value = _vfx.GetVector2(descriptor.parameterName);
      if (descriptor.useOverwrite)
      {
        glueVector2Value.x = descriptor.overwriteVector.x;
        glueVector2Value.y = descriptor.overwriteVector.y;
      }
      vfx.SetVector2(descriptor.parameterName, glueVector2Value);
    }

    void UpdateVector3(VisualEffect vfx, bool isInGameMode, GlueVFXDescriptor descriptor)
    {
      if (!vfx.HasVector3(descriptor.parameterName)) return;
      Vector3 glueVector3Value;
      if (isInGameMode)
      {
        SharpDX.Vector3[] glueVector3Values = GlueValue(descriptor.key, descriptor._fallbackVectors3);
        descriptor._fallbackVectors3 = glueVector3Values;
        Vector3 castedVector3 = Vector3.zero;
        Utils.DXToUnityVector3(glueVector3Values[0], ref castedVector3);
        glueVector3Value = castedVector3;
      }
      else
        glueVector3Value = _vfx.GetVector3(descriptor.parameterName);
      if (descriptor.useOverwrite)
      {
        glueVector3Value.x = descriptor.overwriteVector.x;
        glueVector3Value.y = descriptor.overwriteVector.y;
      }
      vfx.SetVector3(descriptor.parameterName, glueVector3Value);
    }

    void UpdateVector4(VisualEffect vfx, bool isInGameMode, GlueVFXDescriptor descriptor)
    {
      if (!vfx.HasVector4(descriptor.parameterName)) return;
      Vector4 glueVector4Value;
      if (isInGameMode)
      {
        SharpDX.Vector4[] glueVector4Values = GlueValue(descriptor.key, descriptor._fallbackVectors4);
        descriptor._fallbackVectors4 = glueVector4Values;
        Vector4 castedVector4 = Vector4.zero;
        Utils.DXToUnityVector4(glueVector4Values[0], ref castedVector4);
        glueVector4Value = castedVector4;
      }
      else
        glueVector4Value = _vfx.GetVector4(descriptor.parameterName);
      if (descriptor.useOverwrite)
      {
        glueVector4Value.x = descriptor.overwriteVector.x;
        glueVector4Value.y = descriptor.overwriteVector.y;
        glueVector4Value.z = descriptor.overwriteVector.z;
        glueVector4Value.w = descriptor.overwriteVector.w;
      }
      vfx.SetVector4(descriptor.parameterName, glueVector4Value);
    }
    #endregion
  }
}
