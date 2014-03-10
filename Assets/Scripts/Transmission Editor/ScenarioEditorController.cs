using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class ScenarioEditorController : MonoBehaviour {

	// Use this for initialization
	private float wFactor = 0.2f;
	private Process externalEditor;

	public Process ExternalEditor {
		get {
			return externalEditor;
		}
	}

	void Start () {
		wFactor = 0.32f;
		externalEditor = new Process();
		if(Application.loadedLevelName.Equals("TransmissionEditorMode"))
		{
			externalEditor.StartInfo.FileName = Application.dataPath + "/StreamingAssets/ExternalEditor/transmission/Editor.exe";
		}
		else
		{
			externalEditor.StartInfo.FileName = Application.dataPath + "/StreamingAssets/ExternalEditor/training/Editor.exe";
		}
		externalEditor.StartInfo.Arguments = ((int)(wFactor * Screen.width)) + " " + ((int)(Screen.height*0.7)) + " " +((int)((1920 - Screen.width)/2)) + " " + ((int)((1080 - Screen.height)/2)) ;
		//UnityEngine.Debug.Log("Kich thuoc " + ((int)(wFactor * Screen.width)) + " " + Screen.height);
		//externalEditor.StartInfo.Arguments = "600 600";
		//externalEditor.StartInfo.Arguments[1] = Screen.height + "";
		externalEditor.Start();
		UnityEngine.Debug.Log("KET THUC CHAY EDITOR ");
	}

	void OnGUI()
	{
	//	Camera.main.rect = new Rect(wFactor, 0.0f, 1.0f,1.0f);
		// wheel control for zoom
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			
			Camera.main.fieldOfView += 3.5f;
			if (Camera.main.fieldOfView >= 120) Camera.main.fieldOfView = 120;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			Camera.main.fieldOfView -= 3.5f;
			if (Camera.main.fieldOfView <= 15) Camera.main.fieldOfView = 15;
		}

	}
	void OnApplicationQuit () 
	{
		
		
		externalEditor.CloseMainWindow();
	
		
	
			Application.Quit();
		
	}

	void Update () {
	
	}
}
