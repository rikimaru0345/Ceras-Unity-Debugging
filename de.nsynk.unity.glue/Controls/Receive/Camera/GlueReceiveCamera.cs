using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Glue
{
  [System.Serializable]
  [AddComponentMenu("Glue/ReceiveCamera [Glue]")]
  public class GlueReceiveCamera : GlueBehaviour
  {
    [HideInInspector]
    public Camera _unityCamera;
    private CameraStruct _glueCamera;
    private CameraStruct _fallbackCamera;

    private Vector3 _tempPosition = Vector3.zero;
    private Quaternion _tempRotation = Quaternion.identity;
    private Matrix4x4 _tempMatrix = Matrix4x4.identity;

    #region Unity lifecycle
    void Start()
    {
      _unityCamera = GetComponent<Camera>();
    }

    void Update()
    {
      _glueCamera = GlueValue(_fallbackCamera);
      _fallbackCamera = _glueCamera;
      CopyCamera(_glueCamera);
    }
    #endregion

    #region Glue camera struct to unity camera
    void CopyCamera(CameraStruct fromCamera)
    {
      if (fromCamera == null) return;
      _tempPosition = _unityCamera.transform.position;
      Utils.DXToUnityVector3(fromCamera.Position, ref _tempPosition);
      _unityCamera.transform.position = _tempPosition;
      _tempRotation = _unityCamera.transform.rotation;
      Utils.DXToUnityQuaternion(fromCamera.Rotation, ref _tempRotation);
      _unityCamera.transform.rotation = _tempRotation;
      _tempMatrix[0, 0] = fromCamera.Projection.M11;
      _tempMatrix[0, 1] = fromCamera.Projection.M12;
      _tempMatrix[0, 2] = fromCamera.Projection.M13;
      _tempMatrix[0, 3] = fromCamera.Projection.M14;
      _tempMatrix[1, 0] = fromCamera.Projection.M21;
      _tempMatrix[1, 1] = fromCamera.Projection.M22;
      _tempMatrix[1, 2] = fromCamera.Projection.M23;
      _tempMatrix[1, 3] = fromCamera.Projection.M24;
      _tempMatrix[2, 0] = fromCamera.Projection.M31;
      _tempMatrix[2, 1] = fromCamera.Projection.M32;
      _tempMatrix[2, 2] = fromCamera.Projection.M33;
      _tempMatrix[2, 3] = fromCamera.Projection.M34;
      _tempMatrix[3, 0] = fromCamera.Projection.M41;
      _tempMatrix[3, 1] = fromCamera.Projection.M42;
      _tempMatrix[3, 2] = fromCamera.Projection.M43;
      _tempMatrix[3, 3] = fromCamera.Projection.M44;
      _unityCamera.nonJitteredProjectionMatrix = _tempMatrix;
      _unityCamera.nearClipPlane = _tempMatrix.m23 / _tempMatrix.m22;
      _unityCamera.farClipPlane = _tempMatrix.m23 / (_tempMatrix.m22 + 1.0f);
      // still updating the fov field, since some script depend on it (canvas e.g.)
      _unityCamera.fieldOfView = Mathf.Atan(1.0f / _tempMatrix.m11) * 2.0f * Mathf.Rad2Deg;
    }
    #endregion
  }  
}
