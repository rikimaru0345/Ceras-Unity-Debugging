using System;
using UnityEngine;

namespace Glue
{
  [Serializable]
  [ExecuteAlways]
  [AddComponentMenu("Glue/ReceivePosition.NEW [Glue]")]
  public class GlueReceivePositionNew : GlueBehaviour
  {
    public Space _coordinateSpace = Space.World;

    // UI
    public Vector3 overwritePosition = Vector3.zero;
    public bool offset;

    // Backuped position for overwrite
    private Vector3 _backupedPositionOnOverwrite = Vector3.zero;

    private Vector3 _positionWithoutOffset = Vector3.zero;
    private Vector3 _offsettedPosition = Vector3.zero;
    private Vector3 _positionFromGlue = Vector3.zero;

    // Save + restore
    private bool _isPositionBackuped = false;
    private bool _isPositionWithoutOffsetBackuped = false;

    // Fallback values
    private SharpDX.Vector3[] _fallback = new SharpDX.Vector3[] { SharpDX.Vector3.Zero };

    #region Unity lifecycle
    void Update()
    {
      if (Application.IsPlaying(gameObject))
      {
        int glueIndex = 0;
        UpdateFromGlue(this.transform, glueIndex, 1, overwrite);
      }
      else
        UpdateFromOverwrite(this.transform, overwritePosition);
    }
    #endregion

    
    #region Update functions for different use cases - Glue/Overwrite
    public void UpdateFromGlue(Transform t, int glueIndex, int count, bool overwrite)
    {
      if (overwrite)
      {
        BackupPosition(this.transform.position);
        UpdateFromOverwrite(this.transform, overwritePosition);
      }
      else
      {
        RestorePosition();
        if (_fallback.Length != count)
          Array.Resize(ref _fallback, count);
        SharpDX.Vector3[] _vectors = GlueValue(_fallback);
        _fallback = _vectors;
        _positionFromGlue = Utils.DXToUnityVector3(_vectors[glueIndex % count], _positionFromGlue);
        UpdatePosition(t, _positionFromGlue, _coordinateSpace);
      }
    }

    public void UpdateFromOverwrite(Transform t, Vector3 position)
    {
      if (overwrite)
      {
        BackupPosition(this.transform.position);
        UpdatePosition(t, position, _coordinateSpace);
      }
      else
      {
        RestorePosition();
      }
    }
    #endregion

    
    #region Save and restore of positions
    void SaveOffsetPosition(Vector3 position)
    {
      if (!_isPositionWithoutOffsetBackuped)
      {
        _positionWithoutOffset = position;
      }
      _isPositionWithoutOffsetBackuped = true;
    }

    void RestoreOffsetPosition(Transform t)
    {
      if (_isPositionWithoutOffsetBackuped)
        t.position = _positionWithoutOffset;
      _isPositionWithoutOffsetBackuped = false;
    }

    void BackupPosition(Vector3 p)
    {
      if (!_isPositionBackuped)
        _backupedPositionOnOverwrite = p;
      _isPositionBackuped = true;
    }

    void RestorePosition()
    {
      if (_isPositionBackuped)
        transform.position = _backupedPositionOnOverwrite;
      _isPositionBackuped = false;
    }
    #endregion

    
    #region
    void UpdatePosition(Transform t, Vector3 position, Space coordinateSpace)
    {
      switch (coordinateSpace)
      {
        case Space.World:
          UpdateWorldPosition(t, position);
          break;

        case Space.Self:
          UpdateLocalPosition(t, position);
          break;
      }
    }

    void UpdateWorldPosition(Transform t, Vector3 position)
    {
      if (offset)
      {
        SaveOffsetPosition(t.position);
        _offsettedPosition = _positionWithoutOffset + position;
        t.position = _offsettedPosition;
      }
      else
      {
        RestoreOffsetPosition(t);
        t.position = position;
      }
    }

    void UpdateLocalPosition(Transform t, Vector3 position)
    {
      if (offset)
      {
        SaveOffsetPosition(t.position);
        _offsettedPosition = _positionWithoutOffset + position;
        t.localPosition = _offsettedPosition;
      }
      else
      {
        RestoreOffsetPosition(t);
        t.localPosition = position;
      }
    }
    #endregion
  }
}
