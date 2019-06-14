// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CenterBlur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

		CGINCLUDE

#include "UnityCG.cginc"

		sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _MainTex_TexelSize;

	half _BlurRadius;
	half _CenterRadius;
	float totalWeight;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float4 uv : TEXCOORD0;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);

		o.uv.xy = v.uv;
		float2 uv1 = o.uv.xy;
		o.uv.zw = o.uv.xy;

#if UNITY_UV_STARTS_AT_TOP  
		if (_MainTex_TexelSize.y < 0)
			o.uv.w = 1 - o.uv.w;
#endif 
		return o;
	}

	half GetWeight(float x, float y, float rho)
	{
		return exp(-(x*x + y * y) / (2.0f*rho*rho));
	}

	fixed4 GaussBlur(v2f i)
	{
		fixed4 finalColor = fixed4(0, 0, 0, 0);
		half rho = _BlurRadius / 3.0f;

		for (int x = -_BlurRadius; x <= _BlurRadius; x++)
		{
			for (int y = -_BlurRadius; y <= _BlurRadius; y++)
			{
				half wt = GetWeight(x, y, rho) / totalWeight;

				fixed4 col = tex2D(_MainTex, i.uv + float2(x, y) * _MainTex_TexelSize.xy);
				finalColor += col * wt;
			}
		}

		return finalColor;
	}

	ENDCG

		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		//Pass1
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 texCol = tex2D(_MainTex, i.uv);
				fixed4 blur = GaussBlur(i);

				half radius = length(i.uv - fixed2(0.5f,0.5f));
				half dis = max(0, min(1, _CenterRadius - radius));

				fixed4 finalCol = lerp(blur, texCol, dis);

				return finalCol;
			}
			ENDCG
		}

		//Pass2
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			int showFactor = 0;

			sampler2D _BlurTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 texCol = tex2D(_MainTex, i.uv.xy);
				fixed4 blur = tex2D(_BlurTex, i.uv.zw);

				half radius = length(i.uv - fixed2(0.5f,0.5f));
				half factor = max(0, min(1, _CenterRadius - radius));

				fixed4 finalCol = lerp(blur, texCol, factor);
				fixed4 factorCol = fixed4(1,1,1,1) * factor;
				finalCol = lerp(finalCol, factorCol, showFactor);

				return finalCol;
			}
			ENDCG
		}
	}
}