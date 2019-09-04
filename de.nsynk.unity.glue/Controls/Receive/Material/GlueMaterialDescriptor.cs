using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [Serializable]
  public enum GlueMaterialPropertyType { Color, Float, Vector };

  [Serializable]
  public struct GlueMaterialDescriptor
  {
    public string propertyName;
    public GlueMaterialPropertyType propertyType;
    public int[] materialSlots;

    [Header("Overwrite")]
    public bool useOverwrite;
    [ColorUsageAttribute(true, true)]
    public UnityEngine.Color overwriteColor;
    public UnityEngine.Vector4 overwriteVector;

    [HideInInspector]
    public bool isBackuped;
    [HideInInspector]
    public UnityEngine.Color backupColor;
    [HideInInspector]
    public UnityEngine.Vector4 backupVector;
  }
}
