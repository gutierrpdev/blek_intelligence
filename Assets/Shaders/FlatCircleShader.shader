﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FlatCircleShader" {
	Properties{
		_Color("Color", Color) = (1,0,0,0)
		_Thickness("Thickness", Range(0.0,0.5)) = 0.05
		_Radius("Radius", Range(0.0, 0.5)) = 0.4
		_Dropoff("Dropoff", Range(0.01, 4)) = 0.1
		_Alpha("Alpha", Range(0.0, 1.0)) = 1.0
	}
		SubShader{
		 Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		 Blend SrcAlpha OneMinusSrcAlpha
		 ZWrite Off
			Pass {
				//Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"


				fixed4 _Color; // low precision type is usually enough for colors
			   float _Thickness;
			   float _Radius;
			   float _Dropoff;
			   float _Alpha;

				struct fragmentInput {
					float4 pos : SV_POSITION;
					float2 uv : TEXTCOORD0;
				};

				fragmentInput vert(appdata_base v)
				{
					fragmentInput o;

					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord.xy - fixed2(0.5,0.5);

					return o;
				}

				// r = radius
				// d = distance
				// t = thickness
				// p = % thickness used for dropoff
				float antialias(float r, float d, float t, float p) {
					if (d < (r - 0.5*t))
					   return -pow(d - r + 0.5*t,2) / pow(p*t, 2) + 1.0;
				   else if (d > (r + 0.5*t))
					   return -pow(d - r - 0.5*t,2) / pow(p*t, 2) + 1.0;
				   else
					   return 1.0;
				}

				fixed4 frag(fragmentInput i) : SV_Target {
				   float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y,2));
				   return fixed4(_Color.r, _Color.g, _Color.b, _Alpha*antialias(_Radius, distance, _Thickness, _Dropoff));
				}


				ENDCG
			}
	}
}