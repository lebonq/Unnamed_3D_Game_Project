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
            float3 worldPosition;
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
            
            float3 vFloor;
            float3 vWall;

            float2 uv = IN.uv_FloorTex;

            if(IN.worldPosition.y < 100)vFloor = tex2D(_FloorTex, uv).rgb;
            if(IN.worldPosition.y > 100)vFloor = tex2D(_FloorMidHeightTex, uv).rgb;
            //vFloor = tex2D(_FloorSnow, uv).rgb;

            vWall = tex2D(_WallTex, uv).rgb;

            if(IN.worldNormal.y < 0.81 && IN.worldNormal.y > 0.79){
                o.Albedo = lerp(vFloor, vWall, uv.x);
            }
            else if (IN.worldNormal.y < 0.79){
                o.Albedo = vWall;
            }
            else{
                o.Albedo = vFloor;
            }
            
            o.Emission = half3(0, 0, 0) * o.Albedo;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
        }
 
        ENDCG
    }

}