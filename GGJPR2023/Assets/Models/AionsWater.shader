Shader "Custom/AionsWater"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _WaveSpeedY("WaveSpeed", Range(0,10)) = 0.0
        _WaveSpeedX("WaveSpeed", Range(0,10)) = 0.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {} //[PerRendererData]
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        //Blend SrcAlpha OneMinusSrcAlpha
            //Blend One One
            Blend OneMinusSrcAlpha SrcAlpha
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float _WaveSpeedY;
        float _WaveSpeedX;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 UVS = IN.uv_MainTex;

            float u = sin(_Time * _WaveSpeedY);
            float v = cos(_Time * _WaveSpeedX);

            UVS += float2(u, v);
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, UVS) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = .1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
