using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Glue
{
  public sealed class DataPool
  {
    private static readonly DataPool instance = new DataPool();

    public static Settings Settings = new Settings();

    public static Frame ReceivedFrame = new Frame();
    public static Frame SendFrame = new Frame();

    public static Dictionary<string, CommandBuffer> CommandBuffers = new Dictionary<string, CommandBuffer>();

    public struct Diagnostics
    {
      public static int SocketAvailable;
      public static int SocketReceiveBufferSize;
      public static int Framecount;
      public static ulong ReceiveFrameDifference;
      public static int TimeBetweenReceivedAndLateUpdate_ms;
      public static int FPS;
    }

    static DataPool() {}

    public static DataPool Instance {
      get { return instance; }
    }

    public static T GlueFor<T>(string key, T defaultValue)
    {
      // Only propagate values from Glue Global Cargo if a
      // connection has already been established
      try
      {
        var glueValue = ReceivedFrame.GetCargo<T>(key, defaultValue);
        if (glueValue == null) return defaultValue;
        return glueValue;
      }
      catch (NullReferenceException e)
      {
        if (Settings.IsVerbose)
          Debug.Log($"Glue: Exception: {e.Message}, {e.StackTrace}");
        return defaultValue;
      }
      catch (InvalidCastException e)
      {
        if (Settings.IsVerbose)
          Debug.Log($"Glue: Exception: {e.Message}, {e.StackTrace}");
        return defaultValue;
      }
    }
  }
}
