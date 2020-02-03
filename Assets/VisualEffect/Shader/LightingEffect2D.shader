Shader "Hidden/Custom/LightingEffect2D"
{
	HLSLINCLUDE

	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	TEXTURE2D_SAMPLER2D(_PreRenderSource, sampler_PreRenderSource);

	float2 Units[100];

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		//get alpha first
		float alpha = 0;
		float _distance = distance(float2(0.5, 0.5), i.texcoord);
		alpha = 1 - saturate(_distance);

		float4 _postRenderColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float4 _preRenderColor = SAMPLE_TEXTURE2D(_PreRenderSource, sampler_PreRenderSource, i.texcoord);

		float4 color = lerp(_postRenderColor, _preRenderColor, alpha);

		return color;
	}

	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		//0
		Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}