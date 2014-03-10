Shader "Texture With ZTest" 
{
    Properties 
    {
        _Color ("Main Color", Color) = (1,1,1,0.5)
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "red" {}
    }
 
    Category 
    {
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
                Lighting Off
                Color [_Color]
            }
        }
    }
 
    FallBack "HardSurface/Hardsurface Free/Opaque", 1
}