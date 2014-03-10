using UnityEngine;
using System.Collections;
using ETraining;
using ETraining.TrainingMode;


using ETraining.UI.ToolControls;

/**
 * This script is used to add into engine component
 * Everytime component is focused, this script is enable to receive touch.
 * 
 * This script is inherited BBSimpleTouchableObject, from another project called uniTouch
 * That we refer. uniTouch simply assigns the BBSimpleTouchableObject script to object needed
 * to handle by touch. We override the handleSingleTouch() function and do our logic requirement in that.
 * For understanding quickly, every time the object with BBSimpleTouchableObject script is touched by single touch,
 * The code in handleSingleTouch() will be running. We focus on handleSingleTouch only.
 */ 
public class TouchEnableAnimationResearch : BBSimpleTouchableObject {

	public bool allowDrag = false;
	public bool allowScale = true;
	public bool allowRotate = true;
	
	public float minimumScale = 1.0f;
	public float maximumScale = 3.0f;
	public GUISkin warningBox;
	private bool used = false;

	private Transform saveParent;
	private Vector3 movement;
	private Animator ani ;
	private bool isWrongTools = false;
	public AbstractCommonTask currentTask = null;

	public bool IsWrongTools {
		get {
			return this.isWrongTools;
		}
		set {
			isWrongTools = value;
		}
	}	
	public bool hideComponent = false;
	public bool firstEnable = true; // this script is first enabled
	private bool hasRunTheCurrentAnimation = true;
	public void setAnimationTouch(bool v) { this.hasRunTheCurrentAnimation = v;}
	private TrainingModeScenarioController tc;

	TouchEnableAnimationResearch()
	{
		firstEnable = true;
	}		
	
	void Awake()
	{
		warningBox = Resources.Load("WarningBox", typeof(GUISkin)) as GUISkin;
	
	}
	
	
	protected GameObject pivot;
	

	public override void handleSingleTouch(iPhoneTouch touch) 
	{

		// Get all tools are used, and assign their name to array result.
		string[]  result = new string [9];
		int i = 0;
		GameObject[] tools = GameObject.FindGameObjectsWithTag("tools");
		foreach (GameObject tool in tools)
		{
			ToolbarControl s;
	    	s = tool.GetComponent("ToolbarControl") as ToolbarControl;
			
			if( s.used == true) 
			{
				print("Tem cua script: "+ s.name);
				result[i++] = s.name.Substring(12, s.name.Length - 12);
			}
		}
		
		
		
							
		tc = Camera.main.GetComponent(typeof(TrainingModeScenarioController)) as TrainingModeScenarioController;				


		// save all tools needed for current task into toolsChosen
		string neededTools = "";		
		for (int j = 0; j < currentTask.ListOfTools.Length; j++) 
		{
			neededTools+=currentTask.ListOfTools[j].Id;
		
		}

		bool rightToolsChosen = true;
		int toolsChosenLength = 0;


		// loop all what tools have been selected. If at least one tool is not inside the toolsChosen (tools needed to do the task)
		// rrightToolsChosen is false immediately. Beside, we also save the name length of user chosen-tools
		for (int k = 0; k < i; k++) {
			if(!neededTools.Contains(result[k])) rightToolsChosen = false; // rightToolsChosen still right if set of needed tools still contains set of chosen tools
			toolsChosenLength += result[k].Length;
		}

		// if right tools are chosen and equal to list of needed tools ( not less than)
		if(rightToolsChosen && toolsChosenLength == neededTools.Length)
		{	
			if(hasRunTheCurrentAnimation)
			{
				// we need to wait animation before we auto reset the tools gui
				// we do a specific waiting for plier stops it animation because animatio of plier is designed longer than other normal animation.
				if(tc.StepOrder == 3 && tc.TaskOrder == 0)StartCoroutine(tc.WaitToResetToolsPlier());
				else StartCoroutine(tc.WaitToResetTools());
				currentTask.runAnimation();
				hasRunTheCurrentAnimation = false;
			}
			// when user touch the current component, this isWrongTools is decided, and in this case 
			// tools are wrongly choosen and this variable will be sent to EngineComponent of currentTask
			// And this value is used by ScenarioController to decide showing mesh model of tool or not
			isWrongTools = false;
		}
		else
		{
			isWrongTools = true;					
		}

		if (!allowDrag) return;
		// // we want to drag our object

		movement = touchMovementVector(touch);
		//		if (movement.sqrMagnitude > 0.01) {
				
		this.startPivot(gameObject.transform.position); 
		pivot.transform.Translate(movement,Space.World);
		this.endPivot();			
				
	}

