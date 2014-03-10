
using UnityEngine;
using System.Collections;
using ETraining;
using ETraining.UI.ToolControls;
using ETraining.UI.StepControls;
using ETraining.UI.VideoControls;

public class ReverseAnimationController : MonoBehaviour
{
    private Texture2D myGUItexture;
    private string fileName;
    
	public bool hasAnyToolsChanged = false;
    
	private bool resetTools = false;
	private bool isAutoNext = false;
	       		
	private int stepOrder = -1;
	private int taskOrder = -1;
	
	public GUISkin guiSkinNext;
	public GUISkin guiSkinPrev;
	
	public int x;
	public int y;
	private int xSize;
	private int ySize;

	
	public void setResetTools()
	{
		StartCoroutine(WaitToResetTools()) ;
		this.resetTools = true;
	}
	
	private int lastStepOrder;   
    

    private Step[] scenario;
	
   IEnumerator WaitInSeconds(float waitTime) {
        yield return new WaitForSeconds(waitTime);        
    }
	
	public	IEnumerator WaitToResetTools() 
	{        
        yield return StartCoroutine(WaitInSeconds(2.5F));   		
		resetTools = true;		
    }
	
	public	IEnumerator WaitToResetToolsPlier() 
	{       
        yield return StartCoroutine(WaitInSeconds(5.0F));   	
		resetTools = true;		
    }

	public int StepOrder {
		get {
			return this.stepOrder;
		}
		set {
			stepOrder = value;
		}
	}

	public int TaskOrder {
		get {
			return this.taskOrder;
		}
		set {
			taskOrder = value;
		}
	}	
	
	public Step getCurrentStep()
	{
		if(stepOrder == -1 || scenario == null || stepOrder == scenario.Length || scenario[stepOrder] == null) return null;
		return scenario[stepOrder];
	}
	
    private AbstractCommonTask currentTask = null;    
	
	public virtual void doExtraRequirement()
	{
	}

    public AbstractCommonTask getCurrentTask()
    {			
        return currentTask;
    }

    // Use this for initialization
    void displayInfo(string name)
    {
        WWW www = null;
		#if UNITY_STANDALONE_WIN
    		www = new WWW("file://" + Application.dataPath + "/StreamingAssets/Images/" + name);
		#elif UNITY_ANDROID
			www  = new WWW("file://"+ UnityEngine.Application.persistentDataPath +"/"+ name);
		#elif UNITY_IPHONE
		www = new WWW("file://"+ UnityEngine.Application.persistentDataPath + "/sample.xml");
		#endif		        

        myGUItexture = www.texture;
    }

    void Start()
    {
        	string filePath = null;	
		#if UNITY_STANDALONE_WIN
   			filePath = "file://" + Application.dataPath + "/StreamingAssets/Scenario/sample.xml";
		#elif UNITY_ANDROID
			filePath = "file://"+ UnityEngine.Application.persistentDataPath + "/Scenario/sample.xml";
		#elif UNITY_IPHONE
			filePath = "file://"+ UnityEngine.Application.persistentDataPath + "/sample.xml";
		#endif
		
	
		
        // Read data from xml
        XmlReader scenarioReader = new XmlReader();
        scenarioReader.loadXML(filePath);
        scenario = scenarioReader.AllStep;
		lastStepOrder = scenarioReader.Length;
		
//		GameObject [] listSelfMadeParts = GameObject.FindGameObjectsWithTag("self-made-part");
//
//		foreach(GameObject part in listSelfMadeParts)
//		{
//			Animator tempAnimator = part.GetComponent(typeof(Animator)) as Animator;
//			tempAnimator.ForceStateNormalizedTime(0.0f);
//			tempAnimator.SetInteger("Order", 2);
//		}
		
		
		
    }
	
	private bool wrongTools = true;
	
