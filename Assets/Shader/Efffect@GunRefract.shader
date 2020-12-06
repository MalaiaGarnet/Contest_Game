Shader "Custom/Effect@GunRefract"
{
    Properties
    {
        _MainTex("메인텍스쳐", 2D) = "bump" {}
        _RefPow("굴절세기", Range(0, 1)) = 0.05
        [HDR]_RefColor("테 색깔", Color) = (0,0,0,1)
        [Toggle]_UseRefColor("테색깔쓸까요?", bool) = false
    }
    SubShader
    {
        Tags {  "RenderType"="Transparent" "Queue" = "Transparent"}
        ZWrite off

        GrabPass {}

        CGPROGRAM
        #pragma surface surf Invisible noambient alpha:fade  
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        // 텍스쳐
        sampler2D _GrabTexture;
        sampler2D _MainTex;
        
        float     _RefPow;
        float4    _RefColor;
        bool      _UseRefColor;

        struct Input
        {
            float4 screenPos;
            float2 uv_MainTex;
        };


        void surf (Input _IN, inout SurfaceOutput o)
        {
            float4 ref = tex2D(_MainTex, _IN.uv_MainTex);

            float3 screenUV = _IN.screenPos.rgb / _IN.screenPos.a;

            if(_UseRefColor)
            {
                o.Emission = tex2D(_GrabTexture, (screenUV.xy + ref.x * _RefPow)) * _RefColor;
            }
            else
            {
                o.Emission = tex2D(_GrabTexture, (screenUV.xy + ref.x * _RefPow));
            }
        }


        float4 LightingInvisible(SurfaceOutput _So, float3 _LightDir, float _Atten)
        {
            return float4(0,0,0,1);
        }

        ENDCG
    }
    FallBack "Regacy Shaders/Transparent/VertexLit"
}
