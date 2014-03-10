
using UnityEngine;
using System.Collections;
using ETraining;
using ETraining.TrainingMode;

public class TransmissionEditorController : MonoBehaviour {
	
	private TrainingModeScenarioController tc;
	private string btnTitle = "Expand";
	private string prevHitted = "";
	
	public string PrevHitted {
		get {
			return prevHitted;
		}
		set {
			prevHitted = value;
		}
	}
	
	private bool expand = false;
	Vector2 downPos, upPos, oldPos;
	private string dataToClient = "";
	EngineComponent prevEc;
	HandleClient scriptHandle;
	public string DataToClient {
		get {
			return dataToClient;
		}
		set {
			dataToClient = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		oldPos = new Vector2(0,0);
		scriptHandle = transform.GetComponent(typeof(HandleClient)) as HandleClient;
	}
	
	
	void OnGUI()
	{
		//Camera.main.rect = new Rect(0.2f, 0.0f, 1.0f,1.0f);
		if(GUI.Button(new Rect(Screen.width - 100, 0, 90, 90), btnTitle))
		{
			expand = !expand;
			int aniOrder;
			if(expand) aniOrder = 1; else aniOrder = 0;
			if (btnTitle == "Expand") btnTitle = "Collapse"; else btnTitle = "Expand";
			GameObject []engineEditor = GameObject.FindGameObjectsWithTag("engine-editor");
			foreach (GameObject task in engineEditor)
			{
				Animator animator = task.GetComponent(typeof(Animator)) as Animator; 	
				animator.ForceStateNormalizedTime(0.0f);		
				animator.SetInteger("Order", aniOrder);	
			}
		}
	}
	private int countClick = 0;
	void Update () {
		
		
		if(Input.GetMouseButtonDown(1)) 
		{
			downPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			GameObject cube;
			cube = GameObject.Find("Cube");
			//print ("dua cube len truoc");
			cube.transform.position = Camera.main.transform.position - new Vector3(0.0f,0.0f, 0.1f) ;
			
			// Dua tro ve truoc as same as OrbitTouch code
			//var distance = GameObject.Find("touchCube").GetComponent("OrbitTouch").distance;
			//Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find("engine_LOW").transform.position;
			
			//cube.transform.position = position;
		}
		if(Input.GetMouseButtonUp(1))
		{
			upPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if(Vector2.Distance(upPos,oldPos) < 4) // 2 click cach nhau 4 pixel
			{
				countClick ++;
				
			}
			else
			{
				oldPos = upPos;
				countClick = 0;
			}
			if( Vector2.Distance(downPos, upPos)< 1)
			{
				
				RaycastHit[] hits;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				hits = Physics.RaycastAll(ray, Mathf.Infinity);
				
				
				
				Debug.Log("CHIEU DAI CUA LIST HITTED " + hits.Length);
				if(countClick >= hits.Length) countClick = 0;	
				
				//RaycastHit hit = hits[hits.Length - countClick - 1];
				RaycastHit hit = hits[countClick];
				GameObject hitted  = hit.collider.gameObject;
				if(hitted.name == "Cube" ) return;
				
				if((GameObject.Find(hitted.name)) != null && !hitted.name.Contains("tr049") && hitted.name != scriptHandle.componentName)
				{
					Debug.DrawLine(ray.origin,hit.point);
					if(prevHitted !=  hitted.name)
					{
						EngineComponent ec = new EngineComponent("", hitted.name, false);
						ec.enableBoxCollider(true);
						ec.reset();
						ec.setYellSilhouette();
						dataToClient = hitted.name;
						
						if(prevHitted != "")
						{
							//EngineComponent prev = new EngineComponent("", prevHitted, false);
							prevEc.reset();
						}
						
						prevEc = ec;
						prevHitted = hitted.name;
					}
					
					
				}
				
			}
			Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find("engine_LOW").transform.position;
			GameObject cube;
			cube = GameObject.Find("Cube");
			cube.transform.position = position;
		}
		
	}
}