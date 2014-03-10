Shader "Mobile/Legacy/Lightmap/Reflective"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		//_MainTex ("Base (RGB)", 2D) = "white" {}
		//anh nguyen -- comment light map to improve performance 'cause we dont use it
		//_LightMap ("Lightmap (RGB)", 2D) = "white" { LightmapMode }
		_Reflect ("Reflection", 2D) = "black" { TexGen SphereMap }
	}

	SubShader
	{
		
		Pass
		{
			Name "BASE"	
				
			//BindChannels {
			//	Bind "Vertex", vertex
				//anh nguyen -- comment binding light map to texture to improve performance cause we dont use it
				//Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				//Bind "texcoord", texcoord1 // main uses 1st uv
			//}
			// anh nguyen -- comment those SetTexture belows to improve performance because we dont use them
			//SetTexture [_LightMap] {
			//	combine texture
			//}
			//SetTexture [_MainTex] {
			//	combine texture * previous
			//}
		}
		
		// This pass uses vertex information to control Reflection
		Pass
		{
			Name "REFLECT"
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			Lighting Off

			BindChannels {
				//Bind "Vertex", vertex
				Bind "normal", normal
			}
						
			SetTexture [_Reflect] {
				combine texture, primary
			}
		}
		
		// anh nguyen -- comment those to improve performance 
		// Use this pass, if you want to fetch alpha from main texture instead
//		Pass
//		{
//			Name "REFLECT"
//			ZWrite Off
//			Blend SrcAlpha OneMinusSrcAlpha
//			
//			BindChannels {
//				Bind "Vertex", vertex
//				Bind "normal", normal
//				//Bind "texcoord", texcoord0 // main uses 1st uv
//			}
//			// anh - comment to save performance			
//			//SetTexture [_MainTex] {
//			//	combine texture
//			//}
//			SetTexture [_Reflect] {
//				combine texture, previous
//			}
//		}
		
	}
}
