// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _World2Object', a built-in variable

Shader "Custom/test" {
	Properties {
	//_Height("Height Slicer", Range(-2.0, 3.2)) = 3.2
	_Height("Height Slicer", Range(0, 5)) = 2
	
	_Cube ("Reflection Cubemap", CUBE) = "Black" { TexGen CubeReflect }

	
	}

	SubShader {
	    Pass {
	    
	    
	Cull off
	
	CGPROGRAM
	#pragma vertex mainVS
	#pragma fragment slicerPS
	
	#include "UnityCG.cginc"
	
	float _Height;
	uniform samplerCUBE _Cube;
	
	// for reflection
	 //uniform float4 unity_Scale; // w = 1/scale; see _World2Object
     // uniform float3 _WorldSpaceCameraPos;
     // uniform float4x4 _Object2World; // model matrix
     // uniform float4x4 _World2Object; // inverse model matrix 
	//end of reflection
	
	
	struct appdata {
    float3 Position	: POSITION;
    float4 UV		: TEXCOORD0;
    float4 Normal	: NORMAL;
    float4 Tangent	: TANGENT0;
  	float3 Binormal	: BINORMAL0;
  	
  	
	};
	
	struct sliceVertexOutput {
    float4 HPosition	: POSITION;
	float2 UV			: TEXCOORD0;
    //float4 TexCoord		: TEXCOORD1;
    float3 LightVec		: TEXCOORD1;
    float3 WorldNormal	: TEXCOORD2;
    float4 WorldPos		: TEXCOORD3;
    float3 WorldView	: TEXCOORD4;
    float4 SlicePos		: TEXCOORD5;
	float3 WorldTangent	: TEXCOORD6;
    float3 WorldBinormal: TEXCOORD7;
    
     //float4 pos : SV_POSITION;
	};
	
	
	sliceVertexOutput mainVS(appdata IN,
    uniform float4x4 WorldITXf, // our four standard "untweakable" xforms
	uniform float4x4 WorldXf,
	uniform float4x4 ViewIXf,
	uniform float4x4 WvpXf,
    uniform float4x4 SlicerXf,
    uniform float4 LampPos,
	uniform float4 camerapos
	)
	{
		//WorldITXf =  UNITY_MATRIX_IT_MV;
		//WorldXf = _Object2World ; // no lam cho slicer xuat hien curve 
	  	//ViewIXf =  UNITY_MATRIX_IT_MV;
		WvpXf =  UNITY_MATRIX_MVP;	/**/
		SlicerXf = float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
		LampPos = (0.8337898,-1, 1);
		//camerapos = (0.8337898,2.0,-0.7206879);
	
	    sliceVertexOutput OUT = (sliceVertexOutput)0;
	    OUT.WorldNormal = mul(WorldITXf, IN.Normal).xyz;
	    OUT.WorldTangent = mul(WorldITXf, IN.Tangent).xyz;
	   
	     float3 binormal = cross( IN.Normal, IN.Tangent.xyz)* IN.Tangent.w;
	    float4 binormal4 = float4(binormal, 1);
	  	OUT.WorldBinormal = mul(WorldITXf, binormal4).xyz; //minh comment cai nay lai thi shader nay compile duoc. mai nho coi.
	   
	   //IN.Position.y -= 0.4;
	     float4 Po = float4(IN.Position.xyz,1);
	   	float4 Pw = mul(WorldXf, Po);
	    float4 Ps = mul(SlicerXf, Po);
		
	    OUT.WorldPos = Pw;
	    OUT.SlicePos = Ps;
	    OUT.LightVec = normalize( LampPos.xyz - Pw.xyz );
		OUT.UV = IN.UV.xy;
	    //OUT.TexCoord = IN.UV;
		
	    OUT.WorldView = normalize(camerapos.xyz - Pw.xyz);
	    OUT.HPosition = mul(WvpXf, Pw);
	
	/*
		//for reflection
		  float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            OUT.LightVec = float3(mul(modelMatrix, IN.Position) 
               - float4(_WorldSpaceCameraPos, 1.0));
            OUT.UV = normalize(float3(
               mul(float4(IN.Normal, 0.0), modelMatrixInverse)));
            OUT.pos = mul(UNITY_MATRIX_MVP, IN.Position);
		//end of reflection
		*/
	
	    return OUT;
	}

	/*********** pixel shader ******/

	float4 slicerPS(sliceVertexOutput IN,
		uniform float3 SurfaceColor,
		    uniform float Kd,
		    uniform float SpecExpon,
		    uniform float Kr,
		    uniform samplerCUBE EnvSampler,
		    uniform float3 LampColor,
		    uniform float3 AmbiColor
	) :COLOR
	{
	
		SurfaceColor	= float3(0.35, 0.35, 0.35);
		Kd				= 0.6;
		SpecExpon		= 3.0;
		Kr				= 1.8;
		LampColor		= float3(0.6, 0.6, 0.6);
		AmbiColor		= float3(0.6, 0.6, 0.6);
		//EnvSampler		= texCUBE( _Cube);
		
	
		float3 diffContrib;
	    float3 specContrib;
	    float3 Ln = normalize(IN.LightVec.xyz);
	    float3 Nn = normalize(IN.WorldNormal);
	
	
	    float3 Vn = normalize(IN.WorldView);
	
	
	
	
	    float3 Hn = normalize(Vn + Ln);
	    float4 litV = lit(dot(Ln,Nn),dot(Hn,Nn),SpecExpon);
	    diffContrib = litV.y * Kd * LampColor + AmbiColor;
	    specContrib = litV.z * LampColor;
	    float3 reflVect = -reflect(Vn,Nn);
	    
	    // reflectoin
	     //float3 reflectedDir = reflect(IN.LightVec, normalize(IN.UV));
	    //endof relficetion
	    
	    float3 ReflectionContrib = Kr * texCUBE(_Cube,reflVect).rgb;
	    specContrib += ReflectionContrib;
	
	    float3 result = SurfaceColor * (specContrib + diffContrib);
	    
		clip(-IN.SlicePos.y  + _Height);
	   // return float4(1,0,0,1);
		return float4(result,0);
	}
	

	ENDCG
	
	    }
	}

	Fallback "VertexLit"
}
