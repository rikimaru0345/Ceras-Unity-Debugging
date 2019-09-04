using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEditor;

namespace Glue
{
  [System.Serializable]
  [AddComponentMenu("Glue/ReceiveTexture [Glue]")]
  public class GlueReceiveTexture : MonoBehaviour
  {
    [HideInInspector]
    public List<string> keys = new List<string>();

    [HideInInspector]
    public List<int> textureSlotIndices = new List<int>();

    [SerializeField]
    private List<IntPtr> _sharedTextureAddress;

    private Texture2D _texture;
    private Texture2D _sharedTexture;
    private IntPtr _receiverPtr;
    private CommandBuffer _commandBuffer;
    private RenderTexture _renderTexture;

    public string[] GetTextureSlots()
    {
      var material = GetComponent<MeshRenderer>().sharedMaterial;
      return material.GetTexturePropertyNames();
    }

    public void OnEnable()
    {
      _sharedTextureAddress = new List<IntPtr>();
      for (int i = 0; i < keys.Count ; i++) {
        _sharedTextureAddress.Add(IntPtr.Zero);
      }
    }

    public void Update()
    {
      // if (Global.Cargo == null) {
      //   return;
      // }
      // for (int k = 0; k < keys.Count; k++) {
      //   string key = keys[k];
      //   if (!Global.Cargo.Contains(key)) {
      //     return;
      //   }
      //   IntPtr address = _sharedTextureAddress[k];
      //   if (address == IntPtr.Zero)
      //   {
      //     address = Global.Cargo.GetCargo<IntPtr>(keys[k], IntPtr.Zero);
      //     _sharedTextureAddress[k] = address;
      //     // @TODO handle inval values
      //     _receiverPtr = GlueTexShare.CreateReceiver(address);
      //     var texturePointer = GlueTexShare.GetTexturePointer(_receiverPtr, false);
      //     var textureFormat = GlueTexShare.GetTextureFormat(_receiverPtr);
      //     var unityTextureFormat = TextureFormatHelper.ResolveTextureFormat(textureFormat);
      //     var textureWidth = GlueTexShare.GetTextureWidth(_receiverPtr);
      //     var textureHeight = GlueTexShare.GetTextureHeight(_receiverPtr);
      //     _sharedTexture = Texture2D.CreateExternalTexture(textureWidth,
      //                                                      textureHeight,
      //                                                      unityTextureFormat,
      //                                                      false,
      //                                                      false,
      //                                                      texturePointer);
      //     // Generate a texture for the material
      //     _texture = new Texture2D(textureWidth,
      //                              textureHeight,
      //                              unityTextureFormat,
      //                              false,
      //                              true);
      //     // Texture copy via Command Buffer
      //     _commandBuffer = new CommandBuffer();
      //     _commandBuffer.name = "Glue Copy " + this.name + " " + key;
      //     _commandBuffer.CopyTexture(_sharedTexture, _texture);
      //     Global.CmdBuffers.Add($"{this.name}-{key}", _commandBuffer);
      //     // Change the material texture
      //     var renderer = GetComponent<MeshRenderer>();
      //     string textureSlotName = GetTextureSlots()[textureSlotIndices[k]];
      //     renderer.sharedMaterial.SetTexture(textureSlotName, _texture);
      //   }
      // }
    }
  }
}
