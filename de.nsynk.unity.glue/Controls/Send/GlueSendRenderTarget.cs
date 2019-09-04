using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Glue
{
  [System.Serializable]
  [AddComponentMenu("NSYNK/Glue/Send/RenderTexture")]
  public class GlueSendRenderTarget : GlueBehaviour 
  {
    public Vector2Int textureSize;
    
    private IntPtr _senderPtr;
    private IntPtr _sharedHandlePtr;
    private Texture2D _texture;
    private Texture2D _sharedTex;
    private RenderTexture _renderTexture;
    private CommandBuffer _commandBuffer;
    private Camera _unityCamera;

    void Start()
    {
      GlueManager.IsTextureSharingReady();
      _unityCamera = GetComponent<Camera>();
      Debug.Log(_unityCamera);
    }

    void Update()
    {
      if (_unityCamera == null) return;
      if (_renderTexture == null)
      {
        InitSharedTex();
      }
      if (_renderTexture != null)
      {
        // Resolution change
        // if (_renderTexture.width != _glueCamera.ResolutionX ||
        //     _renderTexture.height != _glueCamera.ResolutionY)
        // {
        //   KillSharedTex();
        //   InitSharedTex();
        // }
      }
    }
 
    void InitSharedTex()
    {
      var desc = new RenderTextureDescriptor(textureSize.x,
                                             textureSize.y,
                                             RenderTextureFormat.ARGB32,
                                             24);
      desc.autoGenerateMips = false;
      desc.sRGB = true;
      _renderTexture = new RenderTexture(desc);
      _unityCamera.targetTexture = _renderTexture;
      _texture = new Texture2D(_renderTexture.width,
                               _renderTexture.height,
                               TextureFormat.ARGB32,
                               false,
                               true);
      var nativeTexPtr = _texture.GetNativeTexturePtr();
      _senderPtr = GlueTexShare.CreateSender(nativeTexPtr);
      var texPtr = GlueTexShare.GetTexturePointer(_senderPtr, true);
      _sharedTex = Texture2D.CreateExternalTexture(_renderTexture.width,
                                                   _renderTexture.height,
                                                   TextureFormat.ARGB32,
                                                   false,
                                                   false,
                                                   texPtr);
      _commandBuffer = new CommandBuffer();
      _commandBuffer.name = "Glue Copy " + this.name;
      _commandBuffer.CopyTexture(_renderTexture, _texture);
      _commandBuffer.CopyTexture(_texture, _sharedTex);
      DataPool.CommandBuffers.Add(this.name, _commandBuffer);
      if (_sharedHandlePtr == IntPtr.Zero)
      {
        _sharedHandlePtr = GlueTexShare.GetSharedHandle(_senderPtr, true);
        DataPool.SendFrame.Add(key, ref _sharedHandlePtr);
      }
    }

    void KillSharedTex()
    {
      DataPool.CommandBuffers.Remove(this.name);
      DataPool.SendFrame.Remove(key);
      _unityCamera.targetTexture = null;
      Destroy(_renderTexture);
      Destroy(_texture);
      Destroy(_sharedTex);
      _commandBuffer = null;
      _renderTexture = null;
      _texture = null;
      _senderPtr = IntPtr.Zero;
    }
  }
}
