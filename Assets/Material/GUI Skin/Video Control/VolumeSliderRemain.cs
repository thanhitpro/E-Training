using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * VolumeSliderRemain Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the remain value of Volume Bar
	 */
	public class VolumeSliderRemain : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			VolumeSlider  fsScript = GameObject.Find ("VolumeSlider").GetComponent(typeof(VolumeSlider)) as VolumeSlider;
			
			#if UNITY_STANDALONE
			// Draw Slider Passed accoording to position of Screen size, current position of volume slider and current size of info bar
			GUI.Box(new Rect(fsScript.XSize + 17 - script.getZeroToBoxInfoWidth()/2,Screen.height/2 + scriptVideo.VideoHeight/2, Screen.width/2 + scriptVideo.VideoWidth/2 - 4 -(fsScript.XSize + 17), 52),"");		
			#endif
		}

		#endregion

		// Use this for initialization
		void Start ()
		{
			script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
			
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			// The size is hard-assigned using size of GUI Image
			xSize = Screen.width/2 + scriptVideo.VideoWidth/2;		
			
			#endif
			ySize = 67;
			// all in pixel
		}
	}
}