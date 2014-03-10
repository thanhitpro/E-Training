using System.Collections;
using UnityEngine;

namespace ETraining
{
	/**
	 * This class represents each component of engine model in project.
	 * The method is related to material setting, \TouchEnableAnimation script
	 * and Box collider of it
	 */ 
	public class EngineComponent  {
		
		private string name; /**< Engine component name.*/
		private GameObject comp; /**< The real component model that this class represents.*/
		private string []originalMaterial; /**< The array to save the name of the original .*/
		private BoxCollider boxCollider; /**< The box collider to cover the component, so touch event can work.*/
		private string id; /**< The id of engine component.*/
		private bool hideAfterDisassembled; /**< The property that shows we will hide this component after disassembling or don't hide it.*/

		/**
		 * Get and set name
		 */
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}

	   
		/**
		 * Get and set hideAfterDisassembled
		 */
		public bool HideAfterDisassembled {
			get {
				return this.hideAfterDisassembled;
			}
			set {
				hideAfterDisassembled = value;
			}
		}
		/**
		 * Constructor
		 * @param name : Name of component (The technical name, eg. plier, oil pen...)
		 * @param id : The name of 3D mesh of that component (eg. en0003_01...)
		 * @param hideComponent : Decide whether this component is hidden after working with it
		 */
	    public EngineComponent(string name, string id, bool hideComponent)
	    {
	        this.name = name;
	        this.id = id;
			this.hideAfterDisassembled = hideComponent;	
			
			try {
				// Find component with its model name (id)
				comp = GameObject.Find(id) as GameObject;

				// If component is not a renderer, and it has child, keep finding its child's Transform, and get GameObject from child
				// Transform
				if(!comp.renderer && comp.transform.childCount > 0)
					comp = comp.transform.FindChild(id).gameObject;
			} catch (System.Exception ex) {
				Debug.Log(ex.Message + " [ " + id + " ]");
				throw(ex);
			}
			int numOfMat = 0;
			try {		
					numOfMat = comp.renderer.materials.Length;	
			} catch (System.Exception ex) {
				Debug.Log(ex.Message + " [ " + id + " ]");
				throw(ex);
			}

			// Save the original for later restore the original material
			originalMaterial = new string[numOfMat];

			// Loop all materials, get their name and assign, because the ".name" will get much more than we expect
			// it returns realname + " (Instance)", so we replace unwanted content with empty string ""
			for(int i = 0; i < originalMaterial.Length; i++) originalMaterial[i] = comp.renderer.materials[i].name.Replace(" (Instance)","");

			// Add BoxCollider if not available
			if(comp.GetComponent(typeof(BoxCollider)) == null)
			{
				boxCollider = comp.AddComponent<BoxCollider>();
			}
			else boxCollider = comp.GetComponent(typeof(BoxCollider)) as BoxCollider;

			//first, we disable BoxCollider because dont want it to receive touch event.
			boxCollider.enabled = false;
	    }

		/**
		 * The method is to set component become a little red, when component is focused.
		 * New having-red material = take the main color of original material, set it to red.
		 * The Silhouette material name = original material name + "-sh", both are in resource folder.
		 */ 
		public void setRed()
		{		
			int numOfMat = comp.renderer.materials.Length;		
			Material [] arrMat = new Material[numOfMat];

			for(int i = 0; i < numOfMat; i++) 
			{
				if(!originalMaterial[i].Contains("-sh"))
					arrMat[i] = Resources.Load(originalMaterial[i], typeof (Material)) as Material;
				else
					arrMat[i] = Resources.Load(originalMaterial[i].Replace("-sh",""), typeof (Material)) as Material;
			}

			// For assign multiple materials, we dont assign individual material util all materials are assigned.
			// Because it doesnot work, even logical assignment is nothing wrong. ( Hey Unity ! Thumbs down ..)
			// We better make an array and assign whole array of materials once.
			// http://answers.unity3d.com/questions/8993/change-material-of-an-object.html
			comp.renderer.materials = arrMat;

			// Change the color (a part of material) of each material
			for(int i = 0; i < numOfMat; i++) 
			{
				comp.renderer.materials[i].color = new Color(1.0f, 0.25f, 0.25f); 
			}
		}	

		/**
		 * The method is to set material to Red and have Silhouette effect (Look through...)
		 * New Silhouette and red material = take the main color of Silhouette material (prepared in resource folder), set it to red.
		 * The Silhouette material name = original material name + "-sh", both are in resource folder.
		 */ 
		public void setRedSilhouette()
		{
			if(!comp.renderer) return;
			int numOfMat = comp.renderer.materials.Length;

			Material [] arrMat = new Material[numOfMat];
			for(int i = 0; i < numOfMat; i++) 
			{	
				//#if UNITY_STANDALONE_WIN
				if(!originalMaterial[i].Contains("-sh"))
				{
					arrMat[i] = Resources.Load(originalMaterial[i]+"-sh", typeof (Material)) as Material;
				}
				else arrMat[i] = Resources.Load(originalMaterial[i], typeof (Material)) as Material;
				//#elif UNITY_ANDROID
				//	arrMat[i] = Resources.Load("Android Shading/" + originalMaterial[i]+"-sh", typeof (Material)) as Material;
				//#endif
			}
			comp.renderer.materials = arrMat;
			for(int i = 0; i < numOfMat; i++) 
			{
				comp.renderer.materials[i].color = new Color(1.0f, 0.25f, 0.25f); 
			}		
		}

