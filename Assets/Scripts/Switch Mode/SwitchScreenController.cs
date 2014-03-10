using UnityEngine;
using System.Collections;

public class SwitchScreenController : MonoBehaviour {

	public static string screenName;	
	public static int loadingTextureIndex = 0;
	int currentAnimationIndex = 0;
	int timer = 1;
	public Texture2D[] loadingTexture;
	public Texture2D[] loadingAnimationImage;
	int loadingWidth;
	bool flag = false;
	int loadingHeight;
	// Use this for initialization
	void Start () {
		loadingWidth = loadingAnimationImage [0].width;
		loadingHeight = loadingAnimationImage [0].height;
		//StartCoroutine (LoadNewScreen());
	}

	IEnumerator LoadNewScreen() {
		AsyncOperation async = Application.LoadLevelAsync (screenName);
		yield return async;
	}
	
	// Update is called once per frame
	void Update () {
		timer ++;
		if (timer % 5 == 0) {
			currentAnimationIndex ++;
			if (currentAnimationIndex == (loadingAnimationImage.Length - 1)) {
				currentAnimationIndex = 0;
			}
			timer = 1;
		}
	}

	void OnGUI() {
		if (GUI.Button (new Rect (0, 0, 100, 100), "Next")) {
			StartCoroutine (LoadNewScreen());
			flag = true;
		}

		if (!flag)
			return;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), loadingTexture[loadingTextureIndex]);
		GUI.DrawTexture (new Rect (Screen.width / 2 - loadingWidth/2, Screen.height / 2 - loadingHeight/2, loadingWidth, loadingHeight), loadingAnimationImage [currentAnimationIndex]);

	}
}
