using UnityEngine;
using System.Collections;

namespace ETraining.UI.Scenario
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Exit Editor Button
	 */
	public class ExitEditor : GUIBase
	{
		public bool getUsedStatus(){ return used;}
		// Use this for initialization
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 56;
			ySize = 75;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// This GUI control is located in left edge of screen, so we dont need to move it when
			// info bar is sliding out or sliding in
			isFollowedShowInfoButton = false;

			// Define top left corner GUI Control should be located.
			y = 0;
			x = 0 ;
		}
		#region implemented abstract members of MenuModeGUIBase

		public override void loadMode ()
		{
			if(Application.loadedLevelName == "TransmissionMode")
				Application.LoadLevel("TransmissionEditorMode");
			else if(Application.loadedLevelName == "TrainingMode")
			{
				Application.LoadLevel("TrainingEditorMode");
			}
			else if(Application.loadedLevelName == "TransmissionEditorMode")
			{			
				// Close external window-based editor
				ScenarioEditorController script = Camera.main.GetComponent(typeof(ScenarioEditorController)) as ScenarioEditorController;
				try {
					script.ExternalEditor.CloseMainWindow ();
				} catch {
				} finally {
					// Nomatter what happens, just load level Transmission
					Application.LoadLevel ("TransmissionMode");
				}					
			}
			else
			{		
				// Close external window-based editor
				ScenarioEditorController script = Camera.main.GetComponent(typeof(ScenarioEditorController)) as ScenarioEditorController;
				try {
					script.ExternalEditor.CloseMainWindow ();
				} catch {
				} finally {

					// no splash screen anymore
					SplashScreenControl.isFirstTime = false;
					// Nomatter what happens, just load level Training
					Application.LoadLevel ("TrainingMode");
				}					
			}

			// checkUsed is aimed to track the used's value, so it 's assigned used's value
			checkUsed = used;
		}

		#endregion
	}
}	