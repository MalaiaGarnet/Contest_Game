Shader "Custom/Common_DrawGlow"
{
    Properties
    {
        [Header(TextureMenu)]
        _MainTex("메인텍스쳐", 2D) = "bump" {}
        _Opacity("Opacity", Range(0, 1)) = 0.1 // 투명도

        [Header(RimMenu)]
        _RimPow("Rim Power", int) = 3 // 외곽
        _RimHeight("Rim Height", float) = 5.0
        _RimColor("Rim Color", Color) = (0,1,1,1) // RGBA, 외곽 색깔
    }
    SubShader
    {
        // 쉐이더 전처리옵션들
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        /**********************************************************/
        ZWrite on
        ColorMask 0
        // 전방패스
        CGPROGRAM
        #pragma surface surf DisableLight noambient noforwardadd nolightmap novertexlights noshadow
        struct Input
        {
            float4 color:COLOR;
        };

        void surf(Input _In, inout SurfaceOutput _Out)
        {
        }

        float4 LightingDisableLight(SurfaceOutput _So, float3 _LightDir, float _Atten)
        {
            return float4(0,0,0,1);
        }
        ENDCG
        /**********************************************************/
        ZWrite off
        // 후방패스
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        // 변수
        sampler2D _MainTex;
        float     _Opacity;
        float     _RimPow;
        float     _RimHeight;

        float3    _RimColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 viewDir;
        };

        void surf(Input _In, inout SurfaceOutput _Out)
        {
            fixed4 col = tex2D(_MainTex, _In.uv_MainTex);

            _Out.Emission = _RimColor.rgb * _Opacity;
            float rim = saturate(dot(_Out.Normal, _In.viewDir));
            rim = saturate(pow(1 - rim, _RimPow) + pow(frac(_In.worldPos.g * _RimPow - _Time.y), _RimHeight) * 0.1);
            _Out.Alpha = rim * _Opacity;

            _Out.Albedo = col.rgb;
            _Out.Alpha = _Opacity;
        }

        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
