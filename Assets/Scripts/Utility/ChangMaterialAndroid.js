#if UNITY_ANDROID || UNITY_IPHONE
function Start () {

	var myMaterial : Material;	
	
	var anisotropic2 : Material;	
	var anisotropic3 : Material;	
	var anisotropic6 : Material;	
	var anisotropic5 : Material;	
	var anisotropic1 : Material;	
	var anisotropic4 : Material;	
	var lambert1 : Material;	
	
	// This for new material anisotropic 2 with much brighter and reflection
	//anisotropic2 = Resources.Load("Android Shading/anisotropic2-extra", typeof(Material)) as Material;
	anisotropic2 = Resources.Load("Android Shading/anisotropic2", typeof(Material)) as Material;
	anisotropic3 = Resources.Load("Android Shading/anisotropic3", typeof(Material)) as Material;	
	anisotropic6 = Resources.Load("Android Shading/anisotropic6", typeof(Material)) as Material;	
	anisotropic5 = Resources.Load("Android Shading/anisotropic5", typeof(Material)) as Material;	
	anisotropic1 = Resources.Load("Android Shading/anisotropic1", typeof(Material)) as Material;	
	anisotropic4 = Resources.Load("Android Shading/anisotropic4", typeof(Material)) as Material;	
	lambert1 = Resources.Load("Android Shading/lambert1", typeof(Material)) as Material;	
	var engineLow = GameObject.FindGameObjectsWithTag("el");	
	
	
	
	for (var part : GameObject in engineLow)
	{
		if( part.renderer)
		{	var name1 = part.renderer.materials[0].name.Split(" "[0]);
		
		
			//print("ten cua mat " + name[0]);
			if(name1[0] == "EN_w2")
			{
			
				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				part.renderer.sharedMaterial = anisotropic2;
			}
			if(name1[0] == "EN_w")
			{

				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				part.renderer.sharedMaterial = anisotropic3;
			}
			if(name1[0] == "EN_B")
			{
		
				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				
				part.renderer.sharedMaterial = anisotropic6;
			}
			if(name1[0] == "EN_B2")
			{
				
				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				part.renderer.sharedMaterial = anisotropic5;
			}
			if(name1[0] == "EN_BT")
			{
				
				part.renderer.sharedMaterial = anisotropic1;
			}
			if(name1[0] == "EN_BL")
	
				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				part.renderer.sharedMaterial = anisotropic4;
			}
			if(name1[0] == "EN_BB") // bell
			{
	
				//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
				part.renderer.sharedMaterial = lambert1;
			}
		}
	}
#endif