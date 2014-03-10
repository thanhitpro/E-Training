using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;

namespace ETraining.UI
{
	/**
	 * The base class of Video GUI
	 */ 
	abstract public class VideoGUIBase : MonoBehaviour {

		public GUISkin mySkin;
		public bool used = false;
		public bool checkUsed = false;
		public bool firstRun = true;
		public int x;
		public int y;
		public int order;
		protected int xSize; // width of GUI image
		protected int ySize; // height of GUI image
		public int topLeftX; // x Axis of the control position
		public int topLeftY; // y Axis of the control position
		#if UNITY_STANDALONE
		protected PlayMovie scriptVideo;
		#endif			

		private bool firstFound = true;

		protected ShowButtonInfo script;

		public bool getUsedStatus() {return used;}

		public int XSize {
			get {
				return this.xSize;
			}
			set {
				xSize = value;
			}
		}
		void OnGUI () 
		{
			#if UNITY_STANDALONE_WIN
			// If cannot file current video or current video is disable, return 
			if(scriptVideo == null || !scriptVideo.enabled) return;			
			// Assign new skin to GUI skin
			GUI.skin = mySkin;			
			if(firstFound)
			{
				script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
				firstFound = false;
			}			
			locateGUI();
			#endif		
			
		}
		abstract protected void locateGUI();
	}
}