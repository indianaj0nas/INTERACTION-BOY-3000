//include file for important calculations during rendering
#ifndef MK_TOON_INC
	#define MK_TOON_INC

	#include "../Common/MKToonDef.cginc"
	/////////////////////////////////////////////////////////////////////////////////////////////
	// INC
	/////////////////////////////////////////////////////////////////////////////////////////////

	//world normal
	inline half3 WorldNormal(sampler2D normalMap, float2 uv, half3x3 tbn)
	{
		half4 encode = tex2D(normalMap, uv);
		half3 local = half3(2.0 * encode.a - 1.0, 2.0 * encode.g - 1.0, 0.0);
		local.z = 1.0 - 0.5 * dot(local, local);
		return normalize(mul(local, tbn));
	}

	//specular blinn phong
	inline half GetSpecular(half ndhv, half shine, half ndl)
	{
		return (ndl > 0.0) ? pow(ndhv, shine * SHINE_MULT) : 0.0;
	}

	//threshold based lighting type
	inline half TreshHoldLighting(half lThreshold, half smoothness, half v)
	{
		return smoothstep(lThreshold-smoothness*T_H, lThreshold+smoothness*T_H, v);
	}

	//lerped Rim with smooth interpolation
	inline half3 RimLerped(half size, half3 vdn, fixed3 col, fixed intensity, half smoothness, fixed3 aCol)
	{
		fixed r = pow ((1.0 - saturate(vdn)), size);
		fixed3 nC = col.rgb * r;
		r = smoothstep(r - smoothness, r + smoothness, vdn);
		return lerp(aCol, lerp(nC,aCol, r), intensity);
	}

	//Brightness
	inline fixed3 BControl( fixed3 color, half b)
	{
		fixed3 bc = color * b;
		return bc;
	}
#endif