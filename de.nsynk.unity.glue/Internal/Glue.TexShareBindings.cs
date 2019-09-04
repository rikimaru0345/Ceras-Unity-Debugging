using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;


// @TODO
// - cosistent usage of namespace Glue

namespace Glue
{
  public enum Color { red, green, blue, black, white, yellow, orange };
  public delegate void debugCallback(IntPtr message, int color, int size);

  public class GlueTexShare
  {
    #if UNITY_EDITOR
    #region Hot library loading
    static string _LIB_PATH = "/NSYNK/Glue/Internal/Glue.TexShare.dll";
    private static IntPtr _libraryHandle;

    [DllImport("kernel32")]
    public static extern IntPtr LoadLibrary(string path);

    [DllImport("kernel32")]
    public static extern IntPtr GetProcAddress(IntPtr libraryHandle, string symbolName);

    [DllImport("kernel32")]
    public static extern bool FreeLibrary(IntPtr libraryHandle);

    [System.Runtime.InteropServices.DllImport("UnityPluginLoader")]
    private static extern ulong GetUnityInterfacePtr();
    #endregion


    #region Delegate helper
    public static T GetDelegate<T>(IntPtr libraryHandle, string functionName) where T : class
  {
    IntPtr symbol = GetProcAddress(_libraryHandle, functionName);
    if (symbol == IntPtr.Zero)
    {
      throw new Exception("Glue.TexShareBindings: Function not found " + functionName);
    }
    return Marshal.GetDelegateForFunctionPointer(symbol, typeof(T)) as T;
  }
  #endregion

  
  #region
  delegate void LogDelegate(debugCallback cb);
  static LogDelegate _registerDebugCallback;
  [MonoPInvokeCallback(typeof(debugCallback))]
  public static void OnDebugCallback(IntPtr messagePtr, int color, int size)
  {
    string message = Marshal.PtrToStringAnsi(messagePtr, size);
    string colorPrefix = String.Format("<color={0}>", ((Color)color).ToString());
    string colorSufix = "</color>";
    string prettifiedLogString = String.Format("{0}{1}{2}", colorPrefix, message, colorSufix);
    UnityEngine.Debug.Log(prettifiedLogString);
  }

  delegate void InitDelegate(ulong interfacesPtr);
  static InitDelegate _initDelegate;

  delegate void UnloadDelegate();
  static UnloadDelegate _unloadDelegate; 

  delegate void GreetDelegate();
  static GreetDelegate _delegateGreet;

  delegate bool IsReadyDelegate();
  static IsReadyDelegate _isReadyDelegate;

  delegate IntPtr CreateSenderDelegate(IntPtr ptr);
  static CreateSenderDelegate _createSenderDelegate;

  delegate IntPtr CreateReceiverDelegate(IntPtr ptr);
  static CreateReceiverDelegate _createReceiverDelegate;
  
  delegate IntPtr GetTexturePointerDelegate(IntPtr ptr, bool isSender);
  static GetTexturePointerDelegate _getTexturePointerDelegate;

  delegate int GetTextureWidthDelegate(IntPtr ptr);
  static GetTextureWidthDelegate _getTextureWidthDelegate;
  
  delegate int GetTextureHeightDelegate(IntPtr ptr);
  static GetTextureHeightDelegate _getTextureHeightDelegate;

  delegate IntPtr GetSharedHandleDelegate(IntPtr ptr, bool isSender);
  static GetSharedHandleDelegate _getSharedHandleDelegate;

  delegate int GetTextureFormatDelegate(IntPtr ptr);
  static GetTextureFormatDelegate _getTextureFormatDelegate;
  #endregion

