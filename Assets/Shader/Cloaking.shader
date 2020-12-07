Shader "Custom/Cloaking"
{
	Properties
	{
		_Color ("Primary Color (R)", Color) = (1,1,1,0.5)
		_Color1 ("Secondary Color (G)", Color) = (0.5,0.5,0.5,0.5)
		_Color2 ("Tretiary Color (B)", Color) = (0,0,0,0.5)
		_MainTex ("Fallback (RGB)", 2D) = "white" {}
		_MaskTex ("Mask", 2D) = "black" {}
		_BumpMap ("Normals", 2D) = "" {}
		_SpecMap ("Specular", 2D) = "" {}
		_AOMap ("Ambient Occlusion", 2D) = "" {}
		_AOScale ("Ambient Occlusion Intensity", Range(1.0,10.0)) = 1
		_DetailMap ("Pattern (R)", 2D) = "" {}
        _Power("Vertex Color Intensity", Range(1.0,16.0) ) = 2.0
        [HDR]_GlowColor ("Self-Illumination Color", Color) = (0,0,0,1)
		_DamageColor ("Damage Color", Color) = (1,1,1,0)
		_BurnLevel("Burn Level", Range(0.0,1.0)) = 0.0
		_Opacity ("Opacity", Range(0.0, 1.0)) = 0.5
		[HDR]_OutColor("OutColor", Color) = (1,1,1,1)
		_NoiseTex("Noise Texture", 2D) = "white"{}
		_OutThinkness("OutThinkness", Range(0, 2.0)) = 1.15
		//_CutRender("_CutRender", Range(0.0, 1.0)) = 0.0
	}

	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"  }

		CGPROGRAM
		#pragma surface surf StandardSpecular alpha:fade
		//#pragma surface surf Lambert alpha:fade
		#pragma target 3.0
		#pragma shader_feature _NORMALMAP
		#pragma shader_feature _DETAILMAP
		#pragma shader_feature _AOMAP
		#pragma shader_feature _VERTEXCOLOR

		fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _GlowColor;
		fixed4 _DamageColor;
		fixed _BurnLevel;
		float _Opacity;

		sampler2D _MaskTex;
		sampler2D _SpecMap;
#ifdef _NORMALMAP
        sampler2D _BumpMap;
#endif
#ifdef _DETAILMAP
        sampler2D _DetailMap;
#endif
#ifdef _AOMAP
	    sampler2D _AOMap;
		fixed _AOScale;
#endif
		sampler2D _NoiseTex;
		float4	  _OutColor;
		float	  _OutThinkness;
		uniform float	  _CutRender;


		struct Input {
			float2 uv_MainTex;
			float2 uv_NoiseTex;
#ifdef _DETAILMAP
			float2 uv2_DetailMap;
#endif
			float3 viewDir;
		};

        float _Power;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o)
		{
			fixed4 col = fixed4(0,0,0,0);
			fixed4 mask = tex2D (_MaskTex, IN.uv_MainTex);
			fixed4 spec = tex2D (_SpecMap, IN.uv_MainTex);
#ifdef _DETAILMAP
			fixed4 patt = tex2D (_DetailMap, IN.uv2_DetailMap);
			_Color *= patt;
#endif
			col += _Color * mask.r;
			col += _Color1 * mask.g;
			col += _Color2 * mask.b;

			o.Albedo = col.rgb * (1 - _BurnLevel);
			o.Smoothness = col.a * (1 - _BurnLevel);
			o.Specular = spec.g * (1 - _BurnLevel);


#ifdef _VERTEXCOLOR
			o.Albedo *= col.rgb * _Power;
			o.Specular *= col.rgb;
#endif

#ifdef _AOMAP
			fixed4 ao = tex2D (_AOMap, IN.uv_MainTex);
			o.Albedo *= ao * _AOScale;
#endif

#ifdef _NORMALMAP
            o.Normal = UnpackNormal(tex2D (_BumpMap, IN.uv_MainTex));
#endif
			float4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex);
			float alpha = step(_CutRender, noise.r);
			float outline = step(noise.r, _CutRender * _OutThinkness);

			o.Albedo = col.rgb;
			o.Emission = (outline * _OutColor.rgb) + (col.rgb * mask.rgb) + (clamp((_GlowColor.rgb * mask.a * _GlowColor.a + _DamageColor.rgb * _DamageColor.a), fixed3(0,0,0), fixed3(1,1,1)) * (1 - _BurnLevel)) * _Opacity;
            o.Alpha = alpha * _Opacity;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
