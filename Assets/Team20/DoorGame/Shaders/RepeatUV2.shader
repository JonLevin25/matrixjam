Shader "Sprites/RepeatUV"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_ScaleUVX("Scale UV x", Range(0.01,10.0)) = 1.0 
		_ScaleUVY("Scale UV y", Range(0.01,10.0)) = 1.0
		_OffsetUVX("Offset UV x", Range(-1.0,1.0)) = 0.0
		_OffsetUVY("Offset UV y", Range(-1.0,1.0)) = 0.0
		_PrespectiveShift("PerspectiveShift", Range(-10.0,10.0)) = 0.0
		_ShiftY("Shift Y", Int) = 0
		_LeftZ("Left Z", Float) = 0
		_RightZ("Right Z", Float) = 0
		_TransparentZ("Transparent Z", Float) = 0
		_UseTransparentZ("Use Transparent Z", int) = 0
		_UseShaderZ("Use Shader Z", Int) = 1
		_FlipDirection("Flip Direction", Int) = 0
		_RepeatBoth("_Repeat Both", Int) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
				"DisableBatching" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite On
			ZTest On
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					uint vertexId : SV_VertexID;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float opacityDepth : TEXCOORD1;
				};

				fixed4 _Color;
				float _ScaleUVX;
				float _ScaleUVY;
				float _OffsetUVX;
				float _OffsetUVY;
				float _PrespectiveShift;
				bool _ShiftY;
				float _LeftZ;
				float _RightZ;
				int _UseShaderZ;
				float _TransparentZ;
				bool _FlipDirection;
				bool _UseTransparentZ;
				bool _RepeatBoth;

				v2f vert(appdata_t IN)
				{
					v2f OUT;

					float3 worldScale = float3(
						length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
						length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
						length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
						);

					float leftZ = _LeftZ;
					float rightZ = _RightZ;
					float perspetiveShift = _PrespectiveShift;
					float aspect = worldScale.x / worldScale.y;
					if (_FlipDirection)
					{
						rightZ = _LeftZ;
						leftZ = _RightZ;
						//perspetiveShift = -perspetiveShift;
					}

					uint vid = IN.vertexId % 4;
					if (_ShiftY)
					{
						if (vid == 0 || vid == 2)
						{
							IN.vertex.y += perspetiveShift * aspect;
							if (_UseShaderZ) IN.vertex.z = leftZ;
						}
						else
						{
							IN.vertex.y -= perspetiveShift * aspect;
							if (_UseShaderZ) IN.vertex.z = rightZ;
						}
					}
					else 
					{
						if (vid < 2)
						{
							IN.vertex.x += perspetiveShift / aspect;
							if (_UseShaderZ) IN.vertex.z = leftZ;
						}
						else
						{
							IN.vertex.x -= perspetiveShift / aspect;
							if (_UseShaderZ) IN.vertex.z = rightZ;
						}
					}

					OUT.opacityDepth = UnityObjectToClipPos(float4(IN.vertex.x, IN.vertex.y, _TransparentZ, 1.0)).z;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					OUT.texcoord.x *= worldScale.x * _ScaleUVX;
					if (_RepeatBoth)
					{
						OUT.texcoord.y *= worldScale.y * _ScaleUVY;
					}
					else
					{
						OUT.texcoord.y *= _ScaleUVY;
					}
					
					OUT.texcoord.x += _OffsetUVX;
					OUT.texcoord.y += _OffsetUVY;

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				void frag(v2f IN, out half4 c : COLOR, out float depth : DEPTH)
				{
					c = SampleSpriteTexture(IN.texcoord) * IN.color;
					c.rgb *= c.a;
					if (_UseTransparentZ && c.a < 0.01)
					{
						depth = IN.opacityDepth;
					}
					else 
					{
						depth = IN.vertex.z;
					}
				}
			ENDCG
			}
		}
}