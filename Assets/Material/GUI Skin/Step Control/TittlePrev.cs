using UnityEngine;
using System.Collections;
namespace ETraining.UI.StepControls
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Prev Button on the title bar docked in top edge of screen
	 * which moves learning process to previous component of engine or transmission
	 */
	public class TittlePrev : GUIBase
	{
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 70;
			ySize = 46;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// Define top left (y coordinate only) corner GUI Control should be located.
			y = 0;
			// 374 is the fixed width of StepTittle GUI Control
			x = Screen.width/2 - Mathf.CeilToInt((float)Screen.width / (float)standardWidth * 374.0f) /2 - Mathf.CeilToInt((float)Screen.width / (float)standardWidth * 70);
		}
		
		void OnGUI () 
		{
			// Only run GUI when splash screen is done for the first time.
			if(SplashScreenControl.isFirstTime) return;

			GUI.skin = mySkin;
			// for the first run: we show the button
			if(!scriptFound && firstFound)
			{
				// try to find script at first, then whatever it exists, we never find it anymore
				if(GameObject.Find("ShowInfoButton"))
				{
					script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
					scriptFound = true;
				}			 
				firstFound = false; // first found is done already
			}
			
			if(scriptFound)
			{
				// Locate GUI Control acording to \ref ShowInfoButton position
		 		used = GUI.Toggle (new Rect (x - (script.getZeroToBoxInfoWidth()/2),y, xSize, ySize),used,"");
			}
		}

		#region implemented abstract members of MenuModeGUIBase

		public override void loadMode ()
		{
			// do nothing
			return;
		}

		#endregion
	}
}