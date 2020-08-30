Shader "Custom/Cloaking"
{
    Properties
    {
        _MainTex("Albedo Texture", 2D) = "while"{}
        _NormalMap("Normal Map", 2D) = "bump" {}

        _Opacity("Opacity", Range(0, 1)) = 0.1 // 투명도
        _DeformIntensity("Deform by Normal Intensity", Range(0, 3)) = 1 // 왜곡

        _RimPow("Rim Power", int) = 3 // 외곽
        _RimColor("Rim Color", Color) = (0,1,1,1) // RGBA, 외곽 색깔
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }
       // ZBuffer를 잠시 비활성화
        zwrite off 

        GrabPass {} // 아래 대상을 송출하기전에, 배경을 따오는것
        CGPROGRAM
        #pragma surface surf CloakingLight noambient novertexlights noforwardadd 
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _GrabTexture; // 따올 텍스쳐 (GrabPass's Texture)
        sampler2D _NormalMap;

        float _DeformIntensity;
        float _Opacity;
        float _RimPow;
        fixed3 _RimColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;

            float4 ScreenPos;    
            float3 ViewDir; // 카메라방향
        };


        void surf (Input _In, inout SurfaceOutput o)
        {
          o.Normal = UnpackNormal(tex2D(_NormalMap, _In.uv_NormalMap));
          
          float4 uv_color = tex2D(_MainTex, _In.uv_MainTex);
        
          float2 uv_Screen = _In.ScreenPos.xy / _In.ScreenPos.w;

          fixed3 mappingScreenColor = tex2D(_GrabTexture, uv_Screen + o.Normal.xy * _DeformIntensity);
          
          float rimBrightness = 1 - saturate(dot(_In.ViewDir, o.Normal)); // 비교

          rimBrightness = pow(rimBrightness, _RimPow);

          o.Emission = mappingScreenColor * (1 - _Opacity) + _RimColor * rimBrightness;

          o.Albedo = uv_color.rgb;
        }
        // 위의 선언한 CloakingLight 구현부
        /*
         *
         *  @param s -> 빛을 제외한 surf 함수 처리된 결과 픽셀
         *  @param _LightDir -> 빛의 방향
         *  @param _Brightness -> 빛의 세기
         */
        fixed4 LightingCloakingLight(SurfaceOutput s, float3 _LightDir, float _Brightness)
        {
            return fixed4(s.Albedo * _Opacity * _LightColor0, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
