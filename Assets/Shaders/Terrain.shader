Shader "Custom/TextureDependingNormal"
{
    Properties
    {
        _FloorTex ("Sol", 2D) = "white" {}
        _WallTex ("Pente", 2D) = "white" {}
        _FloorMidHeightTex ("Sol milieu", 2D) = "white" {}
        _FloorSnow ("Sol haut", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }

 
        CGPROGRAM
        #pragma surface surf Standard
 
        struct Input {
            float3 worldNormal;
            float3 worldPos;
            float2 uv_FloorTex;
            float2 uv_WallTex;
            float2 uv_FloorMidHeightTex;
            float2 uv_FloorSnow;
            
        };
 
        sampler2D _FloorTex;
        sampler2D _WallTex;
        sampler2D _FloorMidHeightTex;
        sampler2D _FloorSnow;
 
        void surf(Input IN, inout SurfaceOutputStandard o) {
            
            float h = IN.worldPos.y;

            if (IN.worldNormal.y > 0.73){
                if(h < 45) o.Albedo = tex2D(_FloorTex, IN.uv_FloorTex).rgb;
                if(h >= 45 && h < 60) o.Albedo = tex2D(_FloorMidHeightTex, IN.uv_FloorMidHeightTex).rgb;
                if(h >= 60) o.Albedo = tex2D(_FloorSnow, IN.uv_FloorSnow).rgb;
            }
            else{
                o.Albedo = tex2D(_WallTex, IN.uv_WallTex).rgb;
            }
            
            o.Emission = half3(0, 0, 0) * o.Albedo;
            o.Metallic = 0.5;
            o.Smoothness = 0.25;
        }
 
        ENDCG
    }

}