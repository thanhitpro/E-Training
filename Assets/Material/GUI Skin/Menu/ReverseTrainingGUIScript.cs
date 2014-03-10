using UnityEngine;
using System.Collections;

namespace ETraining.UI.Menu
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control ReverseTraining Menu Button
	 */
	public class ReverseTrainingGUIScript : GUIBase
	{
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 62;
			ySize = 74;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// Define top left corner GUI Control should be located.
			x = Screen.width - xSize;
			y =   findYPosFor (70)
				+ findYPosFor (74)
				+ findYPosFor (74)
				+ findYPosFor (83)
				+ findYPosFor (101)
				+ findYPosFor (67)
				+ findYPosFor (72);
		}

		#region implemented abstract members of MenuModeGUIBase
		public override void loadMode ()
		{

			if(Application.loadedLevelName == "TrainingMode")
			{
				// Load The Reverse Training mode.
				SwitchScreenController.screenName = "ReverseTrainingMode";
				SwitchScreenController.loadingTextureIndex = 1;
				Application.LoadLevel("SwitchScreen");
			}
			else
			{
				// Now the main.cs script for splash screen is no longer running for the firstime
				SplashScreenControl.isFirstTime = false;
				// Load Training mode
				SwitchScreenController.loadingTextureIndex = 0;
				SwitchScreenController.screenName = "TrainingMode";
				Application.LoadLevel("SwitchScreen");
			}
			//checkUsed = used;
		}
		#endregion
	}
}	