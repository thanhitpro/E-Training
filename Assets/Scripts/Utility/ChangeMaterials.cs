using UnityEngine;
using System.Collections;
/**
 * This class is used to change the material of engine model in Training mode.
 * Because the orgininal model has no meaningful material, it just has some materials
 * call EN_ (EN_BT, EN_W2...) from 3D max and those materials do zero contribution to shading process of engine.
 * So, we have to define our own materials and manual test which EN_ could be replaced by a specific materials from ours.
 * After having a good looking engine with our materials, we make an automatic process of materials assignment to replace EN_
 * materials.
 * 
 * Our designed materials have the names starting with anisotropic + number like anisotropic1, anisotropic2, etc...
 * We also design material for silhouette effect and name them anisotropic + number + "-sh" like, anisotropic1-sh, 
 * anisotropic2-sh...
 */ 
public class ChangeMaterials : MonoBehaviour
{
	#if UNITY_STANDALONE_WIN
	private Material myMaterial;
	private Material myMaterial2;
	// Use this for initialization

	void Start ()
	{
		// We tagged all game object having renderer. So just find all of them from tags
		GameObject [] engineLow = GameObject.FindGameObjectsWithTag("el");

		foreach(GameObject part in engineLow)
		{
			if( part.renderer)
			{	string [] name1 = part.renderer.materials[0].name.Split(" "[0]);
		
				string [] name2 = new string[1];
				if(part.renderer.materials.Length > 1)
				{
					name2 = part.renderer.materials[1].name.Split(" "[0]);
				}
				
				
				
				if(name2.Length == 1 && name2[0] == "EN_w2")
				{
					
					myMaterial2 = Resources.Load("anisotropic2", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_w")
				{
					
					myMaterial2 = Resources.Load("anisotropic3", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_B")
				{
					
					myMaterial2 = Resources.Load("anisotropic6", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_B2")
				{
					
					myMaterial2 = Resources.Load("anisotropic5", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_BT")
				{
					
					myMaterial2 = Resources.Load("anisotropic1", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_BL")
				{
					
					myMaterial2 = Resources.Load("anisotropic4", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name2.Length == 1 && name2[0] == "EN_BB") // bell
				{
					
					myMaterial2 = Resources.Load("lambert1", typeof(Material)) as Material;	
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				//print("ten cua mat " + name[0]);
				if(name1[0] == "EN_w2")
				{
					myMaterial = Resources.Load("anisotropic2", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_w")
				{
					myMaterial = Resources.Load("anisotropic3", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_B")
				{
					myMaterial = Resources.Load("anisotropic6", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_B2" )
				{
					myMaterial = Resources.Load("anisotropic5", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_BT" )
				{
					myMaterial = Resources.Load("anisotropic1", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_BL")
				{
					myMaterial = Resources.Load("anisotropic4", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_BB") // bell
				{
					myMaterial = Resources.Load("lambert1", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}


				// if component has many materials, just do array materials assignment.
				if(part.renderer.materials.Length == 2)
				{
				  	Material []arrM = new Material[2];
				  	arrM[0] = myMaterial;
				  	arrM[1] = myMaterial;
				  	part.renderer.materials = arrM;				
				}
				else if(part.renderer.materials.Length == 1)
				{
					part.renderer.material = myMaterial;
				}
				
			}
		}

	}
	#endif
}

