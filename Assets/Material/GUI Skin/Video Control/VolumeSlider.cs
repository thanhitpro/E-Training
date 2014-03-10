using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;


namespace ETraining.UI.VideoControls
{
	/**
	 * VolumeSlider Class
	 * The child classe of /ref VideoGUIBase
	 * This class based on VideoGUIBase and used to draw and control the volume slider 
	 */
	public class VolumeSlider : VideoGUIBase {
		#region implemented abstract members of VideoGUIBase
		protected override void locateGUI ()
		{
			// Draw Volume Slider accoording to current size of info bar and position of it
			GUI.Box(new Rect(xSize - script.getZeroToBoxInfoWidth()/2, ySize, 17, 52),  "");

			/**
			 * Check user touch on the Volume slider
			 */
			if (iPhoneInput.touchCount > 0)
			{
				// Get the position that the value of volume is 0
				int whereVolumeIs0 = xSize - Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)70);
				// Store current xSize to temp value
				int xSizeTemp = xSize;
				// Get x Axis of the touch position
				float x = iPhoneInput.GetTouch(0).position.x * script.getViewportRatio();
				// Get y Axis of the touch position
				float y = iPhoneInput.GetTouch(0).position.y;

				/** Check: If current touch position is inside the volume slider area, set hasSlided is true
				 * That mean user is sliding / change volume
				 */
				if( x > xSize - script.getZeroToBoxInfoWidth()/2 && x < xSize - script.getZeroToBoxInfoWidth()/2 + 17 && y <(Screen.height - ySize) - 12 && y > (Screen.height - ySize) - 30)
				{
					// smooth follow when sliding
					// Change position of the volume slider
					xSize = (int)((iPhoneInput.GetTouch(0).position.x - 9) *  script.getViewportRatio()  + script.getZeroToBoxInfoWidth()/2 ) ;
					hasSlided = true;
				}
				else
				{
					/**
					 * Check: If current touch is out of the volume slider area and state is hasSlided
					 * That mean the user slide to too quick so the volume slider cannot follow
					 * At that time, the system cannot go to if statement although the user is sliding
					 * But: Check hasSlided is true, the system continue to update position of slider accoording to user's touch position
					 * Set postion of the volume volume slider
					 */
					if(hasSlided) xSize = (int)((iPhoneInput.GetTouch(0).position.x  - 9) *  script.getViewportRatio() + script.getZeroToBoxInfoWidth()/2 );
					
				}

				// Check position of the volume slider at the beginning positon
				if(xSize == whereVolumeIs0 )
				{
					// Set volume is 0
					VolumeUp = 0;
				}	
				else
				{
					// Change volume accoording to current position and previous position of volume slider
					// Increase volume
					if(xSize > xSizeTemp ) VolumeUp = 1;			
					// Decrease volume
					if(xSize < xSizeTemp ) VolumeUp = -1;
				}

				/**
				 * Check user has been sliding
				 */ 
				if(hasSlided)
				{
					GameObject cube;
					cube = GameObject.Find("Cube");
					/** Move cube go to Camera position
					 * At position, you cannot touch to Cube, so you cannnot rotate the engine when you slide the volume
					 */
					cube.transform.position = Camera.main.transform.position ;//+ (new Vector3(0.0f, 0.0f, +0.5f));
				}
				
			}
			// Stop touch
			else{
				// Reset hasSlided value to false
				hasSlided = false;
				GameObject cube;
				cube = GameObject.Find("Cube");
				// Move cube to the position that you can rotate and control the engine
				Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find("engine_LOW").transform.position;
				cube.transform.position = position;
			}
			
			#if UNITY_STANDALONE
			if(xSize <= Screen.width/2 + scriptVideo.VideoWidth/4 + 4 + 27 + 4) xSize = Screen.width/2 + scriptVideo.VideoWidth/4  + 4 + 27 + 4;
			else if(xSize > Screen.width/2 + scriptVideo.VideoWidth/2 - 4 - 17) xSize = Screen.width/2 + scriptVideo.VideoWidth/2 - 4 - 17;
			#endif
		}
		#endregion

		/**
		 * volumeUp the value will be added to current volume when user slide
		 */
		private int volumeUp;
		// Get and set value of /ref volumeUp
		public int VolumeUp {
			get {
				return this.volumeUp;
			}
			set {
				volumeUp = value;
			}
		}

		private float sliderValue ;

		// hasSlided check user is sliding on the volume slider
		private bool hasSlided = false;

		// Use this for initialization
		void Start ()
		{
			
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;

			// Get position of volume slider
			xSize = Screen.width/2 + Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)scriptVideo.VideoWidth/4) +
				Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)4) +
					Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)27) + 
					Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)4) + Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)70);
			ySize = Mathf.CeilToInt(Screen.height/2) + Mathf.CeilToInt(((float)Screen.height / (float)1080) * (float)scriptVideo.VideoHeight/2);
			
			#endif
			// all in pixel
		}
	}
}