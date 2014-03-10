using UnityEngine;
using System.Collections;
namespace ETraining.UI.StepControls
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Next Button
	 * which move learning process to next component of engine or transmission
	 */
	public class NextButton : GUIBase 
	{
		public bool Used {
			get {
				return used;
			}
			set {
				used = value;
			}
		}
		
		IEnumerator WaitInSeconds(float waitTime) {
	        yield return new WaitForSeconds(waitTime);        
	    }
		
		IEnumerator WaitforUpdate() 
		{        
	        yield return StartCoroutine(WaitInSeconds(0.5F));   
			used = true;
	    }

		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 67;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);
		
			// Define top left (y coordinate only) corner GUI Control should be located.
			y =   findYPosFor(70) 
				+ findYPosFor(74)
				+ findYPosFor(74)
				+ findYPosFor(83)
				+ findYPosFor(101);
		}

		#region implemented abstract members of MenuModeGUIBase
		public override void loadMode ()
		{
			// Simply do nothing
			return;
		}
		#endregion
	}

}