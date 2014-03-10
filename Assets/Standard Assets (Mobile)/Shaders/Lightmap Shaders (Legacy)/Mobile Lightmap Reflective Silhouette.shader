Shader "Mobile/Legacy/Lightmap/Reflective-sh"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "white" { LightmapMode }
		_Reflect ("Reflection", 2D) = "black" { TexGen SphereMap }
		 _MainTex ("Base (RGB)", 2D) = "white" { }	
		 
		     _OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Range (.002, 0.03)) = .005
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata {
	    float4 vertex : POSITION;
	    float3 normal : NORMAL;
	};
	
	
	
	struct v2f {
	    float4 pos : POSITION;
	    float4 color : COLOR;
	};
	
	 
	
	uniform float _Outline;
	uniform float4 _OutlineColor;
	
	
	v2f vert(appdata v) {
	
	    // just make a copy of incoming vertex data but scaled according to normal direction
	    v2f o;
	    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	 
	    float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	    float2 offset = TransformViewToProjection(norm.xy);
	    o.pos.xy += offset * o.pos.z * _Outline;
	    o.color = _OutlineColor;
	    return o;
	}
	
	ENDCG
	SubShader
	{
		Tags { "Queue"="Overlay+1" }
				
        //UsePass "Specular/BASE"
        Pass 
        {
            ZTest Less 
                     
            SetTexture [_MainTex] {combine texture}
        }

        Pass
        {
            ZTest Greater
           	Lighting On
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite Off
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            ENDCG
            Color [_Color]
        }
	
//		Pass
//		{
//			Name "BASE"	
//				
//			BindChannels {
//				Bind "Vertex", vertex
//				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
//				Bind "texcoord", texcoord1 // main uses 1st uv
//			}
//			SetTexture [_LightMap] {
//				combine texture
//			}
//			SetTexture [_MainTex] {
//				combine texture * previous
//			}
//		}
		
//		/* This pass uses vertex information to control Reflection
//		Pass
//		{
//			Name "REFLECT"
//			ZWrite Off
//			Blend SrcAlpha OneMinusSrcAlpha
//			ColorMaterial AmbientAndDiffuse
//			Lighting Off
//
//			BindChannels {
//				Bind "Vertex", vertex
//				Bind "normal", normal
//			}
//						
//			SetTexture [_Reflect] {
//				combine texture, primary
//			}
//		}*/
		
		// Use this pass, if you want to fetch alpha from main texture instead
		Pass
		{
			Name "REFLECT"
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			BindChannels {
				Bind "Vertex", vertex
				Bind "normal", normal
				Bind "texcoord", texcoord0 // main uses 1st uv
			}
						
			SetTexture [_MainTex] {
				combine texture
			}
			SetTexture [_Reflect] {
				combine texture, previous
			}
		}
		
	}
}
