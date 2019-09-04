using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glue
{
  [ExecuteAlways]
  [Serializable]
  [AddComponentMenu("Glue/ReceiveToggleGameObjects [Glue]")]
  public class GlueReceiveToggleGameObjects : GlueBehaviour
  {
    public List<UnityEngine.GameObject> gameObjects = new List<UnityEngine.GameObject>();
    bool[] _default = new bool[0];
    bool[] _bools = new bool[0];

    void Start()
    {
      _bools = new bool[gameObjects.Count];
      for (int i = 0; i < gameObjects.Count; i++)
      {
        _bools[i] = gameObjects[i].activeInHierarchy;
      }
      _default = _bools;
    }

    void Update()
    {
      if (Application.IsPlaying(gameObject))
      {
        _bools = GlueValue(_default);
        _default = _bools;
        for (int i = 0; i < gameObjects.Count; i++)
        {
          if (overwrite == false && gameObjects[i] != null)
            gameObjects[i].SetActive(_default[i % _bools.Length]);
        }
      }
    }

    public bool[] GetDefaults() { return _default; }
  }
}
