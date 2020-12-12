// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WireFrame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width ("Width", Range(0,1)) = 0.1
        _DotDensity ("DotDensity", Range(0,20)) = 10
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Cull Front
        ZTest Always

        //back pass
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
                float4 screenPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Width;
            int _DotDensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.worldPos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 scale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                float avgScale = (scale.x + scale.y + scale.z) / 3;

                //calculate dot mask
                float rootDepth = distance(unity_ObjectToWorld._m03_m13_m23, _WorldSpaceCameraPos);
                float biasedAvgScale = avgScale > 1 ? pow(avgScale, 0.3) : avgScale;
                _DotDensity *= 2 * max(floor((6 - rootDepth) * biasedAvgScale), 1);
                float dotMask = min(cos(i.uv * _DotDensity * 3.14).x, cos(i.uv * _DotDensity * 3.14).y);
                dotMask = step(dotMask, 0.01);

                float2 gridCoord = abs(i.uv * 2 - 1);
                if ((_Width / 3) * pow(i.vertex.w, 0.9) / pow(avgScale, 1) < 1 - max(gridCoord.x, gridCoord.y) || dotMask < 0.5) {
                    discard;
                }
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col = max(gridCoord.x, gridCoord.y);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * 0.5;
            }
            ENDCG
        }

        Tags { "RenderType"="Opaque" }
        LOD 100 
        Cull Back
        ZTest Always

        //front pass
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
                float4 screenPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Width;
            int _DotDensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.worldPos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 scale = float3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                float avgScale = (scale.x + scale.y + scale.z) / 3;

                //calculate dot mask
                float rootDepth = distance(unity_ObjectToWorld._m03_m13_m23, _WorldSpaceCameraPos);
                float biasedAvgScale = avgScale > 1 ? pow(avgScale, 0.3) : avgScale;
                _DotDensity *= 2 * max(floor((6 - rootDepth) * biasedAvgScale), 1);
                float dotMask = min(cos(i.uv * _DotDensity * 3.14).x, cos(i.uv * _DotDensity * 3.14).y);
                dotMask = step(dotMask, 0.01);

                float2 gridCoord = abs(i.uv * 2 - 1);
                if ((_Width / 3) * pow(i.vertex.w, 0.9) / pow(avgScale, 1) < 1 - max(gridCoord.x, gridCoord.y) || dotMask < 0.5) {
                    discard;
                }
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col = max(gridCoord.x, gridCoord.y);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
