using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;



namespace ETraining.UI.VideoControls
{
	/**
	 * SliderStart Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw the slider at the begining of slider bar 
	 */
	public class SliderStart : VideoGUIBase {

		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			#if UNITY_STANDALONE_WIN
			// Draw Slider Start accoording to position of Screen size, video size and current size of info bar
			topLeftX = Screen.width/2 - scriptVideo.VideoWidth/2 + 38 + 43 - script.getZeroToBoxInfoWidth()/2 ;
			topLeftY = Screen.height/2 + scriptVideo.VideoHeight/2;
			#endif

			GUI.Box(new Rect(topLeftX, topLeftY, xSize, ySize),"");
		}
		#endregion

		private StopButton stopButtonScript;

		/**
		 * Get value of /ref topLeftY
		 */
		public int TopLeftY {
			get {
				return topLeftY;
			}
		}

		/**
		 * Get and set value of /ref topLeftX
		 */
		
		public int TopLeftX {
			get {
				return this.topLeftX;
			}
			set {
				topLeftX = value;
			}
		}
		// Use this for initialization
		void Start ()
		{
			stopButtonScript = GameObject.Find("StopButton").GetComponent(typeof(StopButton)) as StopButton;
			#if UNITY_STANDALONE_WIN
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif

			// The size is hard-assigned using size of GUI Image
			xSize = 4;		
			ySize = 52;
			// all in pixel
		}
	
	}
}
