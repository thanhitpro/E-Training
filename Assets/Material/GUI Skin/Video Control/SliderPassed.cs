using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * SliderPassed Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the passed value of Slider Control Bar
	 */
	public class SliderPassed : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			
			#if UNITY_STANDALONE
			FrameSlider scriptFS = GameObject.Find("FrameSlider").GetComponent(typeof(FrameSlider)) as FrameSlider;
			int xRange = scriptFS.TopLeftX;
			xSize = Screen.width/2 - scriptVideo.VideoWidth/2 + 38 + 43 + 4 - script.getZeroToBoxInfoWidth()/2 ;
			// Draw Slider Passed accoording to position of Screen size, current position of slider and current size of info bar
			GUI.Box(new Rect(xSize, Screen.height/2 + scriptVideo.VideoHeight/2 , scriptFS.AddedValue + xRange - xSize- script.getZeroToBoxInfoWidth()/2, 52),"");				
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
		}
	}
}