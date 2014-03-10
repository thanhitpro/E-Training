using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * VolumeSliderPassed Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the passed value of Volume Bar
	 */
	public class VolumeSliderPassed : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			VolumeSlider scriptFS = GameObject.Find("VolumeSlider").GetComponent(typeof(VolumeSlider)) as VolumeSlider;
			int xRange = scriptFS.XSize;
			
			#if UNITY_STANDALONE
			xSize = Screen.width/2 + scriptVideo.VideoWidth/4 - script.getZeroToBoxInfoWidth()/2 + 4 + 27 + 4;
			// Draw Slider Passed accoording to position of Screen size, current position of volume slider and current size of info bar
			GUI.Box(new Rect(xSize, Screen.height/2 + scriptVideo.VideoHeight/2, xRange - xSize- script.getZeroToBoxInfoWidth()/2, 52),"");				
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
			xSize = 504;		
			ySize = 67;
			// all in pixel
			int h = Screen.height;				
			int w = Screen.width;
			//y = (int)order*ySize;
			y = 70 + 74 + 74+ 83 + 101;
			x = w - xSize  - 270 ;
		}

	}
}