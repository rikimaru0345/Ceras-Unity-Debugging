using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace Glue
{
    [ExecuteAlways]
    [System.Serializable]
    [AddComponentMenu("Glue/ReceiveLight [Glue]")]
    public class GlueReceiveLight : GlueBehaviour
    {
        public bool RunInEditMode = false;
        [HideInInspector]
        public Light _light;
        HDAdditionalLightData _hdLight;
        UnityEngine.Color _color = UnityEngine.Color.white;
        Vector4 _lightControl = Vector4.zero;
        float _intencityControl = 0;
        float _temperatureControl = 0;
        float[] _value = new float[6];
        float[] _default = new float[6];

        [HideInInspector]
        public Vector4 _overwriteColor;
        [HideInInspector]
        public float _overwriteIntesity;
        [HideInInspector]
        public float _overwriteTemperature;

        [HideInInspector]
        public bool _isBackuped = false;

        [HideInInspector]
        public float[] _backup = new float[6];

    void Start()
        {
            _light = GetLightComponent();
            _hdLight = GetComponent<HDAdditionalLightData>();
            // _hdLight.lightUnit = _lightUnit;
            _default[0] = _light.color.r;
            _default[1] = _light.color.g;
            _default[2] = _light.color.b;
            _default[3] = _light.color.a;
            _default[4] = _hdLight.intensity;
            _default[5] = _light.colorTemperature;
            _backup = new float[6] { 0, 0, 0, 0, 0, 0 };
            // Temp Color object to set the lights color in Update
            // without creating a new Color every frame, due to
            // _light.color.r is not settable
            _color = _light.color;
            _lightControl = new Vector4(_light.color.r,
                                        _light.color.g,
                                        _light.color.b,
                                        _light.color.a);
            _intencityControl = _hdLight.intensity;
            _temperatureControl = _light.colorTemperature;
        }

        void Update()
        {
            if (!RunInEditMode && !Application.isPlaying) return;
            // Handle glue value changes 
            if (Application.IsPlaying(gameObject))
                UpdateFromGlue();
            // Handle editor value changes
            else
                UpdateFromEditor();
            _color.r = _lightControl.x;
            _color.g = _lightControl.y;
            _color.b = _lightControl.z;
            _color.a = _lightControl.w;
            _light.color = _color;
            _hdLight.intensity = _intencityControl;
            _light.colorTemperature = _temperatureControl;
        }

        public void UpdateFromEditor()
        {
            if (overwrite)
            {
                if (_isBackuped == false)
                    BackupLight(_light, _hdLight);
                Overwrite();
            }
            else
            {
                if (_isBackuped == true)
                    RestoreBackup();
                else
                {
                    _lightControl.x = _light.color.r;
                    _lightControl.y = _light.color.g;
                    _lightControl.z = _light.color.b;
                    _lightControl.w = _light.color.a;
                    _intencityControl = _hdLight.intensity;
                    _temperatureControl = _light.colorTemperature;
                }
            }
        }

        public void UpdateFromGlue()
        {
            _value = GlueValue(_default);
            _default = _value;
            _lightControl.x = _value[0];
            _lightControl.y = _value[1];
            _lightControl.z = _value[2];
            _lightControl.w = _value[3];
            _intencityControl = _value[4];
            _temperatureControl = _value[5];
            if (overwrite)
            {
                Overwrite();
            }
        }

        public Light GetLightComponent()
        {
            return GetComponent<Light>();
        }

        public void BackupLight(Light l, HDAdditionalLightData hdl)
        {
            _backup[0] = l.color.r;
            _backup[1] = l.color.g;
            _backup[2] = l.color.b;
            _backup[3] = l.color.b;
            _backup[4] = hdl.intensity;
            _backup[5] = l.colorTemperature;
            _isBackuped = true;
        }

        public void RestoreBackup()
        {
            _lightControl.x = _backup[0];
            _lightControl.y = _backup[1];
            _lightControl.z = _backup[2];
            _lightControl.w = _backup[3];
            _intencityControl = _backup[4];
            _temperatureControl = _backup[5];
            _isBackuped = false;
        }

        void Overwrite()
        {
            _lightControl = _overwriteColor;
            _intencityControl = _overwriteIntesity;
            _temperatureControl = _overwriteTemperature;
        }
    }
}
