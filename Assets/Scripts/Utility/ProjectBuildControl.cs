using UnityEngine;
using System.Collections;

namespace ETraining.Utility
{
	public class ProjectBuildControl : MonoBehaviour 
	{		
		// Use this for initialization
		void Awake () 
		{
			#if UNITY_STANDALONE_WIN

			//1. Camera
			Camera.main.clearFlags =  CameraClearFlags.Skybox;
			Camera.main.renderingPath = RenderingPath.UsePlayerSettings;
			Skybox sb = Camera.main.GetComponent(typeof(Skybox)) as Skybox;
			sb.enabled = true;

			//2. Splash screen
			GameObject SplashScreen = GameObject.Find("Splash Screen Android");
			if(SplashScreen) SplashScreen.SetActive(false);

			//3. Light
			GameObject.Find("Light for Tablet Version").SetActive(false);

			#elif UNITY_ANDROID || UNITY_IPHONE

			//1. Camera
			Camera.main.clearFlags =  CameraClearFlags.SolidColor;
			Camera.main.renderingPath = RenderingPath.VertexLit;
			Skybox sb = Camera.main.GetComponent(typeof(Skybox)) as Skybox;
			sb.enabled = false;

			//2. Splash screen
			GameObject SplashScreen = GameObject.Find("Splash Screen Window");
			if(SplashScreen) SplashScreen.SetActive(false);

			//3. Video GUI and movie player
			GameObject.Find("Video GUI").SetActive(false);
			GameObject.Find("moviePlayer").SetActive(false);

			//4. Light
			GameObject.Find("Light for Window Version").SetActive(false);

			
			//5. Disable EditScenarioButton and OpenScenario button in Tablet version
			if(GameObject.Find("EditScenarioButton"))
			{
				GameObject.Find("EditScenarioButton").SetActive(false);
			}
			if(GameObject.Find("OpenScenario"))
			{
				GameObject.Find("OpenScenario").SetActive(false);
			}

			//6. Deactive tranining multi-touch control
			GameObject.Find("Training Multi-touch Control").SetActive(false);

			//7. Disable WMTouchInput and TouchOptions in main camera
			(Camera.main.GetComponent("TouchOptions") as MonoBehaviour).enabled = false;
			(Camera.main.GetComponent("WMTouchInput") as MonoBehaviour).enabled = false;

			#endif
		}	
	}
}
