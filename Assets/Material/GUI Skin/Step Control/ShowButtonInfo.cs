using UnityEngine;
using System.Collections;
using ETraining.TrainingMode;
namespace ETraining.UI.StepControls
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Show Info Button
	 * Which button we click to sliding out and in the information box.
	 */
	public class ShowButtonInfo : GUIBase
	{

		/**< The \ref TrainingModeScenarioController provides information of task, so that we can decide to put linked information button or not.*/
		private TrainingModeScenarioController trainingScript; 
		private GUIStyle listStyle;
		/**
		 * Get and set value for field \ref xSize
		 */
		public int XSize {
			get {
				return this.xSize;
			}
			set {
				xSize = value;
			}
		}


		private int widthOfInfoBox = 400; /**< The fixed value of information box width. This value is relative to \ref standardWidth we choose.*/

		/**
		 * Get and set value for field \ref widthOfInfoBox
		 */
		public int WidthOfInfoBox {
			get {
				return widthOfInfoBox;
			}
		}

		private int zeroToWidthOfInfoBox = 0; /**< The value hold the current size of information box. Range is [0 - \ref widthOfInfoBox.*/

		/**
		 * Get field \ref zeroToWidthOfInfoBox
		 */
		public int getZeroToBoxInfoWidth()
		{
			return zeroToWidthOfInfoBox;
		}

		private Texture2D myGUItexture = null; /**< The information content of each task is as a Texture2D.*/

		/**
		 * Set image content to \ref myGUItexture
		 * @param guiTT : a gui texture 2D, holding information image
		 */
		public void setGUITextTure(Texture2D guiTT)
		{
			myGUItexture = guiTT;
		}
		
		/**
		 * When the information box changes, the view port also changes, this function gets the viewportRatio in width.
		 */	
		public float getViewportRatio()
		{
			return (float)(Screen.width - zeroToWidthOfInfoBox)/Screen.width;
		}


		IEnumerator slideOut() 
		{     
	        yield return StartCoroutine(Wait(0.1F));  
			if(x > Screen.width - xSize - widthOfInfoBox)
			{
				#if UNITY_ANDROID|| UNITY_IPHONE
				x -= 15;
				#elif UNITY_STANDALONE_WIN
				x -= 5;
				#elif UNITY_EDITOR
				x -= 5;
				#endif
				
			}
			if(x < Screen.width - xSize - widthOfInfoBox) x = Screen.width - xSize - widthOfInfoBox;
			
			if(zeroToWidthOfInfoBox < widthOfInfoBox) 
			{
				#if UNITY_ANDROID|| UNITY_IPHONE
				zeroToWidthOfInfoBox+= 15;
				#elif UNITY_STANDALONE_WIN
				zeroToWidthOfInfoBox+=5;
				#elif UNITY_EDITOR
				zeroToWidthOfInfoBox += 5;
				#endif
						
			}
			if(zeroToWidthOfInfoBox > widthOfInfoBox) zeroToWidthOfInfoBox = widthOfInfoBox;
			
						
	    }
		
		IEnumerator slideIn() 
		{     
	        yield return StartCoroutine(Wait(0.1F));  
			if(x < Screen.width - xSize)
			{
				#if UNITY_ANDROID || UNITY_IPHONE
				x += 15;		
				#elif UNITY_STANDALONE_WIN
				x += 5;		
				#elif UNITY_EDITOR
				x += 5;
				#endif
				
			}
			if(zeroToWidthOfInfoBox > 0) 
			{
				#if UNITY_ANDROID || UNITY_IPHONE
				zeroToWidthOfInfoBox -= 15;
				#elif UNITY_STANDALONE_WIN
				zeroToWidthOfInfoBox -= 5;
				#elif UNITY_EDITOR
				zeroToWidthOfInfoBox -= 5;
				#endif
				
			}
			if(zeroToWidthOfInfoBox < 0) zeroToWidthOfInfoBox = 0;
	    }
		
	    IEnumerator Wait(float waitTime) {
	        yield return new WaitForSeconds(waitTime);       
	    }

		// Use this for initialization
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 101;
			// all in pixel
			listStyle = new GUIStyle ();
			listStyle.normal.background = new Texture2D (1, 1);
			listStyle.normal.textColor = new Color (0.3f, 0.3f, 0.3f);
			listStyle.alignment = TextAnchor.UpperCenter;
			listStyle.normal.background.SetPixel (0, 0, new Color(1, 1, 1));
			listStyle.normal.background.Apply ();
			listStyle.padding.left =
				listStyle.padding.right =
					listStyle.padding.top =
					listStyle.padding.bottom = 0;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// Scale widthOfInfoBox to ratio of Device.width and user-selected standardWidth 
			widthOfInfoBox = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float) widthOfInfoBox);

			int h = Screen.height;				
			int w = Screen.width;
			
			
			// define top left corner GUI Control should be located.
			y =   findYPosFor(70)
				+ findYPosFor(74)
				+ findYPosFor(74)
				+ findYPosFor(83);
			x = w - xSize;

			// Get \ref TrainingModeScenarioController script
			trainingScript = Camera.main.GetComponent(typeof(TrainingModeScenarioController)) as TrainingModeScenarioController;
		}

		void OnGUI () 
		{
			// Only run GUI when splash screen is done for the first time.
			if(SplashScreenControl.isFirstTime) return;

			// Assign the skin to be the one currently used.
			GUI.skin = mySkin;

			// for the first run: we show the button
		 	used = (GUI.Toggle (new Rect (x,y, xSize, ySize), used,""));

			// Change the viewport of camera based the "running" value of zeroToWidthOfInfoBox
			Camera.main.rect = new Rect(0.0f, 0.0f, (float)(Screen.width - zeroToWidthOfInfoBox)/Screen.width,1.0f);

			// if we click the show info button, we slide it out, otherwise, slide in
			if(used)StartCoroutine(slideOut());
			else StartCoroutine(slideIn());

			// if not having information image or info box is slided in, just show info box with text "Information"
			if (myGUItexture == null || zeroToWidthOfInfoBox <= 0)
			{
				GUI.Box (new Rect (x + xSize, 0, widthOfInfoBox, Screen.height), "Information", listStyle);
			}
			else
			{
				// if having image info, just show in in a GUI.Box
				GUI.Box (new Rect (x + xSize, 0, widthOfInfoBox, Screen.height), myGUItexture, listStyle);
			}

			// Just show linked information in Unity desktop version
			#if UNITY_STANDALONE_WIN
			if(trainingScript != null && trainingScript.getCurrentStep() != null && trainingScript.getCurrentStep().LinkedInfo != "")
			{
				if(GUI.Button(new Rect((x + xSize) +  widthOfInfoBox/2 - findXPosFor(120), Screen.height - findYPosFor(80), findXPosFor(240) , findYPosFor(60)), "참고자료"))
				{						
						Application.OpenURL(Application.streamingAssetsPath + "/LinkedSource/"+ trainingScript.getCurrentStep().LinkedInfo);			
				}
			}
			#endif
		}

		/**
		 * Function is first used for dealing with viewport change
		 * but I (Ahn) use another easy way, so this function is doing nothing now.
		 * I keep it because it is good to apply in other situation or learn latter.
		 * It looks so complicated, doesnt it ? :)
		 */
		void SetVanishingPoint (Camera cam, Vector2 perspectiveOffset) 
		{
			Matrix4x4 m = cam.projectionMatrix;		
			float w = 2*cam.nearClipPlane/m.m00;		
			float h = 2*cam.nearClipPlane/m.m11;
		 
			float left = -w/2 - perspectiveOffset.x;
			float right = left+w;
			float bottom = -h/2 - perspectiveOffset.y;
			float top = bottom+h;
		 
			cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
		}
	 
		static Matrix4x4 PerspectiveOffCenter (float left, float right, float bottom, float top, float near, float far)
		{
			float x =  (float)(2.0 * near)		/ (float)(right - left);
			float y =  (float)(2.0 * near)		/ (float)(top - bottom);
			float a =  (float)(right + left)		/ (float)(right - left);
			float b =  (float)(top + bottom)		/ (float)(top - bottom);
			float c = -((float)far + (float)near)		/ (float)(far - near);
			float d = (float)-(2.0 * far * near) / (float)(far - near);
			float e = -1.0f;
		 
			Matrix4x4 m  = new Matrix4x4();;
			m[0,0] =   x;  m[0,1] = 0.0f;  m[0,2] = a;   m[0,3] = 0.0f;
			m[1,0] = 0.0f;  m[1,1] =   y;  m[1,2] = b;   m[1,3] = 0.0f;
			m[2,0] = 0.0f;  m[2,1] = 0.0f;  m[2,2] = c;   m[2,3] =   d;
			m[3,0] = 0.0f;  m[3,1] = 0.0f;  m[3,2] = e;   m[3,3] = 0.0f;
			return m;
		}

		#region implemented abstract members of MenuModeGUIBase
		public override void loadMode ()
		{
			return;
		}
		#endregion 
	}
}
