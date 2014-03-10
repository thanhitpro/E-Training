using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * VideoFrame Class
	 * The child classe of /ref VideoGUIBase
	 */
	public class VideoFrame : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{

		}
		#endregion

		// Use this for initialization
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 800;		
			ySize = 600;
			// all in pixel
			int h = Screen.height;				
			int w = Screen.width;
			//y = (int)order*ySize;
			y = 70 + 74 + 74+ 83 + 101;
			x = w - xSize  - 270 ;
		}
	}
}