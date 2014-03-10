using UnityEngine;
using System.Collections;

namespace ETraining.UI.Scenario
{
	public class EditScenarioButton : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
		void OnGUI () 
		{			
			if (GUI.Button(new Rect(0,0,50,30),"Edit",GUIStyle.none)) 
			{	
				Application.LoadLevel("TrainingEditorMode");
			}
					
		}	
	}
}