﻿Shader "Futilef/Mutiply" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
	}

	SubShader {
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		
		// Multiply
		Blend DstColor Zero
		// Additive Multiply
		// Blend DstColor One

		Pass {
		CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment SpriteFrag

			#include "UnityCG.cginc"

			fixed4 _Color;

			struct appdata_t {
				float4 vertex   : POSITION;
				float4 color	: COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex   : SV_POSITION;
				fixed4 color	: COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f SpriteVert(appdata_t i) {
				v2f o;

				o.vertex = UnityObjectToClipPos(i.vertex);
				o.texcoord = i.texcoord;
				o.color = i.color * _Color;

				return o;
			}

			sampler2D _MainTex;

			fixed4 SpriteFrag(v2f i) : SV_Target {
				fixed4 c = tex2D(_MainTex, i.texcoord) * i.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