	public bool WrongTools {
		get {
			return this.wrongTools;
		}
		set {
			wrongTools = value;
		}
	}
    // Update is called once per frame
    void Update()
    {
		if(currentTask != null && currentTask.Component.getIsWrongTools())
			wrongTools = true;
			
		showCurrentComponentTools();
		 // wheel control for zoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            Camera.main.fieldOfView += 3.5f;
            if (Camera.main.fieldOfView >= 80) Camera.main.fieldOfView = 80;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.fieldOfView -= 3.5f;
            if (Camera.main.fieldOfView <= 15) Camera.main.fieldOfView = 15;
        }

		if(resetTools) // đoạn này để chờ 1 lúc rồi reset tool sau khi chạy animation xong
		{
			if(currentTask.GroupId != "1")
			{
				GameObject[] tools = GameObject.FindGameObjectsWithTag("tools");
				foreach (GameObject tool in tools)
				{
					ToolbarControl script = tool.GetComponent("ToolbarControl") as ToolbarControl;					
					if( script.used == true)  script.used = false;
				}
			}
			resetTools = false;
			isAutoNext = true;
			return;			
		}
//		else{
//			if(getCurrentTask() != null && getCurrentTask().GroupId == "1") 
//			{
//				isAutoNext = true;
//			}
//			else
//			{
//			}
//		}
		
		if(true)//isAutoNext)
		{ 
			autoNext();
			return;
		}
		
		NextButton nextButtonScript = GameObject.Find("NextButton").GetComponent(typeof(NextButton)) as NextButton;
		TittleNext tittleNextScript = GameObject.Find("TittleNext").GetComponent(typeof(TittleNext)) as TittleNext;

		if (nextButtonScript.used || tittleNextScript.used)
		{
			nextButtonScript.used = false;
			tittleNextScript.used = false;
			autoNext();		
		}
		
