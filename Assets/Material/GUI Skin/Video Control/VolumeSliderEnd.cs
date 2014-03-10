using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{	
	/**
	 * VolumeSliderEnd Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the slider at the end of volume bar 
	 */
	public class VolumeSliderEnd : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			
			#if UNITY_STANDALONE
			// Draw VolumeSliderEnd accoording to position of Screen size, video size and current size of info bar
			GUI.Box(new Rect(Screen.width/2 + scriptVideo.VideoWidth/2 - 4 - script.getZeroToBoxInfoWidth()/2 ,Screen.height/2 + scriptVideo.VideoHeight/2, 4, 52),"");		
			#endif
		}
		#endregion

		private VolumeSliderStart scriptVoSliStar;

		// Use this for initialization
		void Start ()
		{
			
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;				
			#endif
			scriptVoSliStar = GameObject.Find("VolumeSliderStart").GetComponent(typeof(VolumeSliderStart)) as VolumeSliderStart; 

			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 67;
			// all in pixel
			int h = Screen.height;				
			int w = Screen.width;
			//y = (int)order*ySize;
			y = 70 + 74 + 74+ 83 + 101;
			x = w - xSize  - Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)270) ;
		}
	}
}