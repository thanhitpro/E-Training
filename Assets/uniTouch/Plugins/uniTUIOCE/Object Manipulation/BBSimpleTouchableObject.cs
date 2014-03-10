using UnityEngine;
using System.Collections;

// requires a collider of some sort

// this simple object checks to see if any of the touches that are currently
// active are raycasting through it's collider
// of so, it passes those touches to the appropriate
// methods.
using ETraining.UI.StepControls;

public class BBSimpleTouchableObject : MonoBehaviour {

	private ArrayList thisFrameEvents = new ArrayList();
	protected Camera renderingCamera;

	//anhnguyen
	private ShowButtonInfo scrip;
	private Vector3 screenPosition;
	private bool foundScript = false;
	private bool firstFound = false;


	// Use this for initialization
	void Start () {
		noTouches();
		startup();
		renderingCamera = Camera.main;
		if (renderingCamera == null) {
			// someone didnt tag their cameras properly!!
			// just grab the first one
			if (Camera.allCameras.Length == 0) return;
			renderingCamera = Camera.allCameras[0];
		}
	}

	virtual public void startup() {
		// a place to put any post-start code	
	}
	
	void Update() 
	{
		checkForTouches();
		if (thisFrameEvents.Count > 0) {
			distributeTouches();
		} else {
			noTouches();
		}
	}
	
	public void checkForTouches()
	{
		// some defensive programming
		// check to make sure that we have a collider and that we found a camera
		if (gameObject.collider == null) {
//			Debug.Log("Object: " + gameObject.name + " is trying to collect touches but has no collider");
			return;
		}
		if (renderingCamera == null) {
			Debug.Log("Object: " + gameObject.name + " cannot find a camera.");
			return;			
		}
		
		// clear out our frame event buffer
		thisFrameEvents.Clear();
		RaycastHit hit = new RaycastHit(); // need one of these to check for hits
		
		// step through each touch and see if any are hitting me
		int i;
		for (i = 0; i < iPhoneInput.touchCount; i++) {
			iPhoneTouch touch = iPhoneInput.GetTouch(i);
			
			//anh nguyen -- must count again touch.position in case viewport has changed
			

			if(!firstFound) // ngay mai loai doan nay ra
			{
				if(GameObject.Find("ShowInfoButton")) 
				{
					scrip = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
					foundScript = true;
				}
				firstFound = true;
			}
			else
			{
				if(foundScript)
				{
					float xPox = touch.position.x * scrip.getViewportRatio();
			 		screenPosition = new Vector3(xPox,touch.position.y,0.0f);
				}
				else
				{
					screenPosition = new Vector3(touch.position.x,touch.position.y,0.0f);
				}
			}
			//else screenPosition = new Vector3(touch.position.x,touch.position.y,0.0f);
			// original Vector3 screenPosition = new Vector3(touch.position.x,touch.position.y,0.0f);
			
			//anh nguyen -- Must convert screen point to viewport point first and then viewportpoint to RAY
			//Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPosition);
			//Vector3 viewportPos1 = Camera.main.ViewportToScreenPoint(screenPosition);
		
			
			
			if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition),out hit,Mathf.Infinity)) { 
				// do we have a hit?
				if (hit.transform.gameObject == gameObject) 
					thisFrameEvents.Add(touch);
			}
			
			/* this code is original, Anh Nguyen made code a little change as above
			if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition),out hit,Mathf.Infinity)) { 
				// do we have a hit?
				if (hit.transform.gameObject == gameObject) thisFrameEvents.Add(touch);
			}*/
		}
	}
	
	public void distributeTouches()
	{
		// how many touches do we have? 
		if (thisFrameEvents.Count == 1) {
			handleSingleTouch(thisFrameEvents[0] as iPhoneTouch);
			return;
		}
		if (thisFrameEvents.Count == 2) {
			handleDoubleTouch(thisFrameEvents);
			return;
		}
		handleManyTouches(thisFrameEvents);
	}
		
	virtual public void handleManyTouches(ArrayList touches) {} // more than two
	virtual public void handleDoubleTouch(ArrayList touches) {} // two touches
	virtual public void handleSingleTouch(iPhoneTouch aTouch) {} // just one touch 
	
	virtual public void noTouches() {} // reset or clear the state

}
