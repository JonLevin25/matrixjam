/* Taken and Modified From http://www.shaderslab.com/demo-75---matrix-pattern.html */
Shader "Custom/Team19/Numbers Falling 2"
{
	Properties
	{
		_GridX("Grid X", range(1, 50.)) = 30.
		_GridY("Grid Y", range(1, 50.)) = 30.

		_SpeedMax("Speed Max", range(0, 30.)) = 20.
		_SpeedMin("Speed Min", range(0, 10.)) = 2.
		_Density("Density", range(0, 30.)) = 5.
		_BacklightStartValue("Backlight Start Value", range(0., 1.)) = 0.2
		_BackgroundColor("Background Color", Color) = (0.0, 0.0, 0.0)
		_RegularColor("Regular Color", Color) = (1.0,1.0,1.0)
		_UniqueColor("Unique Color", Color) = (1.0, 0.0, 0.0)
		_BacklightColor("Backlight Color", Color) = (0.2, 1.0, 0.2)
		_BacklightFalloff ("Backlight Falloff", range(1., 10.)) = 1.
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"


			float noise(float x)
			{
				x += 0.2127 * tan(x) + x;
				float2 r = (123.789)*sin(1.823*(x));
				return frac(r.x*r.y);
			}

			float noise(float2 vect)
			{
				vect += 0.2127*cos(vect.x) + vect.x + 0.3713*vect.y;
				float2 r = (123.789)*sin(1.823*(vect));
				return frac(r.x*r.y);
			}

			float texelValue(float2 ipos, float n) {
				for (float i = 0.; i < 5.; i++) {
					for (float j = 0.; j < 3.; j++)
					{
						if (i == ipos.y && j == ipos.x) {
							return step(1., fmod(n, 2.));
						}

						n = ceil(n / 2.);
					}
				}
				return 0.;
			}

			float _Density;

			float char(float2 st, float n) {
					st.x = st.x * 2. - .5;
					st.y = st.y * 1.2 - .1;

					float2 ipos = floor(st * float2(3., 5.));

					n = floor(fmod(n, 20. + _Density));

					float digit = 0.0;

					if (n < 1.) { digit = 9712.; }
					else if (n < 2.) { digit = 21158.0; }
					else if (n < 3.) { digit = 25231.0; }
					else if (n < 4.) { digit = 23187.0; }
					else if (n < 5.) { digit = 23498.0; }
					else if (n < 6.) { digit = 31702.0; }
					else if (n < 7.) { digit = 25202.0; }
					else if (n < 8.) { digit = 30163.0; }
					else if (n < 9.) { digit = 18928.0; }
					else if (n < 10.) { digit = 23531.0; }
					else if (n < 11.) { digit = 29128.0; }
					else if (n < 12.) { digit = 17493.0; }
					else if (n < 13.) { digit = 7774.0; }
					else if (n < 14.) { digit = 31141.0; }
					else if (n < 15.) { digit = 29264.0; }
					else if (n < 16.) { digit = 3641.0; }
					else if (n < 17.) { digit = 31315.0; }
					else if (n < 18.) { digit = 31406.0; }
					else if (n < 19.) { digit = 30864.0; }
					else if (n < 20.) { digit = 31208.0; }
					else { digit = 1.0; }

					float tex = texelValue(ipos, digit);

					float2 borders = float2(1., 1.);
					borders *= step(0., st) * step(0., 1. - st);

					return step(.1, 1. - tex) * borders.x * borders.y;
			}

			float _GridX;
			float _GridY;
			float _SpeedMax;
			float _SpeedMin;
			float _BacklightStartValue;
			float _BacklightFalloff;
			float3 _BackgroundColor;
			float3 _RegularColor;
			float3 _UniqueColor;
			float3 _BacklightColor;
			sampler2D _SymbolsTexture;

			struct v2f {
				float4 pos: SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			fixed4 frag(v2f i) : SV_Target
			{

				const float2 grid = float2(_GridX, _GridY);

				float2 ipos = floor(i.uv * grid);
				float2 fpos = frac(i.uv * grid);

				ipos.y += floor(_Time.y * max(_SpeedMin, _SpeedMax * noise(ipos.x)));
				float charNum = noise(ipos);
				float unique_bias = char(fpos, 100.0 * charNum);
				float val = char(fpos, (20.) * charNum);


				float3 color = lerp(_BackgroundColor, _RegularColor, val);

				color.rgb = lerp(color.rgb, _UniqueColor, unique_bias);
				
				color.rgb *= (1.0 - i.uv.y);

				float backlight_value = clamp(_BacklightStartValue - i.uv.y, 0., 1.);
				color.rgb += pow(backlight_value * _BacklightColor, _BacklightFalloff);

				return fixed4(color.rgb , 1.0);
			}

			ENDCG
		}
	}
}