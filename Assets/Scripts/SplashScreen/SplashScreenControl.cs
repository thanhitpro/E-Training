using UnityEngine;
using System.Collections;
using ETraining.UI;
/**
 * Class does splash screen for introduction of E-Training Project.
 * The main ideas is simulating the fading effect by changing smoothly
 * value of alpha color.
 */ 
public class SplashScreenControl : MonoBehaviour {
	public Texture2D[] tex; /**< Holds all the splashscreens.*/
	public Texture2D whiteTexture; /**< White cover to be faded.*/
	public float faderSpeed; /**<  sets the fading speed.*/
	private float fader;    /**< holds the actual fade value.*/
	private Texture2D splash; /**< holds the currently displayed splash.*/
	private int indexTexture = 0; /**< Index of texture aray of splash images.*/
	public static bool isFirstTime = true;

	/**
	 * Based on platform, we have the specific number of GUI (splash images) needed.
	 */ 
	#if UNITY_STANDALONE_WIN
		private const int NUM_GUI = 1;
	#elif UNITY_ANDROID || UNITY_IPHONE
		private const int NUM_GUI = 1;
	#elif UNITY_EDITOR
		private const int NUM_GUI = 1;
	#endif
	// Use this for initialization
	IEnumerator Start () {	
		if (isFirstTime) {		
			splash = tex[0];
			for(int i=0; i<tex.Length;i++)
			{
				yield return StartCoroutine(FadeSplash(tex[i]));
			}
			splash = null;
		}
	}
	/**
	 * Method changes alpha value (fader) gradually by time
	 * @param pSplash : The splash screen needed to be fading
	 */ 
	IEnumerator FadeSplash(Texture2D pSplash)
	{
		// Set to initial state and assign image
		fader = 0;
		splash = pSplash;

		// Fade in
		while(fader<1)
		{
			yield return null;
			fader += Time.deltaTime * faderSpeed;
		}
		fader = 1;
		
		// Keep displayed for # seconds
		yield return new WaitForSeconds(1.0f);
		// Fade out
		while(fader>0)
		{
			yield return null;
			fader -= Time.deltaTime * faderSpeed;
		}
		fader = 0;
		indexTexture ++;
	}
	
	void OnGUI()
	{
		// If we run all texture then we dont run it any more
		if (indexTexture == NUM_GUI) {
			//After done splash screen, we never run splash screen.
			isFirstTime = false;
			return;
		}

		// Do splash screen changing using fading effect, which is simulated by alpha value of color.
		if(splash!=null)
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteTexture);
			GUI.color = new Color(255,255,255, fader);
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),splash);
			GUI.color = new Color(255,255,255,1);
		}
	}
}
