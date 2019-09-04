Shader "Hidden/nsynk/UI/GlueMeter"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

  Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                fixed4 color         : COLOR;
                float2 texcoord      : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _TimerColor;
            fixed4 _CounterColor;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float _timings[99];
            float _glueTimeBetweenReceiveAndLateUpdate[99];
            float _glueCounterDifferences[99];
            float _min;
            float _max;
            float _target;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                return OUT;
            }
            
            float plotYFunc(float2 uv, float pct) {
                return smoothstep(pct - 0.02, pct, uv.y) - smoothstep(pct, pct + 0.02, uv.y);
            }

            float plotYFuncFilled(float2 uv, float pct) {
	            return 1.0 - smoothstep(pct, pct + 0.02, uv.y);
            }

            float4 frag(v2f IN) : SV_Target
            {
                float4 color = (0).xxxx;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color = float4(0, 0, 0, 1.0);
                float2 uv = IN.texcoord; 
                uv.x = 1 - uv.x; // flip x

                float glueVal = _glueTimeBetweenReceiveAndLateUpdate[floor(uv.x * 99)];
                float yGlue = smoothstep(_min, _max, glueVal);
                float resGlue = plotYFunc(IN.texcoord, yGlue);
                color += resGlue * _TimerColor;

                float glueVal2 = _glueCounterDifferences[floor(uv.x * 99)];
                float yGlue2 = smoothstep(0.0, 1.0, glueVal2);
                float resGlue2 = plotYFuncFilled(IN.texcoord, yGlue2);
                color += resGlue2 * (_CounterColor * 0.5);

                color.a = 0.9;

                return color;
            }
        ENDCG
        }
    }
}
