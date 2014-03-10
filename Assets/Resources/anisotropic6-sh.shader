Shader "Custom/anisotropic6-sh" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_Shininess ("Shininess", Range (0.01, 3)) = 1.5
	_Gloss("Gloss", Range (0.00, 1)) = .5
	_Reflection("Reflection", Range (0.00, 1)) = 0.5
	_Cube ("Reflection Cubemap", Cube) = "Black" { TexGen CubeReflect }
	_FrezPow("Fresnel Reflection",Range(0,2)) = .25
	_FrezFalloff("Fresnal/EdgeAlpha Falloff",Range(0,10)) = 4
	_EdgeAlpha("Edge Alpha",Range(0,1)) = 0
	_Metalics("Metalics",Range(0,1)) = .5
	
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
	SubShader {
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
		Tags {"Queue"="Geometry" "RenderType"="Opaque" "IgnoreProjector"="False" }
		
		UsePass "Hidden/Hardsurface Pro Front Opaque/FORWARD"
		
	} 
	
		Fallback "Diffuse"
	}
