using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceivePosition DEPRECATED [Glue Children]")]
  public class GlueReceivePositionChildren : GlueBehaviour
  {
    public Space _coordinateSpace;
    public bool offset;

    [HideInInspector]
    public List<Transform> _children;

    // this kind of sucks, lets find a better way
    SharpDX.Vector3[] _values;
    SharpDX.Vector3[] _default;
    Vector3[] _unityValues;
    Vector3[] _initialPosition;
    int _valuesLength;
    int _x;

    void Start()
    {
      _values = new SharpDX.Vector3[_children.Count];
      _default = new SharpDX.Vector3[_children.Count];
      _unityValues = new Vector3[_children.Count];
      _initialPosition = new Vector3[_children.Count];
      _children = new List<Transform>(_children.Count);
      int i = 0;
      foreach (Transform child in transform)
      {
        _children.Add(child);

        if (_coordinateSpace == Space.World)
          _initialPosition[i] = child.position;
        else if (_coordinateSpace == Space.Self)
          _initialPosition[i] = child.localPosition;

        _values[i] = new SharpDX.Vector3(_initialPosition[i].x,
                                         _initialPosition[i].y,
                                         _initialPosition[i].z);
        _default[i] = _values[i];
        _unityValues[i] = _initialPosition[i];

        i++;
      }
    }

    void Update()
    {
      // Currently broken:
      
      // if (GlueConnector.HasClients())
      // {
      //   _values = GlueValue(_default);
      //   _valuesLength = _values.Length;
      //   _default = _values;
      //   _x = 0;
      //   for (int i = 0; i < _children.Count; i++)
      //   {
      //     _x = i % _valuesLength;
      //     Utils.DXToUnityVector3(_values[_x], ref _unityValues[_x]);

      //     if (overwrite == false)
      //     {
      //       if (_coordinateSpace == Space.World)
      //         _children[i].position = offset ? _initialPosition[i] + _unityValues[_x] : _unityValues[_x];
      //       else if (_coordinateSpace == Space.Self)
      //         _children[i].localPosition = offset ? _initialPosition[i] + _unityValues[_x] : _unityValues[_x];
      //     }
      //   }
      // }
      // else
      // {
      //   for (int i = 0; i < _children.Count; i++)
      //   {
      //     if (overwrite == false)
      //     {
      //       if (_coordinateSpace == Space.World)
      //         _children[i].position = offset ? _initialPosition[i] + _unityValues[_x] : _unityValues[_x];
      //       else if (_coordinateSpace == Space.Self)
      //         _children[i].localPosition = offset ? _initialPosition[i] + _unityValues[_x] : _unityValues[_x];
      //     }
      //   }
      // }
    }
  }
}
