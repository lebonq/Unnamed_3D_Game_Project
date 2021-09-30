/*
Copyright(c) 2017 Untitled Games
Written by Chris Bellini

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files(the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

Shader "Untitled Games/Examples/Linear Lerp"
{
	Properties 
	{
		_Texture1("Texture 1", 2D) = "white" {}
		_Texture2("Texture 2", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert fullforwardshadows
		#pragma target 3.0

		sampler2D _Texture1;
		sampler2D _Texture2;

		struct Input 
		{
			float2 uv_Texture1;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 uv = IN.uv_Texture1;

			float3 v1 = tex2D(_Texture1, uv).rgb;
			float3 v2 = tex2D(_Texture2, uv).rgb;
			float t = uv.x;

			o.Albedo = lerp(v1, v2, t);
		}

		ENDCG
	}

	FallBack "Diffuse"
}
