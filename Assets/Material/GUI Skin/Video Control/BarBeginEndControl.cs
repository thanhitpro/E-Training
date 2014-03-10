using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;

namespace ETraining.UI.VideoControls
{
	/**
	 * BarBeginEndControl Class
	 * The child class of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the begin and end area of the Video Control bar
	 */
	public class BarBeginEndControl : VideoGUIBase {
		// Use this for initialization
		public GUISkin mySkinBarEnd;

		void Start ()
		{
			#if UNITY_STANDALONE_WIN
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif

			// The size is hard-assigned using size of GUI Image
			xSize = 10;		
			ySize = 52;					
		}

	
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			// In desktop version only.
			// Compute position of this GUI Control based on position of PlayMovie

				// Update position of this GUI control accoording to current Screen size, video size and current size of the info box
			#if UNITY_STANDALONE_WIN
				topLeftX = Screen.width / 2 - scriptVideo.VideoWidth / 2 - scriptVideo.VideoLeftRightBorder - script.getZeroToBoxInfoWidth () / 2;
				topLeftY = Screen.height / 2 + scriptVideo.VideoHeight / 2;
				GUI.Box (new Rect (topLeftX, topLeftY, xSize, ySize), "");
				
				topLeftX = Screen.width / 2 + scriptVideo.VideoWidth / 2 - script.getZeroToBoxInfoWidth () / 2;
				topLeftY = Screen.height / 2 + scriptVideo.VideoHeight / 2;
			#endif
				GUI.skin = mySkinBarEnd;
				GUI.Box (new Rect (topLeftX, topLeftY, xSize +1, ySize ) , "");
		
		}
		#endregion

	}
}