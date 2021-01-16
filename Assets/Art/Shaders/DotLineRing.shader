Shader "Unlit/DotLineRing"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Size("Size", Float) = 10
        //_Width("LineWidth", Range(0, 1)) = 0.3
        _Angle("Angle", Range(0, 360)) = 30
        _isClockWise("isClockWise", range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching" = "true"}
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 localPos : TEXCOORD1;
            };

            float4 _Color;
            int _Size;
            float _Angle;
            bool _isClockWise;
            //float _LineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.localPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 degreeUV = frac(i.uv * float2(6, 1));
                float2 uv = frac(i.uv * float2(_Size, 1));
                fixed4 col = _Color;
                float width = pow(1.0 / _Size, 0.9);
                float radian = (_Angle > 180 ? 360 - _Angle : _Angle) / 180 * 3.1415926;
                bool lineMask;
                if (_isClockWise)
                {
                    bool onRight = cross(float3(i.localPos.x, i.localPos.y, 0), float3(0, 1, 0)).z > 0;
                    bool correctRadian = acos(dot(normalize(float2(i.localPos.x, i.localPos.y)), float2(0, 1))) < radian;
                    lineMask = _Angle > 180 ? !correctRadian || onRight : correctRadian && onRight;
                    lineMask = lineMask && distance(0.5, i.uv.y) < width * 3;
                }
                else 
                {
                    bool onRight = cross(float3(i.localPos.x, i.localPos.y, 0), float3(0, 1, 0)).z > 0;
                    bool correctRadian = acos(dot(normalize(float2(i.localPos.x, i.localPos.y)), float2(0, 1))) < radian;
                    lineMask = _Angle > 180 ? !correctRadian || !onRight : correctRadian && !onRight;
                    lineMask = lineMask && distance(0.5, i.uv.y) < width * 3;
                }
                bool dotMask = distance(0.5, i.uv.y) < width && uv.x > 0.5;
                bool degreeMask = (degreeUV.x > 0.98 || degreeUV.x < 0.02) && degreeUV.y < 0.5 + width;
                if (!dotMask && !lineMask && !degreeMask) 
                {
                    discard;
                }
                if(degreeMask && !dotMask && !lineMask)
                {
                    col = fixed4(1, 0.8, 0, 1);
                }
                return col;
            }
            ENDCG
        }
    }
}
