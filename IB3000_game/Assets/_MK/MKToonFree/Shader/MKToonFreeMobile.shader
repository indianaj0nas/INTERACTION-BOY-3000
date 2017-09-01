Shader "Hidden/MK/Toon/FreeMobile"
{
	Properties
	{
		//Main
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Color (RGB)", 2D) = "white"{}

		//Normalmap
		_BumpMap ("Normalmap", 2D) = "bump" {}

		//Light
		_LightThreshold("LightThreshold", Range (0.01, 1)) = 0.3

		//Render
		_LightSmoothness ("Light Smoothness", Range(0,1)) = 0
		_RimSmoothness ("Rim Smoothness", Range(0,1)) = 0.5

		//Custom shadow
		_ShadowColor ("Shadow Color", Color) = (0.25,0.25,0.25,0.5)
		_HighlightColor ("Highlight Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ShadowIntensity("Shadow Intensity", Range (0.0, 2.0)) = 1.0

		//Outline
		_OutlineColor ("Outline Color", Color) = (0,0,0,1.0)
		_OutlineSize ("Outline Size", Float) = 0.02

		//Rim
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimSize ("Rim Size", Range(0.0,3.0)) = 1.5
		_RimIntensity("Intensity", Range (0, 1)) = 0.5

		//Specular
		_Shininess ("Shininess",  Range (0.01, 1)) = 0.275
		_SpecColor ("Specular Color", Color) = (1,1,1,0.5)
		_SpecularIntensity("Intensity", Range (0, 1)) = 0.5

		//Emission
		_EmissionColor("Emission Color", Color) = (0,0,0)
	}
	SubShader
	{
		LOD 150
		Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull Back
			Blend One Zero
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "Inc/Forward/MKToonForwardBaseSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" } 
			Name "FORWARDADD"
			Cull Back
			Blend One One
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT

			#pragma vertex vertfwd
			#pragma fragment fragfwd

			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#include "Inc/Forward/MKToonForwardAddSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}
		/////////////////////////////////////////////////////////////////////////////////////////////
		// SHADOWCASTER
		/////////////////////////////////////////////////////////////////////////////////////////////
		UsePass "Standard/SHADOWCASTER"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		UsePass "Standard/META"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// OUTLINE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			LOD 150
			Tags {"LightMode" = "Always"}
			Name "OUTLINE_SM_2_5"
			Cull Front
			Blend One Zero
			ZWrite On
			ZTest LEqual

			CGPROGRAM 
			#pragma target 2.5
			#pragma vertex outlinevert 
			#pragma fragment outlinefrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_fog

			#include "/Inc/Outline/MKToonOutlineOnlySetup.cginc"
			#include "/Inc/Outline/MKToonOutlineOnlyBase.cginc"
			ENDCG 
		}
    }
	FallBack "Diffuse"
	CustomEditor "MK.Toon.MKToonFreeEditor"
}
