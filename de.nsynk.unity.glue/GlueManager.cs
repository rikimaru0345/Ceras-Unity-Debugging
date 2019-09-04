using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using Ceras;
using System.Linq;

namespace Glue
{
  [RequireComponent(typeof(GlueConnector))]
  [ExecuteAlways]
  public class GlueManager : MonoBehaviour
  {
    private static bool _isTextureSharingInitialized = false;

    void OnEnable()
    {
      SetupCommandBuffers();
    }

    void SetupCommandBuffers()
    {
      Glue.DataPool.CommandBuffers = new Dictionary<string, CommandBuffer>();
    }

    void Update()
    {
      foreach (KeyValuePair<string, CommandBuffer> cmd in DataPool.CommandBuffers)
      {
        // @TODO
        // All texture copy jobs should happen in this cmdbuffer.
        // Make it extendable somehow
        Graphics.ExecuteCommandBuffer(cmd.Value); 
      }
      // Flush DX11 internal command buffer & execute
      // texture copy immediately
      GL.Flush();
    }

    void OnDestroy()
    {
      if (_isTextureSharingInitialized) GlueTexShare.Release();
    }

    #region Glue data API
    public static bool IsReady()
    {
      return true;
    }

    public static bool IsTextureSharingReady()
    {
      if (_isTextureSharingInitialized == false)
      {
        GlueTexShare.Load();
        GlueTexShare.Greet();
        _isTextureSharingInitialized = true;
      }
      return GlueTexShare.IsReady();
    }
    #endregion
  }  
}

