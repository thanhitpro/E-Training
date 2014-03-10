using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * SliderRemain Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the remain value of Slider Control Bar
	 */
	public class SliderRemain : VideoGUIBase 
	{
		#if UNITY_STANDALONE		
		private FrameSlider fsScript;
		#endif
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			
			#if UNITY_STANDALONE					
			// Draw Slider Passed accoording to position of Screen size, current position of slider and current size of info bar
			GUI.Box(new Rect(fsScript.AddedValue + fsScript.sliderStartScript.topLeftX + fsScript.XSize, Screen.height/2 + scriptVideo.VideoHeight/2, xSize -  fsScript.TopLeftX - fsScript.XSize - fsScript.AddedValue, 52),"");		
			#endif
		}
		#endregion

		// Use this for initialization
		void Start ()
		{
			
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			fsScript = GameObject.Find ("FrameSlider").GetComponent(typeof(FrameSlider)) as FrameSlider;	
			// The size is hard-assigned using size of GUI Image
			xSize = Screen.width/2 + scriptVideo.VideoWidth/4;		
			#endif
			ySize = 67;
			// all in pixel						
			int h = Screen.height;				
			int w = Screen.width;

		}
	}
}