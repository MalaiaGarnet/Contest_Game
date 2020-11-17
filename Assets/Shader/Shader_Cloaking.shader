Shader "Custom/Shader_Cloaking"
{
 Properties {
		_MainTex("기본 텍스쳐",2D) = "white" {}
		_DeformMap("울렁일 외곽텍스쳐",2D) = "black" {}
		_NormalMap("노말맵",2D) = "black" {}
		_Opacity("투명도",Range(0,1)) = 0.1
		_DeformIntense("울렁임정도",Range(0,3)) = 1
		_RimPow("Rim Pow",Range(0,60)) = 30
		_RimColor("Rim Color",Color) = (0,1,1,1)
		_Color ("Primary Color (R)", Color) = (1,1,1,0.5)
		_Color1 ("Secondary Color (G)", Color) = (0.5,0.5,0.5,0.5)
		_Color2 ("Tretiary Color (B)", Color) = (0,0,0,0.5)
		_MaskTex ("Mask", 2D) = "black" {}
		_SpecMap ("Specular", 2D) = "" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque"}
		zwrite off

		GrabPass {}

		CGPROGRAM
		#pragma surface surf StandardSpecular CloakingLight
		#pragma target 3.0

		sampler2D _SpecMap;
		sampler2D _MaskTex;
		sampler2D _GrabTexture;
		sampler2D _DeformMap;
		sampler2D _MainTex;
		sampler2D _NormalMap;

		float _DeformIntense;
		float _Opacity;
		float _RimPow;
		float3 _RimColor;

		fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;

		struct Input
		{
			float4 color : COLOR;
			float4 screenPos;
			float2 uv2_DeformMap;
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutputStandardSpecular o) 
		{
			fixed4 col = fixed4(0,0,0,0);
			fixed4 mask = tex2D (_MaskTex, IN.uv_MainTex);
			fixed4 spec = tex2D (_SpecMap, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_NormalMap,IN.uv_NormalMap));

			fixed4 patt = tex2D (_DeformMap, IN.uv2_DeformMap);
			_Color *= patt;

			col += _Color * mask.r;
			col += _Color1 * mask.g;
			col += _Color2 * mask.b;

			o.Albedo = col.rgb;
			o.Smoothness = col.a;
			o.Specular = spec.g;
			
			o.Albedo *= IN.color * 2.0;
			o.Specular *= IN.color;

			float2 noiseOffset = tex2D(_DeformMap,IN.uv2_DeformMap).rg;

			noiseOffset *= o.Normal.z * 0.1;

			float2 uv_screen = IN.screenPos.rg / IN.screenPos.a;

			uv_screen += o.Normal.rg * _DeformIntense;

			fixed3 mappingScreenColor = tex2D(_GrabTexture,uv_screen + noiseOffset);
			
			float rimBrightness = 1 - saturate(dot(IN.viewDir,o.Normal));
			rimBrightness = pow(rimBrightness,_RimPow);
			
			o.Emission = mappingScreenColor * (1-_Opacity) + _RimColor * rimBrightness;
			o.Alpha = _Opacity;
		}

		// Lighting 커스텀 함수
		fixed4 LightingCloakingLight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return fixed4(s.Albedo * _Opacity * _LightColor0, 1);
		}
		


		ENDCG
	}
	FallBack "Diffuse"
}
