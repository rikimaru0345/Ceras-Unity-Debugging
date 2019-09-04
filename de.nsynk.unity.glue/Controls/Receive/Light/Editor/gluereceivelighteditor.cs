#if UNITY_EDITOR
using UnityEditor;

namespace Glue
{
    [CustomEditor(typeof(GlueReceiveLight))]
    public class GlueReceiveLightEditor : Editor
    {
        private GlueReceiveLight _script;

        void OnEnable()
        {
            _script = (GlueReceiveLight)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            _script.overwrite = EditorGUILayout.Toggle("Overwrite", _script.overwrite);
            if (_script.overwrite)
            {
                var light = _script.GetLightComponent();
                UnityEngine.Color oldColor = new UnityEngine.Color();
                oldColor.r = _script._overwriteColor.x;
                oldColor.g = _script._overwriteColor.y;
                oldColor.b = _script._overwriteColor.z;
                oldColor.a = _script._overwriteColor.w;

                UnityEngine.Color newColor = EditorGUILayout.ColorField("Color", oldColor);
                _script._overwriteColor.x = newColor.r;
                _script._overwriteColor.y = newColor.g;
                _script._overwriteColor.z = newColor.b;
                _script._overwriteColor.w = newColor.a;
                _script._overwriteTemperature = EditorGUILayout.FloatField("Temperature", _script._overwriteTemperature);

                _script._overwriteIntesity = EditorGUILayout.FloatField("Intensity", _script._overwriteIntesity);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