	void OnGUI()
	{	
		// we use this to make warning box every time isWrongTools is true
		if(isWrongTools)
		{
			GUI.skin = warningBox;
			GUI.Box(new Rect(Screen.width/2 - 100, Screen.height/2 - 80, 200, 160),"Please choose other tool(s)");
			used = GUI.Toggle(new Rect(Screen.width/2 - 50, Screen.height/2 + 40, 100, 30),used, "OK");
			if(used || !tc.WrongTools)
			{
				isWrongTools = false;
			}
			
			
		}
		else used = false;
		
	}

	public override void handleDoubleTouch(ArrayList events) 
	{
				
		
		if (!allowRotate && !allowScale) return;
		// double touch can be a scale or a rotate, or both
		// 
		// let's do the rotate first
		// since this is a 2 touch gesture, we can only rotate in 2d, which in this case is in the camera plane
		// pivot on the lower touch index

		iPhoneTouch touch0 = (iPhoneTouch)events[0];
		iPhoneTouch touch1 = (iPhoneTouch)events[1];
		if (touch0.fingerId > touch1.fingerId) {
			// flip them, 0 should be the earlier index
			touch0 = (iPhoneTouch)events[1];
			touch1 = (iPhoneTouch)events[0];
		}

	  this.startPivot(gameObject.transform.position); 

		// 	
		// 	//////////////////////////////// ROTATE
		// 	
		
		float zDistanceFromCamera = Vector3.Distance(renderingCamera.transform.position,gameObject.transform.position);

		Vector3 screenPosition0 = new Vector3(touch0.position.x,touch0.position.y,zDistanceFromCamera);
		Vector3 lastScreenPosition0 = new Vector3(touch0.position.x - touch0.deltaPosition.x,touch0.position.y - touch0.deltaPosition.y,zDistanceFromCamera);

		Vector3 screenPosition1 = new Vector3(touch1.position.x,touch1.position.y,zDistanceFromCamera);
		Vector3 lastScreenPosition1 = new Vector3(touch1.position.x - touch1.deltaPosition.x,touch1.position.y - touch1.deltaPosition.y,zDistanceFromCamera);


		float angleNow = Mathf.Atan2(screenPosition0.x - screenPosition1.x, screenPosition0.y - screenPosition1.y) * Mathf.Rad2Deg;
		float angleThen = Mathf.Atan2(lastScreenPosition0.x - lastScreenPosition1.x, lastScreenPosition0.y - lastScreenPosition1.y) * Mathf.Rad2Deg;
		
		float angleDelta = angleNow - angleThen;

	 	if (allowRotate) pivot.transform.RotateAround(gameObject.transform.position,renderingCamera.transform.position- gameObject.transform.position,angleDelta);

		// 
		// 	///////////////////////////  SCALE
		// 
		if (allowScale) {
			float distNow = (screenPosition0 - screenPosition1).magnitude;
			float distThen = (lastScreenPosition0 - lastScreenPosition1).magnitude;
		 
			float scale = distNow/distThen;
		
			// presume for the time being that our scales are uniform
			if (transform.localScale.x * scale < minimumScale) scale = minimumScale/transform.localScale.x;
			if (transform.localScale.x * scale > maximumScale) scale = maximumScale/transform.localScale.x;
	
			Vector3 local = pivot.transform.localScale;
			
			local.x *= scale;
			local.y *= scale;
			local.z *= scale;
			pivot.transform.localScale = local;
		}

	 	this.endPivot();
	}
	
	virtual protected void startPivot(Vector3 pivotPosition)
	{			
		if (pivot == null) {
			pivot = new GameObject();
			pivot.name = "BBBasicTouchManipulation Pivot";
			pivot.transform.position = pivotPosition;		
		}	

		saveParent = gameObject.transform.parent;
		gameObject.transform.parent = null;
		pivot.transform.parent = saveParent;
		gameObject.transform.parent = pivot.transform;
	}

	virtual protected void endPivot()
	{
		gameObject.transform.parent = saveParent;		
		pivot.transform.parent = null;	
		Destroy(pivot);	
	}

	public Vector3 touchMovementVector(iPhoneTouch touch) 
	{
		float zDistanceFromCamera = Vector3.Distance(renderingCamera.transform.position,gameObject.transform.position);

		Vector3 screenPosition = new Vector3(touch.position.x,touch.position.y,zDistanceFromCamera);
		Vector3 lastScreenPosition = new Vector3(touch.position.x - touch.deltaPosition.x,touch.position.y - touch.deltaPosition.y,zDistanceFromCamera);

		Vector3 cameraWorldPosition = this.renderingCamera.ScreenToWorldPoint(screenPosition);
		Vector3 lastCameraWorldPosition = this.renderingCamera.ScreenToWorldPoint(lastScreenPosition);

		return cameraWorldPosition - lastCameraWorldPosition;
	}


}
