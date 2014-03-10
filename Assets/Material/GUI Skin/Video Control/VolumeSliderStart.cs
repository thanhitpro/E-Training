using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * VolumeSliderStart Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the slider at the begining of volume bar 
	 */
	public class VolumeSliderStart : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			
			#if UNITY_STANDALONE
			// Draw Slider Start accoording to position of Screen size, video size and current size of info bar
			GUI.Box(new Rect(Screen.width/2 + scriptVideo.VideoWidth/4 + 4 + 27 - script.getZeroToBoxInfoWidth()/2, Screen.height/2 + scriptVideo.VideoHeight/2, 4, 52),"");													
			#endif
		}

		#endregion

		// Use this for initialization
		void Start ()
		{
			
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif
			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 67;
			// all in pixel
		}
	}
}