Shader "Hidden/GT/Vertex"
{
	Properties
	{
		_Scale("Scale", Range(1,7)) = 2.0
	}

	SubShader
	{
		Tags { "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True" "Queue" = "Overlay" }
		Lighting Off
		ZTest LEqual
		ZWrite On
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
			AlphaTest Greater .25

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Scale;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MV, v.vertex);
				//o.pos.xyz *= .95;
				o.pos = mul(UNITY_MATRIX_P, o.pos);

				o.pos.xy /= o.pos.w;
				o.pos.xy = o.pos.xy * .5 + .5;
				o.pos.xy *= _ScreenParams.xy;

				o.pos.xy += v.texcoord1.xy * _Scale;

				o.pos.xy /= _ScreenParams.xy;
				o.pos.xy = (o.pos.xy - .5) / .5;
				o.pos.xy *= o.pos.w;

				o.color = v.color;

				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				return i.color;
			}

			ENDCG
		}
	}
}