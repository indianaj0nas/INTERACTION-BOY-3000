//Light Calculations
#ifndef MK_TOON_LIGHT
	#define MK_TOON_LIGHT

	////////////
	// LIGHT
	////////////
	void MKToonLightMain(inout MKToonSurface mkts, in VertexOutputForward o)
	{
		//Base light calculation
		fixed baseLightCalc;

		mkts.Pcp.NdotL = mkts.Pcp.NdotL * T_V + T_V;
		baseLightCalc = mkts.Pcp.NdotL;

		mkts.Color_Diffuse = TreshHoldLighting(_LightThreshold, _LightSmoothness, baseLightCalc);
		mkts.Color_Diffuse *= mkts.Pcp.LightAttenuation;

		//Custom shadow
		_ShadowColor.rgb = lerp(_HighlightColor, _ShadowColor, _ShadowIntensity);
		mkts.Color_Diffuse = lerp(_ShadowColor, _HighlightColor, mkts.Color_Diffuse);

		fixed4 c;
		//Diffuse light
		c.rgb = mkts.Color_Albedo * mkts.Color_Diffuse;

		//Specular
		half spec;
		_Shininess *= mkts.Color_Specular.g;
		spec = GetSpecular(mkts.Pcp.NdotHV, _Shininess, mkts.Pcp.NdotL);
		mkts.Color_Specular = TreshHoldLighting(_LightThreshold, _LightSmoothness, spec);
		mkts.Color_Specular = mkts.Color_Specular * mkts.Pcp.LightAttenuation * _SpecColor * (_SpecularIntensity *  mkts.Color_Specular.r);

		//apply lightcolor
		c.rgb *= _LightColor0.rgb;

		//apply rim lighting
		c.rgb = RimLerped(_RimSize, mkts.Pcp.VdotN, _RimColor.rgb, _RimIntensity, _RimSmoothness, c.rgb);

		//apply specular
		c.rgb += mkts.Color_Specular * mkts.Pcp.ShadowAttenuation;

		//apply alpha
		c.a = mkts.Alpha;

		mkts.Color_Out = c;
	}

	void MKToonLightLMCombined(inout MKToonSurface mkts, in VertexOutputForward o)
	{
		//apply lighting to surface
		MKToonLightMain(mkts, o);

		#ifdef MK_TOON_FWD_BASE_PASS
			//add ambient light
			fixed3 amb = mkts.Color_Albedo * o.aLight;
			mkts.Color_Out.rgb += amb;
		#endif

		#ifdef MK_TOON_FWD_BASE_PASS
			#ifdef LIGHTMAP_ON
				#if DIRLIGHTMAP_COMBINED
					// directional lightmaps
					fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, o.uv_Lm.xy);
					half3 lm = DecodeLightmap(lmtex);
				#elif DIRLIGHTMAP_SEPARATE
					// directional with specular - no support
					half4 lmtex = 0;
					half3 lm = 0;
				#else
					// single lightmap
					fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, o.uv_Lm.xy);
					fixed3 lm = DecodeLightmap (lmtex);
				#endif
			#endif
		#endif

		#ifdef MK_TOON_FWD_BASE_PASS
			#ifdef LIGHTMAP_ON
				//apply lightmap
				#ifdef SHADOWS_SCREEN
					#if defined(UNITY_NO_RGBM)
						mkts.Color_Out.rgb += mkts.Color_Albedo * min(lm, mkts.Pcp.LightAttenuation*2);
					#else
						mkts.Color_Out.rgb += mkts.Color_Albedo * max(min(lm,(mkts.Pcp.LightAttenuation*2)*lmtex.rgb), lm*mkts.Pcp.LightAttenuation);
					#endif
				#else
					mkts.Color_Out.rgb += mkts.Color_Albedo * lm;
				#endif
			#endif
		#endif

		#ifdef MK_TOON_FWD_BASE_PASS
			#ifdef DYNAMICLIGHTMAP_ON
				//apply dynamic lightmap
				fixed4 dynlmtex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, o.uv_Lm.zw);
				mkts.Color_Out.rgb += mkts.Color_Albedo * DecodeRealtimeLightmap (dynlmtex);
			#endif
		#endif
	}
#endif