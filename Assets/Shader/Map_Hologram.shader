Shader "Custom/Map_Hologram"
{
    Properties
    {
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Opacity("Opacity", Range(0, 1)) = 0.1 // 투명도

        _HoloColor("HoloColor", Color) = (0,1,0,1)
        _RimPow("Rim Power", int) = 3 // 외곽
        _RimHeight("Rim Height", float) = 5.0
        _RimColor("Rim Color", Color) = (0,1,1,1) // RGBA, 외곽 색깔
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }

        CGPROGRAM
        #pragma surface surf invisible noambient alpha:fade  
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _NormalMap;

        float _Opacity;
        float _RimPow;
        float _RimHeight;

        fixed3 _RimColor;
        fixed3 _HoloColor;

        struct Input
        {
            float2 uv_NormalMap;

            float3 viewDir;    
            float3 worldPos; // 카메라방향
        };


        void surf (Input _In, inout SurfaceOutput o)
        {
          o.Normal = UnpackNormal(tex2D(_NormalMap, _In.uv_NormalMap));
          o.Emission = _HoloColor.rgb * _Opacity;
          float rim = saturate(dot(o.Normal, _In.viewDir));
          rim = saturate(pow(1 - rim, _RimPow) + pow(frac(_In.worldPos.g * _RimPow - _Time.y), _RimHeight) * 0.1);
          o.Alpha = rim * _Opacity;
        }

        float4 Lightinginvisible(SurfaceOutput s, float3 _LightDir, float _Brightness)
        {
            //return fixed4(s.Albedo * _Opacity * _LightColor0, 1);
            return float4(0, 0, 0, s.Alpha);
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
