// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/uWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveSpeedY ("WaveSpeedY", Range(2, 221)) = 0.0
        _WaveSpeedX ("WaveSpeedX", Range(2, 221)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float _WaveSpeedY;
            float _WaveSpeedX;

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
                 float2 UVS = i.uv;
                 float dist = distance(worldPos.x, UVS.x);
                 float distx = distance(worldPos.y, UVS.y);
                 float u = sin(_Time.y * dist * 1 / (_WaveSpeedY*_WaveSpeedY))*0.01;// +i.vertex.x);
                 float v = cos(_Time.x * distx * 1 / (_WaveSpeedX* _WaveSpeedX)) * 0.01;// +i.vertex.y);
                 //float u = sin(_Time * i.vertex.x * 0.001);
                //float v = cos(_Time * i.vertex.y * 0.001);
                 //float nu = smoothstep(0, 1, u);
                 //float v = UVS.y;
                UVS += float2(u, v);
                fixed4 col = tex2D(_MainTex, UVS);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
