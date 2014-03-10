using UnityEngine;
using System.Collections;

/**
 * This class is used to change the material of engine model in Transmission mode.
 * The approach is same to \ref ChangeMaterials, please refer it to understand more.
 */ 
public class ChangeMaterialsTransmission : MonoBehaviour
{
	private Material myMaterial;
	private Material myMaterial2;
	// Use this for initialization
	void Awake ()
	{
		// We tagged all game object having renderer. So just find all of them from tags
		GameObject [] engineLow = GameObject.FindGameObjectsWithTag("el");

		foreach(GameObject part in engineLow)
		{



			if( part.renderer)
			{	
					if(part.name.Contains("tr049")) continue;
				string [] name1 = part.renderer.materials[0].name.Split(" "[0]);
		
				
				if(name1[0] == "transmission0432_total_lambert58")
				{
					
						if(part.name == "tr035_13")
						{
							int debughere = 0;
						}
						myMaterial = Resources.Load("anisotropic2", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] =="blinn1")// "EN_w")
				{
					myMaterial = Resources.Load("anisotropic3", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] =="blinn2")// "EN_B")
				{
					myMaterial = Resources.Load("anisotropic6", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "transmission0432_total_lambert46")//"EN_B2" )
				{
					myMaterial = Resources.Load("anisotropic5", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
					if(name1[0] == "lambert5" || name1[0] == "lambert10")//"EN_BT" )
				{
					myMaterial = Resources.Load("anisotropic1", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
					if(name1[0] == "transmission0432_total_lambert16" )//"EN_BL")
				{
					myMaterial = Resources.Load("anisotropic4", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				if(name1[0] == "EN_BB") 
				{
					myMaterial = Resources.Load("lambert1", typeof(Material)) as Material;	
					
					//print("Assets/Engine/OriginalMaterial/"+name+".mat  " );
					//part.renderer.material = myMaterial;
				}
				
					part.renderer.material = myMaterial;
				
			}
		}	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

