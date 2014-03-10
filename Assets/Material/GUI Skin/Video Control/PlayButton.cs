using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * PlayButton Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to control PlayButton 
	 */
	public class PlayButton : VideoGUIBase {

		private BarBeginEndControl barScript;

		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			#if UNITY_STANDALONE_WIN
			// Update position of this GUI control accoording to current Screen size, video size and current size of the info box
			topLeftX = Screen.width/2 - scriptVideo.VideoWidth/2 - script.getZeroToBoxInfoWidth()/2;
			topLeftY =  Screen.height/2 + scriptVideo.VideoHeight/2;
			#endif
			// Handling the toggle of this GUI control
			used = GUI.Toggle(new Rect(topLeftX, topLeftY, xSize, ySize),used,"");		
			#if UNITY_STANDALONE_WIN
			if(used)
			{
				scriptVideo.setPlay();
			}
//			else 
//			{
//				scriptVideo.setPause();		
//			}
			#endif
		}

		#endregion

		/**
		 * Get and set value of used
		 */
		public bool Used {
			get {
				return this.used;
			}
			set {
				used = value;
			}
		}

		// Use this for initialization
		void Start () {
			barScript =  GameObject.Find("BarBegin_End").GetComponent(typeof(BarBeginEndControl)) as BarBeginEndControl;
			#if UNITY_STANDALONE_WIN
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif
			xSize = 38;		
			ySize = 52;
		}

	}
}
