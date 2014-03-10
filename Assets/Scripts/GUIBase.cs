using UnityEngine;
using ETraining.UI.StepControls;

namespace ETraining.UI
{
	/**
	 * The base class of GUI controls.
	 * \author AnhNguyen
	 */
	abstract public class GUIBase : MonoBehaviour
	{
		public GUISkin mySkin; /**< The custom GUISkin to be used.*/
		public bool used = false; /**< The bool variable to hold value returned from GUI.Toggle.*/
		public bool checkUsed = false; /**< The bool variable to compare with \ref used and decide if mode is changed.*/

		protected int standardWidth = 1920; /**< The standard width for dynamic GUI, every device  will scale this base size if it is not 1920 x 1080 resolution.*/
		protected int standardHeight = 1080;/**< The standard height for dynamic GUI, every device will scale this base size if it is not 1920 x 1080 resolution**/

		public int x; /**< x-coordinate of top-left corner of GUI control.*/
		public int y; /**< y-coordinate of top-left corner of GUI control.*/

		public int order; /**< The order that GUI control appears. It is used for \ref ToolBarControl*/

		protected int xSize; /**< Width of GUI control.*/
		protected int ySize; /**< Height of GUI control.*/

		protected bool scriptFound = false;
		protected bool firstFound = true;

		protected ShowButtonInfo script = null; /**< Script \ref ShowButtonInfo is called to check position of ShowInfoButton control.*/

		protected bool isFollowedShowInfoButton = true;  /**< Decide whether GUI control is layout in regard with ShowButtonInfo script.*/

		/**
		 * Function helps to locate GUI control vertically dynamically and stick Gui control to the distance by 
		 * value parameter based on the ratio of scale: Screen.height / \ref standardHeight.
		 * @param value : the vertical distance to top edge of screen that GUI control should locate
		 */
		protected int findYPosFor(int value)
		{
			return Mathf.CeilToInt(Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)(value)));
		}
		/**
		 * Function helps to locate GUI control horizontally dynamically and stick Gui control to the distance by 
		 * value parameter based on the ratio of scale: Screen.width / \ref standardWidth.
		 * @param value : the horizotal distance to left edge of screen that GUI control should locate
		 */
		protected int findXPosFor(int value)
		{
			return Mathf.CeilToInt(Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)(value)));
		}
		/**
		 * OnGUI is used to draw GUI control, it runs more than once per frame ( can be 4 5 times per frame)
		 * We should put everthing related to GUI in OnGUI to avoid unexpected error.
		 * If child classes of GUIBase defines their own OnGUI, this function is not used.
		 */	
		protected void OnGUI () 
		{
			/**
			 * Because of the overlapping of GUI Control onto Splash Screen. So just
			 * let GUI run, only after Splash screen is done in first time.
			 */
			if(SplashScreenControl.isFirstTime) return;

			// Assign mySkin to the GUI.skin of system. Now original GUI is changed as our defined style in mySkin
			GUI.skin = mySkin;

			// Just find the script once, and relocate GUI.Toggle control by the control from script ShowInfoButton
			// firstFound guarantees we just find object ShowInfoButton once. scriptFound indicate whether ShowInfoButton found or not
			if(firstFound)
			{
				firstFound = false; // first found is done already
				// try to find script at first, then whatever it exists, we never find it anymore
				if(GameObject.Find("ShowInfoButton"))
				{
					script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
					scriptFound = true;
				}	
				else
				{	
					scriptFound = false;
				}
			}

			if(scriptFound && isFollowedShowInfoButton)
			{
				//if scritp found and it follows position of ShowInfoButton
				used = GUI.Toggle (new Rect (script.x,y, xSize, ySize), used,"");
			}
			else
			{
				used = GUI.Toggle (new Rect (x, y, xSize, ySize), used,"");
			}

			// GUI.Toggle will change the value of \ref used, if used != checkUsed, we should know click event is fired and 
			// do something else by calling loadMode()
			if(used != checkUsed) 
			{						
				loadMode();
			}		
		}
		/**
		 * An abstract method to be overrided in child classes of \ref GUIBase.
		 * In childs class, this method should:
		 * 1. Call another mode, or
		 * 2. Do click event, or
		 * 3. Operate some specific task.
		 */
		abstract public void loadMode();
	}
}
