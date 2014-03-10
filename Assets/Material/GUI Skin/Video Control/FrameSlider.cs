using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;



namespace ETraining.UI.VideoControls
{
	/**
	 * FrameSlider Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to control slider of Video Control Bar 
	 */
	public class FrameSlider : VideoGUIBase {

		private int addedValue = 0; /**< Current position of slider.*/
		public SliderStart sliderStartScript; /**<Start position of Slider.*/

		/**
		 * Get and set value for field \ref addedValue
		 */
		public int AddedValue {
			get {
				return this.addedValue;
			}
			set {
				addedValue = value;
			}
		}

		/**
		 * Increase position of Slider \ref addedValue.
		 * @param int valueI : this is the value that added to the current position in increments.
		 */
		public void increasePosition(int valueI)
		{
			addedValue += valueI;
		}

		/**
		 * Reset the current position of slider
		 */
		public void setBegin() { 
			addedValue = 0;
		}

		/**
		 * Get and set value for field \ref topLeftX
		 */
		public int TopLeftX {
			get {
				return this.topLeftX;
			}
			set {
				topLeftX = value;
			}
		}

		/**
		 * Get value for field \ref used
		 */
		public bool getUsedStatus() {
			return used;
		}

		// Use this for initialization
		void Start ()
		{
			#if UNITY_STANDALONE_WIN
				sliderStartScript = GameObject.Find("SliderStart").GetComponent(typeof(SliderStart)) as SliderStart;
				scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;

				// The size is hard-assigned using size of GUI Image
				xSize = 17;	
				ySize = 52;
			#endif
		}

		#region implemented abstract members of VideoGUIBase

		protected override void locateGUI ()
		{
			// Update position of this GUI control accoording to sliderStartScript and addedValue
			topLeftX = sliderStartScript.TopLeftX + sliderStartScript.XSize + script.getZeroToBoxInfoWidth()/2;
			topLeftY = sliderStartScript.TopLeftY;
			#if UNITY_STANDALONE_WIN
				GUI.Box(new Rect(topLeftX - script.getZeroToBoxInfoWidth()/2 + addedValue , topLeftY, xSize, ySize),  "");
			#endif
		}

		#endregion

		/**
		 * Check position of slider at the end or not
		 * @access public
		 * @param ShowButtonInfo 
		 * @return bool
		 */
		public bool checkEndPosition(ShowButtonInfo script)
		{	
			#if UNITY_STANDALONE_WIN					
				return(topLeftX - script.getZeroToBoxInfoWidth()/2  + addedValue)>=(Screen.width/2 + scriptVideo.VideoWidth/4 - script.getZeroToBoxInfoWidth()/2 - xSize);	
			#endif
			return true;
		}

	}
}
