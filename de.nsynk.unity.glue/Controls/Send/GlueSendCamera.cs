using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  public class GlueSendCamera : GlueBehaviour
  {
    private Camera _unityCamera;
    private CameraStruct _glueCamera;

    private SharpDX.Vector3 gluePosition = SharpDX.Vector3.Zero;
    private SharpDX.Vector4 glueRotation = SharpDX.Vector4.Zero;

    void Start()
    {
      _glueCamera = new CameraStruct();
      _unityCamera = GetComponent<Camera>();
    }

    void Update()
    {
      //Global.BackCargo.Add(key, ref _glueCamera);
    }

    void ToGlueCamera()
    {
      Utils.UnityToDXVector3(_unityCamera.transform.position, ref gluePosition);
      Utils.UnityToDXVector4(_unityCamera.transform.rotation, ref glueRotation);
      _glueCamera.Position = gluePosition;
      _glueCamera.Rotation = glueRotation;
    }
  }
}

