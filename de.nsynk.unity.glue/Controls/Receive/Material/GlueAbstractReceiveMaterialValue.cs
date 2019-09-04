using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  public abstract class GlueAbstractReceiveMaterialValue : GlueBehaviour
  {
    protected bool _useSharedMaterial = false;

    public GlueMaterialDescriptor materialDescriptor;

    [HideInInspector]
    public MaterialPropertyBlock _materialPropertyBlock;

    private bool _hasRenderer = false;
    protected Renderer _renderer;

    // Fallback vector is missued for general purpose:
    // for float values only the x component is used
    // for vectors and colors, the 4 component vector is used
    private SharpDX.Vector4[] _fallback = { SharpDX.Vector4.Zero };

    #region Unity lifecycle
    public void OnEnable()
    {
      _materialPropertyBlock = new MaterialPropertyBlock();
      _hasRenderer = HasRenderer(transform);
    }

    public void Update()
    {
      if (_hasRenderer)
        UpdateMaterialProperties(_renderer);
    }

    public void OnDestroy()
    {
      // Clear material property blocks
      if (_hasRenderer)
        UpdateMaterialProperties(_renderer, true);
    }
    #endregion

    protected void UpdateMaterialProperties(Renderer renderer, bool clear = false)
    {
      var isOverwritten = materialDescriptor.useOverwrite;
      var propertyName = materialDescriptor.propertyName;
      var type = materialDescriptor.propertyType;
      if (materialDescriptor.materialSlots.Length == 0) return;
      for(var i = 0; i < renderer.sharedMaterials.Length; i++)
      {
        try
        {
          var materialSlot = materialDescriptor.materialSlots[i];
          if (materialSlot >= renderer.sharedMaterials.Length) return;
          if (renderer.HasPropertyBlock())
            renderer.GetPropertyBlock(_materialPropertyBlock, materialSlot);
          if (clear)
          {
            _materialPropertyBlock.Clear();
            renderer.SetPropertyBlock(_materialPropertyBlock, materialSlot);
            return;
          }
          if (propertyName == "") return;
          if (Application.IsPlaying(gameObject))
          {
            if (isOverwritten)
              UpdateFromOverwrite(renderer, propertyName, type, materialSlot);
            else
              UpdateFromGlue(renderer, propertyName, type, materialSlot);
          }
          else
          {
            if (isOverwritten)
              UpdateFromOverwrite(renderer, propertyName, type, materialSlot);
            else
            {
              _materialPropertyBlock.Clear();
              renderer.SetPropertyBlock(_materialPropertyBlock, materialSlot);
              if (materialDescriptor.isBackuped)
                RestoreSharedMaterial(renderer, propertyName, type, materialSlot);
            }
          }
        }
        catch (IndexOutOfRangeException) {};
      }
    }

    private void RestoreSharedMaterial(Renderer renderer,
                                       string propertyName,
                                       GlueMaterialPropertyType type,
                                       int materialSlot)
    {
      if (type.Equals(GlueMaterialPropertyType.Float))
      {
        var restored = materialDescriptor.backupVector[0];
        renderer.sharedMaterials[materialSlot].SetFloat(propertyName, restored);
      }
      if (type.Equals(GlueMaterialPropertyType.Vector))
      {
        var restored = materialDescriptor.backupVector;
        renderer.sharedMaterials[materialSlot].SetVector(propertyName, restored);
      }
      if (type.Equals(GlueMaterialPropertyType.Color))
      {
        var restored = materialDescriptor.backupColor;
        renderer.sharedMaterials[materialSlot].SetColor(propertyName, restored);
      }
      materialDescriptor.isBackuped = false;
    }

    private void UpdateFromGlue(Renderer renderer,
                                string propertyName,
                                GlueMaterialPropertyType type,
                                int materialSlot)
    {
      if (key == "") return;
      if (materialSlot >= renderer.sharedMaterials.Length) return;
      if (type.Equals(GlueMaterialPropertyType.Float))
      {
        float[] fallback = { _fallback[0].X };
        float[] uv = GlueValue(fallback);
        _fallback[0].X = uv[0];
        if (_useSharedMaterial)
          renderer.sharedMaterials[materialSlot]?.SetFloat(propertyName, uv[0]);
        else
          _materialPropertyBlock.SetFloat(propertyName, uv[0]);
      }
      if (type.Equals(GlueMaterialPropertyType.Color))
      {
        SharpDX.Vector4[] v = GlueValue(_fallback);
        _fallback[0] = v[0];
        Vector4 uv = new Vector4();
        Utils.DXToUnityVector4(v[0], ref uv);
        if (_useSharedMaterial)
          renderer.sharedMaterials[materialSlot]?.SetColor(propertyName, uv);
        else
          _materialPropertyBlock.SetColor(propertyName, uv);
      }
      if (type.Equals(GlueMaterialPropertyType.Vector))
      {
        SharpDX.Vector4[] v = GlueValue(_fallback);
        _fallback[0] = v[0];
        Vector4 uv = new Vector4();
        Utils.DXToUnityVector4(v[0], ref uv);
        if (_useSharedMaterial)
          renderer.sharedMaterials[materialSlot]?.SetVector(propertyName, uv);
        else
          _materialPropertyBlock.SetVector(propertyName, uv);
      }
      if (!_useSharedMaterial)
        renderer.SetPropertyBlock(_materialPropertyBlock, materialSlot);
    }

    private void UpdateFromOverwrite(Renderer renderer,
                                     string propertyName,
                                     GlueMaterialPropertyType type,
                                     int materialSlot)
    {
      if (materialSlot >= renderer.sharedMaterials.Length) return;
      var hasProperty = renderer.sharedMaterials[materialSlot]?.HasProperty(propertyName);
      if ((bool)!hasProperty) return;
      if (type.Equals(GlueMaterialPropertyType.Float))
      {
        var overwriteValue = materialDescriptor.overwriteVector[0];
        if (_useSharedMaterial && !materialDescriptor.isBackuped)
        {
          materialDescriptor.backupVector[0] = renderer.sharedMaterials[materialSlot].GetFloat(propertyName);
          materialDescriptor.isBackuped = true;
        }
        if (_useSharedMaterial)
        {
          renderer.sharedMaterials[materialSlot].SetFloat(propertyName, overwriteValue);
        }
        else
          _materialPropertyBlock.SetFloat(propertyName, overwriteValue);
      }
      if (type.Equals(GlueMaterialPropertyType.Color))
      {
        var overwriteColor = materialDescriptor.overwriteColor;
        if (_useSharedMaterial && !materialDescriptor.isBackuped)
        {
          materialDescriptor.backupColor = renderer.sharedMaterials[materialSlot].GetColor(propertyName);
          materialDescriptor.isBackuped = true;
        }
        if (_useSharedMaterial)
          renderer.sharedMaterials[materialSlot].SetColor(propertyName, overwriteColor);
        else
          _materialPropertyBlock.SetColor(propertyName, overwriteColor);
      }
      if (type.Equals(GlueMaterialPropertyType.Vector))
      {
        var overwriteVector = materialDescriptor.overwriteVector;
        if (_useSharedMaterial && !materialDescriptor.isBackuped)
        {
          materialDescriptor.backupVector = renderer.sharedMaterials[materialSlot].GetVector(propertyName);
          materialDescriptor.isBackuped = true;
        }
        if (_useSharedMaterial)
          renderer.sharedMaterials[materialSlot].SetColor(propertyName, overwriteVector);
        else
          _materialPropertyBlock.SetVector(propertyName, overwriteVector);
      }
      if (!_useSharedMaterial)
        renderer.SetPropertyBlock(_materialPropertyBlock, materialSlot);
    }

    #region Helper
    public bool HasRenderer(Transform t)
    {
      try
      {
        _renderer = t.GetComponent<Renderer>();
        return _renderer != null;
      }
      catch (MissingComponentException) { return false; }
    }
    #endregion
  }
}
