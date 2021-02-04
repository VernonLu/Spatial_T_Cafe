// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Wootopia/Outline"
{
	Properties
	{

		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}

		// Outline properties
		_Outline("Outline Width", Range(0,1)) = 0.01
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		//

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		[Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0

		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

		_BumpScale("Scale", Float) = 1.0
		[Normal] _BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
		_ParallaxMap ("Height Map", 2D) = "black" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		_DetailMask("Detail Mask", 2D) = "white" {}

		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		[Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0


		// Blending state
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0
		
		
	}

	CGINCLUDE
	#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG



	SubShader
	{

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]

			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------

			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _DETAIL_MULX2
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local _GLOSSYREFLECTIONS_OFF
			#pragma shader_feature_local _PARALLAXMAP

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			// Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
			//#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma vertex vertBase
			#pragma fragment fragBase
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}


		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARD_DELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend[_SrcBlend] One
			Fog { Color(0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual

			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------


			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local _DETAIL_MULX2
			#pragma shader_feature_local _PARALLAXMAP

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog
			// Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
			//#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma vertex vertAdd
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			ENDCG
		}


		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma target 3.0

			// -------------------------------------


			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _PARALLAXMAP
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing
			// Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
			//#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}


		// ------------------------------------------------------------------
		//  Deferred pass
		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }

			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma target 3.0
			#pragma exclude_renderers nomrt


			// -------------------------------------

			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local _DETAIL_MULX2
			#pragma shader_feature_local _PARALLAXMAP

			#pragma multi_compile_prepassfinal
			#pragma multi_compile_instancing
			// Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
			//#pragma multi_compile _ LOD_FADE_CROSSFADE

			#pragma vertex vertDeferred
			#pragma fragment fragDeferred

			#include "UnityStandardCore.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META"
			Tags { "LightMode" = "Meta" }

			Cull Off
			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			CGPROGRAM
			#pragma vertex vert_meta
			#pragma fragment frag_meta

			#pragma shader_feature _EMISSION
			#pragma shader_feature_local _METALLICGLOSSMAP
			#pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature_local _DETAIL_MULX2
			#pragma shader_feature EDITOR_VISUALIZATION

			#include "UnityStandardMeta.cginc"
			ENDCG
		}
		// ------------------
		// Outline Top Right
		Pass{
			Stencil
			{
				Ref 1
				Comp NotEqual
			}
			//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了
			Cull Front
			CGPROGRAM

			//include useful shader functions 
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float _Outline;
			fixed4 _OutlineColor;

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 pos : SV_POSITION;
			};

			//the vertex shader
			v2f vert(appdata_full v){
				v2f o;

				float3 viewUp = UNITY_MATRIX_IT_MV[1].xyz;
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				float3 viewRight = cross(viewDir, viewUp);
				v.vertex.xyz += normalize((1) * viewRight + (1) * viewUp) * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				return _OutlineColor;
			}

			ENDCG
		}
		
		
		// Outline Top Left
		Pass{
			//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了
			Cull Front
			Stencil
			{
				Ref 1
				Comp NotEqual
			}
			CGPROGRAM

			//include useful shader functions 
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float _Outline;
			fixed4 _OutlineColor;

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 pos : SV_POSITION;
			};

			//the vertex shader
			v2f vert(appdata_full v){
				v2f o;

				float3 viewUp = UNITY_MATRIX_IT_MV[1].xyz;
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				float3 viewRight = cross(viewDir, viewUp);
				v.vertex.xyz += normalize((-1) * viewRight + (1) * viewUp) * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				return _OutlineColor;
			}

			ENDCG
		}

		// Outline Bottom Right
		Pass{
			Stencil
			{
				Ref 1
				Comp NotEqual
			}
			//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了
			Cull Front
			CGPROGRAM

			//include useful shader functions 
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float _Outline;
			fixed4 _OutlineColor;

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 pos : SV_POSITION;
			};

			//the vertex shader
			v2f vert(appdata_full v){
				v2f o;

				float3 viewUp = UNITY_MATRIX_IT_MV[1].xyz;
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				float3 viewRight = cross(viewDir, viewUp);
				v.vertex.xyz += normalize((1) * viewRight + (-1) * viewUp) * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				return _OutlineColor;
			}

			ENDCG
		}
		
		// Outline Bottom Left
		Pass{
			//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了
			Cull Front
			Stencil
			{
				Ref 1
				Comp NotEqual
			}
			CGPROGRAM

			//include useful shader functions 
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float _Outline;
			fixed4 _OutlineColor;

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 pos : SV_POSITION;
			};

			//the vertex shader
			v2f vert(appdata_full v){
				v2f o;

				float3 viewUp = UNITY_MATRIX_IT_MV[1].xyz;
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;
				float3 viewRight = cross(viewDir, viewUp);
				v.vertex.xyz += normalize((-1) * viewRight + (-1) * viewUp) * _Outline;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				return _OutlineColor;
			}

			ENDCG
		}
		// End outline
		// --------------------------------

		

	}



	FallBack "VertexLit"
	// CustomEditor "StandardShaderGUI"
}