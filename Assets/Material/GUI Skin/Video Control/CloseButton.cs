
using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;

namespace ETraining.UI.VideoControls
{
	/**
	 * CloseButton Class.
	 * The child class of \ref VideoGUIBase.
	 * This class based on VideoGUIBase and used to control CloseButton. 
	 */
	public class CloseButton : VideoGUIBase {
		void Start () {
			// In desktop version only
			#if UNITY_STANDALONE_WIN
				scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif


			// The size is hard-assigned using size of GUI Image
			xSize = 28;
			ySize = 27;
		}

		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			// In desktop version only
			// Compute position of this GUI Control based on position of PlayMovie

			#if UNITY_STANDALONE_WIN
				topLeftX = Screen.width/2  + scriptVideo.VideoWidth/2 - xSize - script.getZeroToBoxInfoWidth()/2;
				topLeftY = Screen.height/2 - scriptVideo.VideoHeight/2 - scriptVideo.VideoUpBorder;			
			#endif
				// If close button is pressed, stop the current video
				if(	GUI.Button(new Rect(topLeftX, topLeftY, xSize, ySize),""))
				{
				#if UNITY_STANDALONE_WIN
					scriptVideo.setStop();
					scriptVideo.enabled = false;
					scriptVideo.releaseVideo();
				#endif
				}

		}

		#endregion
	}
}