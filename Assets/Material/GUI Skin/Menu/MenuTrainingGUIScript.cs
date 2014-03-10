using UnityEngine;

namespace ETraining.UI.Menu
{

	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Training Menu Button
	 */
	public class MenuTrainingGUIScript : GUIBase
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
			y = findYPosFor(70) + findYPosFor(74);				
		}
		

		#region implemented abstract members of MenuMode
		public override void loadMode ()
		{
			// Now the main.cs script for splash screen is no longer running for the firstime
			SplashScreenControl.isFirstTime = false;
			// Load The Training mode, without start the splash screen
			Application.LoadLevel("TrainingMode");

		}
		#endregion
	}

}