		PrevButton prevButtonScript = GameObject.Find("PrevButton").GetComponent(typeof(PrevButton)) as PrevButton;
		TittlePrev tittlePrevScript = GameObject.Find("TittlePrev").GetComponent(typeof(TittlePrev)) as TittlePrev;
        if (tittlePrevScript.used|| prevButtonScript.used)
		{
			prevButtonScript.used = false;
			tittlePrevScript.used = false;
			autoPrev();        	
		}
		
		
    }
	// anhnguyen sorrow experience: OnGUI just for GUI :(, which considered with frame by frame should be put on Update() instead
    void OnGUI()
    {    
//		if(wrongTools)
//		{
//			GUI.skin = warningBox;
//			GUI.Box(new Rect(Screen.width/2 - 150, Screen.height/2- 50, 300, 100),"Please choose other tool(s)");
//		}
    }
	
	private void autoNext()
	{		
		isAutoNext = false;
		if(stepOrder > -1 && stepOrder < lastStepOrder)
			{			  	
				 //1. Disable touch script
                //scenario[stepOrder].ListOfTask[taskOrder].Component.enableTouchScript(false);

                //2.Set the normal material
               // scenario[stepOrder].ListOfTask[taskOrder].Component.reset();

                //3. Set the end state
               // scenario[stepOrder].ListOfTask[taskOrder].setEndState();

                //4. Hide component if component should be hide as scenario shows
				if (scenario[stepOrder].ListOfTask[taskOrder].Component.HideAfterDisassembled)
                	scenario[stepOrder].ListOfTask[taskOrder].Component.showByEnableRenderer(false);

                //5. Hide related tool
             	scenario[stepOrder].ListOfTask[taskOrder].showTool(false);	
			
				//6. disable collider box
				//scenario[stepOrder].ListOfTask[taskOrder].Component.enableBoxCollider(false);
			}
			
			if(stepOrder == -1)	{stepOrder = 0; taskOrder = 0;}
			else if(stepOrder == lastStepOrder) 
			{
				return;
				stepOrder = lastStepOrder -1;
				taskOrder = scenario[stepOrder].ListOfTask.Length - 1;
			}
			else
			{
			
	            // Reset the index of Task to 0 if the tasks in previous step has been reached all
				taskOrder++;
	            if (scenario[stepOrder].ListOfTask.Length == taskOrder) // out of index
	            {
	                stepOrder++;
					if(stepOrder == lastStepOrder){
						return;
						stepOrder--;
						taskOrder = scenario[stepOrder].ListOfTask.Length - 1;
					}else taskOrder = 0;
	
	            }
			}
        	    // find current EngineComponent
            EngineComponent currentComponent = scenario[stepOrder].ListOfTask[taskOrder].Component;

            //1. Display information
           // fileName = scenario[stepOrder].ImagePath;
           // displayInfo(fileName);
			//ShowButtonInfo showInfoScript = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
			//showInfoScript.setGUITextTure(myGUItexture);
			
			//1. Display Step title
			//string stepTitle = scenario[stepOrder].Name;
			//StepTittle stepTitleScript = GameObject.Find("Step-Tittle").GetComponent(typeof(StepTittle)) as StepTittle;
			//stepTitleScript.setTitle(stepTitle);
			

            //2. Enable current TouchEnable Script
           // currentComponent.enableTouchScript(true);

            //3. Make current component distinctive
           // currentComponent.setRedSilhouette();

            //4. Make current component the initializing state
           // scenario[stepOrder].ListOfTask[taskOrder].setStartState();

            //5. Show component whether it's hidden
           // currentComponent.showByEnableRenderer(true);

            //6. Hide related tool whether it's hidden
           // scenario[stepOrder].ListOfTask[taskOrder].showTool(false);

            //7. Enable checktool process
           // hasAnyToolsChanged = true;

            
            //6. Get the current task -> make it public -> Touch component to enable animation
           // currentTask = scenario[stepOrder].ListOfTask[taskOrder];
			//9. Enable collider box
			//currentComponent.enableBoxCollider(true);
		
			//10. Do some extra requitment
			doExtraRequirement();
		
			//11 Show button video if video available

		
			#if UNITY_STANDALONE
			if(getCurrentStep() == null || getCurrentStep().VideoPath == "")
			{
				GameObject videoBtn = GameObject.Find("VideoButton") as GameObject;
				VideoButton videoBtnScript = videoBtn.GetComponent(typeof(VideoButton)) as VideoButton;
				videoBtnScript.setShowGUI(false);
			}
			else 
			{
				GameObject videoBtn = GameObject.Find("VideoButton") as GameObject;
				VideoButton videoBtnScript = videoBtn.GetComponent(typeof(VideoButton)) as VideoButton;
				videoBtnScript.setVideoName(getCurrentStep().VideoPath);
				videoBtnScript.setShowGUI(true);			
				
			}
			#endif	
		

		
	}
	
	private void autoPrev()
	{
		if(stepOrder < lastStepOrder && stepOrder > -1)
			{			  	
				 //1. Disable touch script
                scenario[stepOrder].ListOfTask[taskOrder].Component.enableTouchScript(false);

                //2.Set the normal material
                scenario[stepOrder].ListOfTask[taskOrder].Component.reset();

                //3. Set the end state
                scenario[stepOrder].ListOfTask[taskOrder].setStartState();

                //4. Hide component if component should be hide as scenario shows
                scenario[stepOrder].ListOfTask[taskOrder].Component.showByEnableRenderer(true);

                //5. Hide related tool
             	scenario[stepOrder].ListOfTask[taskOrder].showTool(false);	
			
				//6. Disable collider box
			  scenario[stepOrder].ListOfTask[taskOrder].Component.enableBoxCollider(false);
			}
			
			if(stepOrder == -1)	{stepOrder = 0; taskOrder = 0;}
			else if(stepOrder == lastStepOrder) 
			{
				stepOrder = lastStepOrder -1;
				taskOrder = scenario[stepOrder].ListOfTask.Length - 1;
			}
			else
			{
			
	            // Reset the index of Task to 0 if the tasks in previous step has been reached all
				taskOrder--;
	            if (-1 == taskOrder) // out of index
	            {
	                stepOrder--;
					if(stepOrder == -1) {stepOrder = 0; taskOrder = 0;}
					else taskOrder = scenario[stepOrder].ListOfTask.Length - 1;;
	
	            }
			}
								
            // find current EngineComponent
	            EngineComponent currentComponent = scenario[stepOrder].ListOfTask[taskOrder].Component;
	
	            //1. Display information
	            fileName = scenario[stepOrder].ImagePath;
	            displayInfo(fileName);
				ShowButtonInfo showInfoScript = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
				showInfoScript.setGUITextTure(myGUItexture);
			
				//1. Display Step title
				string stepTitle = scenario[stepOrder].Name;
				StepTittle stepTitleScript = GameObject.Find("Step-Tittle").GetComponent(typeof(StepTittle)) as StepTittle;
				stepTitleScript.setTitle(stepTitle);
	
	            //2. Enable current TouchEnable Script
	            currentComponent.enableTouchScript(true);
	
	            //3. Make current component distinctive
	            currentComponent.setRedSilhouette();
	
	            //4. Make current component the initializing state
	            scenario[stepOrder].ListOfTask[taskOrder].setStartState();
	
	            //5. Show component whether it's hidden
	            currentComponent.showByEnableRenderer(true);
	
	            //6. Hide related tool whether it's hidden
	            scenario[stepOrder].ListOfTask[taskOrder].showTool(false);
	
	            //7. Enable checktool process
	            hasAnyToolsChanged = true;
	
	            
	            //6. Get the current task -> make it public -> Touch component to enable animation
	            currentTask = scenario[stepOrder].ListOfTask[taskOrder];
		
		 		//9. Enable collider box
	            currentComponent.enableBoxCollider(true);
		
				//10. Do some extra requitment
				doExtraRequirement();
		
				//11. Show button video if video available
		
				#if UNITY_STANDALONE
				if(getCurrentStep() == null || getCurrentStep().VideoPath == "")
				{
					GameObject videoBtn = GameObject.Find("VideoButton") as GameObject;
					VideoButton videoBtnScript = videoBtn.GetComponent(typeof(VideoButton)) as VideoButton;
					videoBtnScript.setShowGUI(false);
				}
				else 
				{
					GameObject videoBtn = GameObject.Find("VideoButton") as GameObject;
					VideoButton videoBtnScript = videoBtn.GetComponent(typeof(VideoButton)) as VideoButton;
					videoBtnScript.setVideoName(getCurrentStep().VideoPath);
					videoBtnScript.setShowGUI(true);
				}
				#endif
	}
	
	void showCurrentComponentTools()
	{
			
		if (!hasAnyToolsChanged)
		{					
			return;
		}
		
	

		// Get all checked tools into an array of strings
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
		
		
		
		//compare to the tool from task
		
//		if(currentTask==null)
//		{
//			hasAnyToolsChanged = false;			
//			return;
//		}
		string toolsChosen = "";
		
		for (int j = 0; j < currentTask.ListOfTools.Length; j++) 
		{
			toolsChosen+=currentTask.ListOfTools[j].Id;
		
		}
		bool rightToolsChosen = true;
		int toolsChosenLength = 0;
		for (int k = 0; k < i; k++) {
			if(!toolsChosen.Contains(result[k])) rightToolsChosen = false;
			toolsChosenLength += result[k].Length;
		}
		if(rightToolsChosen && toolsChosenLength == toolsChosen.Length)
		{
			// Doing showing tools in here
			// 01. Make current tool for task shown
				currentTask.showTool(true);
				currentTask.Component.setRed();
				// 02. Make variable hasAnyToolsChanged become false to avoid next frame call
				hasAnyToolsChanged = false;
				wrongTools = false;
		}
		else
		{	
			currentTask.showTool(false);
			currentTask.Component.setRedSilhouette();
			hasAnyToolsChanged = false;			
			wrongTools = true;
		}
			
		
	}
}

