using UnityEngine;

namespace ETraining.UI.Scenario
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Edit Scenario Button
	 */
	public class EditScenario : GUIBase
	{
		public bool getUsedStatus(){ return used;}

		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 56;
			ySize = 75;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// Define top left corner GUI Control should be located.
			y = findYPosFor(62);
			x = 0;

			isFollowedShowInfoButton = false;
		}

		#region implemented abstract members of MenuModeGUIBase

		public override void loadMode ()
		{
			// Do multiple mode loading based on current loaded level
			if(Application.loadedLevelName == "TransmissionMode")
			{
				Application.LoadLevel("TransmissionEditorMode");
			}
			else if (Application.loadedLevelName == "TransmissionEditorMode")
			{
				Application.LoadLevel("TransmissionMode");
			}
			else if (Application.loadedLevelName == "TrainingEditorMode") {
				// Splash screen is not running any more
				SplashScreenControl.isFirstTime = false;
				Application.LoadLevel("TrainingMode");
			}

			else if (Application.loadedLevelName == "TrainingMode")
			{
				Application.LoadLevel("TrainingEditorMode");
			}
			else{}
			// make checkUsed same with used
			checkUsed = used;
		}

		#endregion
	}
}	