  public static void Load()
  {
    // Initialize DLL
    _libraryHandle = LoadLibrary(Application.dataPath + _LIB_PATH);
    Debug.Log("Library Path:" + Application.dataPath + _LIB_PATH);
    Debug.Log("Library Handle:" + _libraryHandle);
    // Request unity interface ptr
    ulong unityInterface = GetUnityInterfacePtr();
    Debug.Log("Unity Interface: " + unityInterface);
    // Init & Unload
    _initDelegate = GetDelegate<InitDelegate>(_libraryHandle, "Init");
    _initDelegate(unityInterface);
    _unloadDelegate = GetDelegate<UnloadDelegate>(_libraryHandle, "Unload");
    // Logging
    _registerDebugCallback = GetDelegate<LogDelegate>(_libraryHandle, "RegisterDebugCallback");
    _registerDebugCallback(OnDebugCallback);
    // Greet 
    _delegateGreet = GetDelegate<GreetDelegate>(_libraryHandle, "Greet");
    // IsReady
    _isReadyDelegate = GetDelegate<IsReadyDelegate>(_libraryHandle, "IsReady"); 
    // CreateSender
    _createSenderDelegate = GetDelegate<CreateSenderDelegate>(_libraryHandle, "CreateSender");
    // CreateReceiver
    _createReceiverDelegate = GetDelegate<CreateReceiverDelegate>(_libraryHandle, "CreateReceiver");
    // GetTexturePointer 
    _getTexturePointerDelegate = GetDelegate<GetTexturePointerDelegate>(_libraryHandle, "GetTexturePointer");
    // GetTextureWidth 
    _getTextureWidthDelegate = GetDelegate<GetTextureWidthDelegate>(_libraryHandle, "GetTextureWidth");
    // GetTextureHeight 
    _getTextureHeightDelegate = GetDelegate<GetTextureHeightDelegate>(_libraryHandle, "GetTextureHeight");
    // GetTextureFormat
    _getTextureFormatDelegate = GetDelegate<GetTextureFormatDelegate>(_libraryHandle, "GetTextureFormat");
    // GetSharedHandle
    _getSharedHandleDelegate = GetDelegate<GetSharedHandleDelegate>(_libraryHandle, "GetSharedHandle");
  }

  public static void Release()
  {
    _unloadDelegate();
    FreeLibrary(_libraryHandle);
  }

  public static void Greet()
  {
    _delegateGreet();
  }

  public static bool IsReady()
  {
    return _isReadyDelegate();
  }

  public static IntPtr CreateSender(IntPtr ptr)
  {
    return _createSenderDelegate(ptr);
  }

  public static IntPtr CreateReceiver(IntPtr ptr)
  {
    return _createReceiverDelegate(ptr);
  }

  public static IntPtr GetTexturePointer(IntPtr ptr, bool isSender)
  {
    return _getTexturePointerDelegate(ptr, isSender);
  }

  public static int GetTextureWidth(IntPtr ptr)
  {
    return _getTextureWidthDelegate(ptr);
  } 

  public static int GetTextureHeight(IntPtr ptr)
  {
    return _getTextureHeightDelegate(ptr);
  }

  public static IntPtr GetSharedHandle(IntPtr ptr, bool isSender)
  {
    return _getSharedHandleDelegate(ptr, isSender);
  }

  public static int GetTextureFormat(IntPtr ptr)
  {
    return _getTextureFormatDelegate(ptr);
  }
#else
  [MonoPInvokeCallback(typeof(debugCallback))]
  public static void OnDebugCallback(IntPtr messagePtr, int color, int size)
  {
    string message = Marshal.PtrToStringAnsi(messagePtr, size);
    string colorPrefix = String.Format("<color={0}>", ((Color)color).ToString());
    string colorSufix = "</color>";
    string prettifiedLogString = String.Format("{0}{1}{2}", colorPrefix, message, colorSufix);
    UnityEngine.Debug.Log(prettifiedLogString);
  }

  // Empty stub, needed for development & hot library reloading
  public static void Load() {}

  // Empty stub, needed for development & hot library reloading
  public static void Release() {}

  [DllImport("Glue.TexShare", CallingConvention = CallingConvention.Cdecl)]
  public static extern void RegisterDebugCallback(debugCallback callback);

  [DllImport("Glue.TexShare")]
  public static extern void Greet();

  [DllImport("Glue.TexShare")]
  public static extern IntPtr CreateReceiver(IntPtr ptr);

  [DllImport("Glue.TexShare")]
  public static extern IntPtr CreateSender(IntPtr ptr);

  [DllImport("Glue.TexShare")]
  public static extern IntPtr GetTexturePointer(IntPtr ptr, bool isSender);

  [DllImport("Glue.TexShare")]
  public static extern int GetTextureWidth(IntPtr ptr);

  [DllImport("Glue.TexShare")]
  public static extern int GetTextureHeight(IntPtr ptr);

  [DllImport("Glue.TexShare")]
  public static extern bool IsReady();

  [DllImport("Glue.TexShare")]
  public static extern IntPtr GetSharedHandle(IntPtr ptr, bool isSender);

  [DllImport("Glue.TexShare")]
  public static extern int GetTextureFormat(IntPtr ptr);
#endif
}
}
