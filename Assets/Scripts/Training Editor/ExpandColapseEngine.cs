using UnityEngine;
using System.Collections;
using ETraining;
using ETraining.TrainingMode;
/**
 * This class is used in Unity Scenario Editor mode. When user wants to choose a component to apply back
 * to window-based editor, he can expand the whole engine to choose the inner component of engine easily.
 * 
 * This class also does controlling component picking (by right mouse click), and transfer information to window-base
 * client.
 */ 

abstract public class ExpandColapseEngine : MonoBehaviour
{
	
	private TrainingModeScenarioController tc;
	private string btnTitle = "Expand";
	private bool firstChoose = true;
	private string prevHitted = "";
	
	public string PrevHitted
	{
		get
		{
			return prevHitted;
		}
		set
		{
			prevHitted = value;
		}
	}
	
	private bool expand = false;
	Vector2 downPos, upPos, oldPos;
	private string dataToClient = "";
	EngineComponent prevEc;
	HandleClient scriptHandle;
	
	
	public string DataToClient
	{
		get
		{
			return dataToClient;
		}
		set
		{
			dataToClient = value;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		oldPos = new Vector2(0, 0);
		scriptHandle = transform.GetComponent(typeof(HandleClient)) as HandleClient;
	}
	
	void OnGUI()
	{
		//Show or hide button and call all animators to set animation order 1 (as run), or 0 (as start state)
		// Respectively
		if (GUI.Button(new Rect(Screen.width - 100, 0, 90, 90), btnTitle))
		{
			expand = !expand;
			int aniOrder;
			if (expand)
				aniOrder = 1;
			else
				aniOrder = 0;
			if (btnTitle == "Expand")
				btnTitle = "Collapse";
			else
				btnTitle = "Expand";
			GameObject[] engineEditor = GameObject.FindGameObjectsWithTag("engine-editor");
			foreach (GameObject task in engineEditor)
			{
				Animator animator = task.GetComponent(typeof(Animator)) as Animator;
				animator.ForceStateNormalizedTime(0.0f);
				animator.SetInteger("Order", aniOrder);
			}
		}
	}
	
	private int countClick = 0; /**< Number of right mouse clicks we click.*/
	
	void Update()
	{
		
		
		if (Input.GetMouseButtonDown(1))
		{
			// if right mouse click down but not release mouse, we save the down click position
			downPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			GameObject cube = GameObject.Find("Cube");
			// move touch cube to behind the camera, it means disabling the touch orbit.
			cube.transform.position = Camera.main.transform.position - new Vector3(0.0f, 0.0f, 0.5f);

		}
		if (Input.GetMouseButtonUp(1))
		{
			// if rigt mouse click is now released, we save the release position of right mouse
			upPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			// if mouse up position is no longer than 4 pixels to old (up) position then we 
			// count that click up by 1. This click is in a continuous sequence of clicks.
			// this is to implement purpose: click continuously to choose expected component
			if (Vector2.Distance(upPos, oldPos) < 4)
			{ 
				countClick++;				
			}
			else
			{
				//otherwise, upPos and reset countClick
				oldPos = upPos;
				countClick = 0;
			}

			// downPos and upPos almost same, so here is the correct right mouse click.
			// it avoids click down -> drag to another position -> release up.
			if (Vector2.Distance(downPos, upPos) < 1)
			{

				// Use ray cast to detect which object is hitted by ray
				RaycastHit[] hits;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				hits = Physics.RaycastAll(ray, Mathf.Infinity);

				if (hits.Length == 0) return;


				// countClick is also the index to get info from hits array.
				// so just reset it to zero if it reach hits.Length
				if (countClick >= hits.Length)
					countClick = 0;

				RaycastHit hit = hits[countClick];
				GameObject hitted = hit.collider.gameObject;
				if (hitted.name == "Cube")
					return;

				// Stuff below is for choosing component by right click
				// set it yellow with silhouette, set prev component to normal material.
				// if new component is picked, then just send its name to client.
				if ((GameObject.Find(hitted.name)) != null && !hitted.name.Contains("tr049") && hitted.name != scriptHandle.componentName)
				{
					Debug.DrawLine(ray.origin, hit.point);
					if (prevHitted != hitted.name)
					{
						EngineComponent ec = new EngineComponent("", hitted.name, false);
						ec.enableBoxCollider(true);
						ec.reset();
						ec.setYellSilhouette();
						dataToClient = hitted.name;
						string child = hitted.name;
						GameObject childGO = GameObject.Find(child);
						GameObject f1GO = null;
						while (childGO.transform.parent != null && childGO.transform.parent.gameObject)
						{
							f1GO = childGO;
							childGO = childGO.transform.parent.gameObject;
							
						}
						dataToClient += "|" + f1GO.name;															
						Component[] childs = f1GO.GetComponentsInChildren(typeof(Transform));

						if (childs != null && childs.Length > 0)
						{
							foreach (Component cw in childs)
							{								
								Transform go = (Transform)cw;
								if (go.name.Contains("sorkets") || go.name.Contains("wrench") || go.name.Contains("EN_PCV_sorkets_"))
								{
									if (go.transform.childCount == 1)
									{
										dataToClient += "|" + go.transform.GetChild(0).gameObject.name;
									}
									else if (go.transform.childCount == 2)
									{
										dataToClient += "|" + go.transform.GetChild(0).gameObject.name + "," + go.transform.GetChild(1).gameObject.name;
									}
									else
									{
										dataToClient += "|" + go.name;
									}
									break;
								}								
							}
						}


						
						if (prevHitted != "")
						{
							prevEc.reset();
						}
						
						prevEc = ec;
						prevHitted = hitted.name;
					}									
				}				
			}

			//after right mouse click is released up, just let touch Cube return it position between camera and engine.
			Vector3 position = Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find("engine_LOW").transform.position;
			GameObject cube;
			cube = GameObject.Find("Cube");
			cube.transform.position = position;
		}
		
	}
	
	abstract protected bool doNotNeedTools(string componentName);
	abstract protected string chooseAniOrder(string componentName);
}