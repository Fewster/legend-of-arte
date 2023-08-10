Shader "Cg shading in world space" 
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_CutoutPosition("Cutout Position", Vector) = (0,0,0)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
		  CGPROGRAM

		  #pragma vertex vert  
		  #pragma fragment frag 

		  // uniform float4x4 unity_ObjectToWorld; 
			 // automatic definition of a Unity-specific uniform parameter

		  struct vertexInput
		  {
			 float4 vertex : POSITION;
			 float2 texcoord : TEXCOORD0;
		  };

		  struct vertexOutput
		  {
			 float4 pos : SV_POSITION;
			 float4 world : TEXCOORD0;
			 float2 texcoord : TEXCOORD1;
		  };

		  vertexOutput vert(vertexInput input)
		  {
			vertexOutput output;

			output.texcoord = input.texcoord;
			output.pos = UnityObjectToClipPos(input.vertex);
			output.world = mul(unity_ObjectToWorld, input.vertex);

			// transformation of input.vertex from object 
			// coordinates to world coordinates;
			return output;
		  }

		  float4 _CutoutPosition;
		  sampler2D _MainTex;
		  sampler2D _AlphaTex;

		  fixed4 SampleSpriteTexture(float2 uv)
		  {
			  fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
			  fixed4 alpha = tex2D(_AlphaTex, uv);
			  color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
#endif

			  return color;
		  }

		   float4 frag(vertexOutput input) : COLOR
		   {
			   float dist = distance(input.world, _CutoutPosition.xyz);
			   // computes the distance between the fragment position 
			   // and the origin (the 4th coordinate should always be 
			   // 1 for points).

		   float3 up = float3(0, 1, 0);
		   float3 dir = normalize(_CutoutPosition.xyz - input.world);

				if (input.world.y > _CutoutPosition.y)
			//if ((dot(dir, up) > 0.4) && dist < _CutoutPosition.w)
				{
					discard;
					return float4(0,0,0,0);
				}
				else
				{
					float4 c = tex2D(_MainTex, input.texcoord);
					clip(c.a - 0.2);
					return c;
				}
		    }

ENDCG
		}
	}
}