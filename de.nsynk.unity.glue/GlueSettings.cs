using System;
using UnityEngine;

namespace Glue
{
  [Serializable]
  public class Settings 
  {
    [Header("Debugging")]
    public bool IsVerbose = true;

    [Header("Refresh Rate")]
    public bool OverrideVSync = false;
    public int ForcedFPS = 50;

    [Header("UDP Settings")]
    public string IPOfReceiver = "127.0.0.1";
    public int PortOfUnityReceiver = 27184;
    public int PortToListenOn = 27183;
  }
}
