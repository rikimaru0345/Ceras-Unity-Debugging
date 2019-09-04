using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Glue
{
    [RequireComponent(typeof(Image))]
    public class GlueMeter : MonoBehaviour
    {
        [Header("Values in milliseconds")]
        public float min = 0.001f;
        public float max = 0.04f;
        public float target = 0.02f;
        public UnityEngine.Color TimeBetweenReceiveAndLateUpdateColor = UnityEngine.Color.white;
        public UnityEngine.Color CounterDifferenceColor = UnityEngine.Color.black;
        public Text text;

      [Header("Counter")]
      public int _counterDifferenceMax = 5;

        float timingCurrent;
        float timingAverage;
        int _fps;
        int _fpsAverage;

        Material mat;
        int _timings;
        int _gc;
        int _min;
        int _max;
        int _target;
        int _glueTimeBetweenReceiveAndLateUpdate_ms;
        int _glueCounterDiffences;

        float[] _glueTimingsBuffer;
        float[] _glueCounterBuffer;
        int capacity = 99;

      System.Random rand = new System.Random();

        void Start()
        {
            mat = new Material(Shader.Find("Hidden/nsynk/UI/GlueMeter"));
            var image = GetComponent<Image>();
            image.material = mat;
            image.raycastTarget = false;
            _glueTimeBetweenReceiveAndLateUpdate_ms = Shader.PropertyToID("_glueTimeBetweenReceiveAndLateUpdate");
            _glueCounterDiffences = Shader.PropertyToID("_glueCounterDifferences");
            _gc = Shader.PropertyToID("_gc");
            _min = Shader.PropertyToID("_min");
            _max = Shader.PropertyToID("_max");
            _target = Shader.PropertyToID("_target");
            _glueTimingsBuffer = new float[capacity];
            _glueCounterBuffer = new float[capacity];
            mat.SetColor("_TimerColor", TimeBetweenReceiveAndLateUpdateColor);
            mat.SetColor("_CounterColor", CounterDifferenceColor);
        }

        void Update()
        {
          _glueTimingsBuffer[0] = DataPool.Diagnostics.TimeBetweenReceivedAndLateUpdate_ms / 1000.0f;
          _glueCounterBuffer[0] = ((float)DataPool.Diagnostics.ReceiveFrameDifference) / _counterDifferenceMax;
          for (int i = capacity - 1; i > 0; i--)
          {
            _glueTimingsBuffer[i] = _glueTimingsBuffer[i - 1];
            _glueCounterBuffer[i] = _glueCounterBuffer[i - 1];
          }
          mat.SetFloatArray(_glueTimeBetweenReceiveAndLateUpdate_ms, _glueTimingsBuffer);
          mat.SetFloatArray(_glueCounterDiffences, _glueCounterBuffer);
          mat.SetFloat(_min, min);
          mat.SetFloat(_max, max);
          mat.SetFloat(_target, target);
        }
    }
}
