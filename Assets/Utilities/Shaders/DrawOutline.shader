Shader "Custom/DrawOutline"
{
	Properties
	{
		_MainTex("Main Texture",2D) = "black"{}
		_SceneTex("Scene Texture",2D) = "black"{}
		_Color("Outline Color",Color) = (0,1,0,1)
		_Width("Outline Width",int) = 4
		_Iterations("Iterations",int) = 3
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" }
			Pass
			{
				ZTest Always Cull Off ZWrite Off

				CGPROGRAM

				sampler2D _MainTex;
				float2 _MainTex_TexelSize;
				sampler2D _SceneTex;
				fixed4 _Color;
				float _Width;
				int _Iterations;

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord.xy;
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					int iterations = _Iterations * 2 + 1;
					float ColorIntensityInRadius;
					float TX_x = _MainTex_TexelSize.x * _Width;
					float TX_y = _MainTex_TexelSize.y * _Width;

					float r = iterations / 2;

					for (int k = 0; k < iterations; k += 1)
						for (int j = 0; j < iterations; j += 1)
							if ((k - r)*(k - r) + (j - r)*(j - r) <= r * r)
								ColorIntensityInRadius += tex2D(_MainTex, i.uv.xy + float2((k - r) * TX_x,(j - r) * TX_y));

					if (tex2D(_MainTex,i.uv.xy).r > 0 || ColorIntensityInRadius == 0)
						return tex2D(_SceneTex, i.uv);
					else
						return _Color.a * _Color + (1 - _Color.a) * tex2D(_SceneTex, i.uv);
				}
				ENDCG
			}

		}
}