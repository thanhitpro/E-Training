using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;



namespace ETraining.UI.VideoControls
{
	/**
	 * StopButton Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to control StopButton 
	 */
	public class StopButton : VideoGUIBase {

		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			#if UNITY_STANDALONE_WIN
			// Draw Stop Button accoording to position of Screen size, video size, play button and current size of info bar
			topLeftX = Screen.width/2 - scriptVideo.VideoWidth/2  + playButtonScript.XSize - script.getZeroToBoxInfoWidth()/2;
			topLeftY = Screen.height/2 + scriptVideo.VideoHeight/2;
			#endif

			used = GUI.Toggle(new Rect(topLeftX, topLeftY, xSize, ySize),used,"");	
		}

		#endregion

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
		private PlayButton playButtonScript;
		
		/**
		 * Get and set value of /ref checkUsed
		 */
		public bool CheckUsed {
			get {
				return this.checkUsed;
			}
			set {
				checkUsed = value;
			}
		}
		
		// Use this for initialization
		void Start () {
			playButtonScript = GameObject.Find("PlayButton").GetComponent(typeof(PlayButton)) as PlayButton;
			#if UNITY_STANDALONE_WIN
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif
			// The size is hard-assigned using size of GUI Image
			xSize = 43;		
			ySize = 52;
		}
	
	}
}
