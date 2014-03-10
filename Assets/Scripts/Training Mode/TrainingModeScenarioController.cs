using UnityEngine;
using System.Collections;
namespace ETraining.TrainingMode
{
	/**
	 * This class does some specific tasks for training mode.
	 * Because of modeling step, one component can have many 3d meshes
	 * if we touch 1 largest mesh to enable the animation then, other meshes must follow the largest mesh 
	 * and be hidden whenever largest part hides away, be shown whenever larget part is shown. The framework based on xml scenario just works with
	 * single-meshed component, so we have to do extra things for other meshes, such as hidding them, hidding tools
	 * 
	 * The limitation is this one just works fine on sample.xml. any changes in sample xml step order, can make it wrong.
	 */ 
	public class  TrainingModeScenarioController : ScenarioController
	{
		private bool isHide = false;
		private bool isHideEn0146 = false;
		private bool isHideEn0145 = false;
		private bool isHideEn0024_01_02 = false;
		private bool isHideEn00121 = false;
		private bool isHideEn01430144 = false;
		private bool isHideEn0008 = false;
		private bool isHideEn0160_1 = false;
		public override void doExtraRequirement()
		{
			if(Application.loadedLevelName == "ReverseTrainingMode" || currentComponent == null) 
			{						
				return;
			}

			//currentComponent.Name
			if(StepOrder <= 33)
			{
				GameObject [] arrows = GameObject.FindGameObjectsWithTag("arrow");
				for (int i = 0; i < arrows.Length; i++) 
				{
					arrows[i].renderer.enabled = true;	
				}
			}
			else
			{
				GameObject [] arrows = GameObject.FindGameObjectsWithTag("arrow");
				for (int i = 0; i < arrows.Length; i++) 
				{
					arrows[i].renderer.enabled = false;	
				}
			}
			
			if(StepOrder > 6) // we need to hide more components related to manifold
			{
				if(!isHide)
				{
					GameObject hose = GameObject.Find("EN_PCV_pipe_01") as GameObject;
					hose.renderer.enabled = false;
					GameObject hose2 = GameObject.Find("EN_PCV_pipe_02") as GameObject;
					hose2.renderer.enabled = false; 
					GameObject en0012_01 = GameObject.Find("en0012_01") as GameObject;
					en0012_01.renderer.enabled = false;
					isHide = true;
				}
				
			}
			else
			{
				if(isHide)
				{
					GameObject hose = GameObject.Find("EN_PCV_pipe_01") as GameObject;
					hose.renderer.enabled = true;
					GameObject hose2 = GameObject.Find("EN_PCV_pipe_02") as GameObject;
					hose2.renderer.enabled = true; 
					GameObject en0012_01 = GameObject.Find("en0012_01") as GameObject;
					en0012_01.renderer.enabled = true;
					isHide = false;
				}
			}
			
			if(StepOrder > 9){
				if(!isHideEn0146)
				{
					GameObject.Find("en0146").renderer.enabled = false;
					isHideEn0146 = true;
				}
			}
			else
			{
				if(isHideEn0146)
				{
					GameObject.Find("en0146").renderer.enabled = true;
					isHideEn0146 = false;
				}
			}
			
			
			if(StepOrder > 10){
				if(!isHideEn0145)
				{
					GameObject.Find("en0145").renderer.enabled = false;
					isHideEn0145 = true;
				}
			}
			else
			{
				if(isHideEn0145)
				{
					GameObject.Find("en0145").renderer.enabled = true;
					isHideEn0145 = false;
				}
			}
			
			if(StepOrder > 11){
				if(!isHideEn01430144)
				{
					GameObject.Find("en0143").renderer.enabled = false;
					GameObject.Find("en0142").renderer.enabled = false;
					GameObject.Find("en0144").renderer.enabled = false;
					GameObject.Find("en0146 1").renderer.enabled = false;
					isHideEn01430144 = true;
				}
			}
			else
			{
				if(isHideEn01430144)
				{
					GameObject.Find("en0143").renderer.enabled = true;
					GameObject.Find("en0144").renderer.enabled = true;
					GameObject.Find("en0142").renderer.enabled = true;
					GameObject.Find("en0146 1").renderer.enabled = true;
					isHideEn01430144 = false;
				}
			}
			
			if(StepOrder > 12){
				if(!isHideEn00121)
				{
					GameObject.Find("en00121").renderer.enabled = false;
					isHideEn00121 = true;
				}
			}
			else
			{
				if(isHideEn00121)
				{
					GameObject.Find("en00121").renderer.enabled = true;
					isHideEn00121 = false;
				}
			}
			
			if(StepOrder > 14)
			{
				if(!isHideEn0024_01_02)
				{
					GameObject.Find("en0024_01").renderer.enabled = false;
					GameObject.Find("en0024_02").renderer.enabled = false;
					GameObject.Find("en00105").renderer.enabled = false;
					GameObject.Find("en00106").renderer.enabled = false;
					GameObject.Find("en00107").renderer.enabled = false;
					isHideEn0024_01_02 = true;
				}
			}
			else
			{
				if(isHideEn0024_01_02)
				{
					GameObject.Find("en0024_01").renderer.enabled = true;
					GameObject.Find("en0024_02").renderer.enabled = true;
					GameObject.Find("en00105").renderer.enabled = true;
					GameObject.Find("en00106").renderer.enabled = true;
					GameObject.Find("en00107").renderer.enabled = true;
					isHideEn0024_01_02 = false;
				}
			}
			if(StepOrder > 15){
				if(!isHideEn0160_1)
				{
					GameObject.Find("en0160_1").renderer.enabled = false;
					isHideEn0160_1 = true;
				}
			}
			else
			{
				if(isHideEn0160_1)
				{
					GameObject.Find("en0160_1").renderer.enabled = true;
					isHideEn0160_1 = false;
				}
			}
			if(StepOrder > 16){
				if(!isHideEn0008)
				{
					GameObject.Find("en0008_02 1").renderer.enabled = false;
					GameObject.Find("en0008_03 1").renderer.enabled = false;
					isHideEn0008 = true;
				}
			}
			else
			{
				if(isHideEn0008)
				{
					GameObject.Find("en0008_02 1").renderer.enabled = true;
					GameObject.Find("en0008_03 1").renderer.enabled = true;
					isHideEn0008 = false;
				}
			}
		
		}
	}
}	