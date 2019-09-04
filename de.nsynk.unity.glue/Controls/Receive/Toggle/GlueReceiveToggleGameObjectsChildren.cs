using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveToggleGameObjects [Glue Children]")]
  public class GlueReceiveToggleGameObjectsChildren : GlueBehaviour
  {
    [HideInInspector]
    public List<GameObject> gameObjects;
    bool[] _default;
    bool[] _bools;
    int _count;
    int _length;

    void Start()
    {
      _count = transform.childCount;
      _bools = new bool[_count];
      gameObjects = new List<GameObject>(_count);

      int iteration = 0;
      foreach (Transform child in transform)
      {
        gameObjects.Add(child.gameObject);
        _bools[iteration] = child.gameObject.activeInHierarchy;
        iteration++;
      }

      _default = _bools;
    }

    void Update()
    {
      _bools = GlueValue(_default);
      _default = _bools;
      _length = _bools.Length;

      for (int i = 0; i < gameObjects.Count; i++)
      {
        if (overwrite == false)
          gameObjects[i].SetActive(_bools[i % _length]);
      }
    }
  }
}
