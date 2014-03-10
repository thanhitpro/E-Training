using UnityEngine;
using System.Collections;
using ETraining.UI.StepControls;

namespace ETraining.UI.VideoControls
{
	/**
	 * VideoButton Class
	 * The child classe of /ref GUIBase
	 * This class based on GUIBase and used to control VideoButton 
	 */
	public class VideoButton : GUIBase
	{
		
		#if UNITY_STANDALONE
		private PlayButton pl;
		#endif
		private bool isShowGUI; // the variale is used to check the video is showed on the screen
		private string videoName;
		private Color bgColor = Color.black;
		#if UNITY_ANDROID || UNITY_IPHONE	
		private FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
		private FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
		#endif	
		#if UNITY_STANDALONE_WIN
		private  PlayMovie scriptVideo; // The object is used to play video
		#endif	

		// Use this for initialization
		void Start ()
		{
			#if UNITY_STANDALONE_WIN
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			pl = GameObject.Find("PlayButton").GetComponent(typeof(PlayButton)) as PlayButton;
			#endif

			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 84;
			// all in pixel

			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);
			
			int h = Screen.height;				
			int w = Screen.width;
			
			
			//y = (int)order*ySize;
			y =   findYPosFor(70)
				+ findYPosFor(74)
				+ findYPosFor(74)
				+ findYPosFor(83)
				+ findYPosFor(101)
				+ findYPosFor(67)
				+ findYPosFor(72)
				+ findYPosFor(74);
			//y =  Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) *  (70+74+74+83+101+67+72 + 74));
		
			//x = w - xSize  - 270 ;

		}

		// Set value of /ref videoName
		public void setVideoName(string videoName) { this.videoName = videoName;}
		// Set value of isShowGUI
		public void setShowGUI(bool value) { isShowGUI = value;}
		
		void OnGUI () 
		{			
			if(!isShowGUI) return;
			GUI.skin = mySkin;

				// Assign the skin to be the one currently used.
				
				// for the first run: we show the button
				if(!scriptFound && firstFound)
				{
					// try to find script at first, then whatever it exists, we never find it anymore
					if(GameObject.Find("ShowInfoButton"))
					{
						script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
						scriptFound = true;
					}			 
					firstFound = false; // first found is done already
				}

				if(scriptFound)
				{
					//used = GUI.Toggle(new Rect (script.x,y, xSize, ySize),used,"");
					//if(GUI.Button (new Rect (script.x,y, xSize, ySize),""))
					//if(used != checkUsed)
					if(GUI.Button (new Rect (script.x,y, xSize, ySize),""))
					{	
						Resources.UnloadUnusedAssets ();
						#if UNITY_STANDALONE_WIN
							scriptVideo.enabled =  !scriptVideo.enabled;		

							if(scriptVideo.enabled)
							{	
								scriptVideo.setStop();
								scriptVideo.releaseVideo();
								scriptVideo.setVideo(videoName + ".ogv"); 								
								
								pl.Used = true;
								scriptVideo.setPlay();
								
							}
							else
							{								
								scriptVideo.setStop();								
								scriptVideo.releaseVideo();
								pl.Used = false;
							}
						#endif
						#if UNITY_ANDROID								
							//When video button clicked, open android video player here	
							Handheld.PlayFullScreenMovie(Application.persistentDataPath + "/" + videoName + ".mp4", bgColor, controlMode, scalingMode);
						#endif	
						#if UNITY_IPHONE
							//When video button clicked, open ios video player here	
							Handheld.PlayFullScreenMovie("file://" + Application.persistentDataPath + "/" + videoName + ".mov", bgColor, controlMode, scalingMode);
						#endif	
						//checkUsed = used;
					}
					
				}




		}

		#region implemented abstract members of MenuModeGUIBase

		public override void loadMode ()
		{
			return;
		}

		#endregion
	}
}

