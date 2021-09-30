Shader "Custom/TextureDependingNormal"
{
    Properties
    {
        _FloorTex ("Sol", 2D) = "white" {}
        _WallTex ("Pente", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
 
 
        CGPROGRAM
        #pragma surface surf Standard
 
        struct Input {
            float3 worldNormal;
            float2 uv_FloorTex;
            float2 uv_WallTex;
        };
 
        sampler2D _FloorTex;
        sampler2D _WallTex;
 
        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Floor
            if(IN.worldNormal.y > 0.8)
            {
                o.Albedo = tex2D(_FloorTex, IN.uv_FloorTex).rgb;
            }
            else
            {
                o.Albedo = tex2D(_WallTex, IN.uv_WallTex).rgb;
            }
            o.Emission = half3(0, 0, 0) * o.Albedo;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
        }
 
        ENDCG
    }
}