		/**
		 * The method is to set material to Red and have Silhouette effect (Look through...)
		 * New Silhouette and yellow material = take the main color of Silhouette material (prepared in resource folder), set it to yellow.
		 * The Silhouette material name = original material name + "-sh", both are in resource folder.
		 */ 
		public void setYellSilhouette()
		{
			if(!comp.renderer) return;
			int numOfMat = comp.renderer.materials.Length;
			//if(numOfMat == 1) { comp.renderer.material.color = new Color(1.0f, 0.25f, 0.25f); return;}
			Material [] arrMat = new Material[numOfMat];
			for(int i = 0; i < numOfMat; i++) 
			{	
				//#if UNITY_STANDALONE_WIN
				if(!originalMaterial[i].Contains("-sh"))
				{
					arrMat[i] = Resources.Load(originalMaterial[i]+"-sh", typeof (Material)) as Material;
				}
				else arrMat[i] = Resources.Load(originalMaterial[i], typeof (Material)) as Material;
				//#elif UNITY_ANDROID
				//	arrMat[i] = Resources.Load("Android Shading/" + originalMaterial[i]+"-sh", typeof (Material)) as Material;
				//#endif
			}
			comp.renderer.materials = arrMat;
			for(int i = 0; i < numOfMat; i++) 
			{
				comp.renderer.materials[i].color = new Color(1.0f, 1.0f, 0.00f); 
			}		
		}

		/**
		 * The method is to hide component by using transparent material assignment
		 */ 
		public void hideByTransparentMaterial()
		{	
			int numOfMat = comp.renderer.materials.Length;

			Material [] arrMat = new Material[numOfMat];
			for(int i = 0; i < numOfMat; i++) 
			{			
				arrMat[i] = Resources.Load("Transparent", typeof (Material)) as Material;
			}
			comp.renderer.materials = arrMat;
		}

		/**
		 * The method is to hide or show component by Renderer of component
		 * @param shown : true is shown, false is hidden
		 */ 
		public void showByEnableRenderer(bool shown)
		{		
			comp.renderer.enabled = shown;
			
		}

		/**
		 * The method is to enable or disable component box collider
		 * @param value : true is enabled, false is disabled
		 */ 
		public void enableBoxCollider(bool value)
		{		
			boxCollider.enabled = value;
		}

		/**
		 * The method is to reset component material to the original one
		 */ 
		public void reset()
		{	
			if(!comp.renderer) return;
			int numOfMat = comp.renderer.materials.Length;
			//if(numOfMat == 1) { comp.renderer.material.color = new Color(1.0f, 0.25f, 0.25f); return;}
			Material [] arrMat = new Material[numOfMat];
			for(int i = 0; i < numOfMat; i++) 
			{	
				#if UNITY_ANDROID || UNITY_IPHONE
					arrMat[i] = Resources.Load("Android Shading/" + originalMaterial[i], typeof (Material)) as Material;	
				#elif UNITY_STANDALONE_WIN
				if(!originalMaterial[i].Contains("-sh"))
					arrMat[i] = Resources.Load(originalMaterial[i], typeof (Material)) as Material;
				else
					arrMat[i] = Resources.Load(originalMaterial[i].Replace("-sh",""), typeof (Material)) as Material;
				#endif
				
			}
			comp.renderer.materials = arrMat;
		}

		/**
		 * \ref TouchEnableAnimationResearch is attached manually to each component that should be worked with.
		 * This method is to enable this script every time component is the current component being learnt
		 */ 
		public void enableTouchScript(bool v)
		{
			TouchEnableAnimationResearch tempScript = comp.GetComponent(typeof(TouchEnableAnimationResearch)) as TouchEnableAnimationResearch;
			tempScript.enabled = v;
			tempScript.setAnimationTouch(v);
		}

		/**
		 * This method is to set current task to \ref TouchEnableAnimationResearch
		 * TouchEnableAnimationResearch needs information about current task to work.
		 */ 
		public void setCurrentTask(AbstractCommonTask task)
		{
			TouchEnableAnimationResearch tempScript = comp.GetComponent(typeof(TouchEnableAnimationResearch)) as TouchEnableAnimationResearch;	
			tempScript.currentTask = task;
		}

		/**
		 * This method is to get the wrong-tool-checking result from \ref TouchEnableAnimationResearch
		 */ 
		public bool getIsWrongTools()
		{
			TouchEnableAnimationResearch tempScript = comp.GetComponent(typeof(TouchEnableAnimationResearch)) as TouchEnableAnimationResearch;
			return tempScript.IsWrongTools;
		}
	}
}