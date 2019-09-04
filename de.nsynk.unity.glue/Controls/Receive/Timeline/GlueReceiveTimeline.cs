using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Glue
{
  [ExecuteAlways]
  [System.Serializable]
  [AddComponentMenu("Glue/ReceiveTime [Glue]")]
  public class GlueReceiveTimeline : GlueBehaviour
  {
    private PlayableDirector _director;
    private float[] fallback = new float[]{ 0 };

    public double overwriteValue = 0.0;

    void Start()
    {
      _director = GetComponent<PlayableDirector>();
      _director.time = 0.0;
    }

    void Update()
    {
      if (_director != null)
      {
        float[] glueTime = GlueValue(fallback);
        _director.time = (double)glueTime[0];
        if (overwrite)
          _director.time = overwriteValue;
        fallback[0] = (float)_director.time;
        _director.Evaluate();
      }
    }
  }
}
