using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ETraining.UI.StepControls;
using ETraining.UI.ToolControls;
using ETraining.UI.VideoControls;

namespace ETraining
{
	
	/**
 	 * This class is the most important class in project. 
 	 * It controls the main learning process of system, connect other scripts together,
 	 * deliver the input and output of other functions as well as scripts.
 	 */ 
	public class ScenarioController : MonoBehaviour
	{
		private Texture2D emptyTex = null;
	    private Texture2D myGUItexture = null;
	    private string imageFileName = "";
		public static string scenarioFileName = "sample.xml";
	    
		public bool hasAnyToolsChanged = false;
		public bool hasSelectedTool = false;
	    
		private bool resetTools = false;
		private bool isAutoNext = false;
		       		
		private static  int stepOrder = -1;
		private static int taskOrder = -1;

		private bool isSetForNext = false;
		private bool isSetForPrev = false;

		private NextButton nextButtonScript;
		private TittleNext tittleNextScript;
		private PrevButton prevButtonScript;
		private TittlePrev tittlePrevScript;
		public static bool resetFlag = false;
		public static bool finishResetEngine = false;
		private bool finishResetEngineFlag = false;
		private static Dictionary<string, WWW> dictionaryWWW = null;
		private GUIStyle boxStyle;
		private float xTextPostion = Screen.width;
		private bool checkPressScenario = false;
		public GUISkin warningBox;

		/// <summary>
		/// The show error log flag;
		/// </summary>
		public static bool showErrorLog = false;

		/**
		 * Only call PlayMovie script in Desktop Windows version of E-Training
		 */ 
		#if UNITY_STANDALONE
		private PlayMovie scriptVideo = null;
		#endif

		protected EngineComponent currentComponent = null;


		private static string[] listImageNames;
		static string filePath = null;	


		public void setResetTools()
		{
			StartCoroutine(WaitToResetTools()) ;
			this.resetTools = true;
		}
		
		private static int lastStepOrder;   
	    

	    private static Step[] scenario;
		
	   IEnumerator WaitInSeconds(float waitTime) {
	        yield return new WaitForSeconds(waitTime);        
	    }

		IEnumerator LoadXmlFile() {
			yield return new WaitForSeconds(1.0f);
			LoadDataFromXmlFile();
			ScenarioController.showErrorLog = false;
			ErrorLogManager.exportNormalLog();
		}
		
		public	IEnumerator WaitToResetTools() 
		{        
			if(Application.loadedLevelName == "ReverseTrainingMode") yield return StartCoroutine(WaitInSeconds(1.85F));   
			else yield return StartCoroutine(WaitInSeconds(2.5F));   				        
			resetTools = true;		
	    }
		
		public	IEnumerator WaitToResetToolsPlier() 
		{       
	        yield return StartCoroutine(WaitInSeconds(5.0F));   	
			resetTools = true;		
	    }

		public int StepOrder { 
			get {
				return ScenarioController.stepOrder;
			}
			set {
				stepOrder = value;
			}
		}

		public int TaskOrder {
			get {
				return ScenarioController.taskOrder;
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


	  	/**
	  	 * This method is to assigned the information image to a GUITexture.
	  	 * GUITexture in turn will be used to create information box in \ref ShowButtonInfo
	  	 * @param name : The name of information image read from xml file.
	  	 */ 
	    void displayInfo(string name)
	    {
			// Unity leaks memory when working with 2D texture assignment
			// We do some release memory tasks to avoid it
			myGUItexture = emptyTex;
			Resources.UnloadUnusedAssets ();
			Resources.UnloadAsset (myGUItexture);
			Destroy (myGUItexture);

			// assign information image to GUI texture.
			myGUItexture = dictionaryWWW[name].texture;
	    }
		void Awake()
		{
			emptyTex = new Texture2D (1, 1);
			dictionaryWWW = new Dictionary<string, WWW>();
			warningBox = Resources.Load("WarningBox", typeof(GUISkin)) as GUISkin;
		}
		void Start()
	    {
			#if UNITY_STANDALONE
			scriptVideo = GameObject.Find("moviePlayer").GetComponent(typeof(PlayMovie)) as PlayMovie;
			#endif

	        // Read data from xml
			LoadDataFromXmlFile();

			ErrorLogManager.exportNormalLog();

			boxStyle = new GUIStyle ();
			boxStyle.normal.background = new Texture2D (1, 1);
			boxStyle.normal.textColor = new Color (0.3f, 0.3f, 0.3f);
			boxStyle.alignment = TextAnchor.UpperCenter;
			boxStyle.normal.background.SetPixel (0, 0, new Color(0, 0, 0));
			boxStyle.normal.background.Apply ();
			boxStyle.padding.left =
				boxStyle.padding.right =
					boxStyle.padding.top =
					boxStyle.padding.bottom = 0;

			for (int i = 0; i < listImageNames.Length; i++) 
			{
				WWW www = new WWW("file://" + listImageNames[i]);
				StartCoroutine(waitForLoadResource(listImageNames[i], www));
			}
	    }

		/**
		 * This function is used to load external data which have names defined in xml scenario file.
		 * It also read xml content to create object model representing for Object Model in our project.
		 * This is loading external files based on the path following the specific platform such as Windows,
		 * Android, or Ios.
		 * 
		 */ 
		public static void LoadDataFromXmlFile() {
			ErrorLogManager.resetLog();
			#if UNITY_STANDALONE
			if (listImageNames == null || listImageNames.Length == 0) {
				listImageNames = Directory.GetFiles(Application.dataPath + "/StreamingAssets/Images/", "*.png");
			}
			if(Application.loadedLevelName == "ReverseTrainingMode")
				filePath = "file://" + Application.dataPath + "/StreamingAssets/Scenario/reverse.xml";
			else if (Application.loadedLevelName == "TransmissionMode")
			{
				//string fileXml = FileChooser.Apply();
				//Debug.Log("DUONG DAN XML LA " + fileXml);
				//if(fileXml == null || fileXml == "")
				filePath = "file://" + Application.dataPath + "/StreamingAssets/Scenario/transmission.xml";
				//else
				//	filePath = "file://" + fileXml;					
			}
			else
				filePath = "file://" + Application.dataPath + "/StreamingAssets/Scenario/" + scenarioFileName;
			
			#elif UNITY_ANDROID
			if (listImageNames == null || listImageNames.Length == 0) {
				listImageNames = Directory.GetFiles(Application.persistentDataPath, "*.png");
			}

			if(Application.loadedLevelName == "ReverseTrainingMode")
				filePath = Application.persistentDataPath + "/Scenario/reverse.xml";
			else
				filePath = Application.persistentDataPath + "/Scenario/" + scenarioFileName;
			#elif UNITY_IPHONE
			if (listImageNames == null || listImageNames.Length == 0) {
				listImageNames = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");
			}

			if(Application.loadedLevelName == "ReverseTrainingMode")
				filePath = "file://" + Application.persistentDataPath + "/reverse.xml";
			else
				filePath = "file://"+ Application.persistentDataPath + "/" + scenarioFileName;
			
			 //Config to run on Mac Editor
//						listImageNames = Directory.GetFiles(UnityEngine.Application.dataPath + "/StreamingAssets/Images/", "*.png");
//							print (listImageNames.Length);
//						if(Application.loadedLevelName == "ReverseTrainingMode")
//							filePath = "file://" + UnityEngine.Application.dataPath + "/StreamingAssets/Scenario/reverse.xml";
//						else
//							filePath = "file://"+ UnityEngine.Application.dataPath + "/StreamingAssets/Scenario/sample.xml";
			
			#elif UNITY_EDITOR
			if (listImageNames == null || listImageNames.Length == 0) {
				listImageNames = Directory.GetFiles(UnityEngine.Application.dataPath + "/StreamingAssets/Images/", "*.png");
			}
			if(Application.loadedLevelName == "ReverseTrainingMode")
				filePath = "file://" + UnityEngine.Application.dataPath + "/StreamingAssets/Scenario/reverse.xml";
			else
				filePath = "file://"+ UnityEngine.Application.dataPath + "/StreamingAssets/Scenario/" + scenarioFileName;
			
			#endif
			XmlReader scenarioReader = new XmlReader();
			scenarioReader.loadXML(filePath);
			scenario = scenarioReader.AllStep;
			lastStepOrder = scenarioReader.Length;
			stepOrder = -1;
			taskOrder = -1;
			ShowButtonInfo showInfoScript = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
			showInfoScript.setGUITextTure(null);
		}

		/**
		 * This method makes system wait for loading all the external nformation images
		 * before running into main process of step by step training.
		 * @param imageName : Name of image that is used to put in WWW
		 * @param www: The object to load external resource 
		 * \ref http://docs.unity3d.com/Documentation/ScriptReference/WWW.html
		 */ 
		IEnumerator waitForLoadResource(string imageName, WWW www) {
			yield return www;
			if (www!= null && www.error == null && www.isDone) 
			{
				dictionaryWWW.Add(imageName.Substring(imageName.LastIndexOf('/') + 1), www);
			}
		}



		public void restart()
		{
			Start ();
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
			GUIStyle newGUIStyle = new GUIStyle();
			newGUIStyle.alignment = TextAnchor.MiddleLeft;
			newGUIStyle.normal.textColor = Color.red;
			newGUIStyle.fontSize = 15;

			float length = newGUIStyle.CalcSize(new GUIContent(ErrorLogManager.normalErrorLog + " " + ErrorLogManager.warningLog)).x;

			if (xTextPostion >= -length) 
				xTextPostion -= (Time.deltaTime * 100);
			else {
				xTextPostion = Screen.width;
			}
			if(currentTask != null && currentTask.Component.getIsWrongTools())
			{			
				wrongTools = true;
			}

			// call this function to check whether user is choosing the right tools or not to show the tool model (3d mesh of tool)
			showCurrentComponentTools();
			 
			// wheel control for zoom in/out the camera.
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

			if(resetTools) // wait for a while until animation is done, to reset tools
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

			
			if(isAutoNext)
			{ 
				autoNext();
				return;
			}

			if(!isSetForNext) // only find NextButton and TittleNext once
			{
				nextButtonScript = GameObject.Find("NextButton").GetComponent(typeof(NextButton)) as NextButton;
				tittleNextScript = GameObject.Find("TittleNext").GetComponent(typeof(TittleNext)) as TittleNext;
				isSetForNext = true;
			}

			// The next button in right side bar or the next button in title bar is touched or clicked
			if (nextButtonScript.used || tittleNextScript.used)
			{
				// reset them to false, it means not clicked or touched
				nextButtonScript.used = false;
				tittleNextScript.used = false;

				// run to next task
				if (ErrorLogManager.errorFlag) {
					checkPressScenario = true;
					return;
				}
				autoNext();		
			}
			if (!isSetForPrev) // only find PrevButton and TittlePrev once
			{
				prevButtonScript = GameObject.Find("PrevButton").GetComponent(typeof(PrevButton)) as PrevButton;
				tittlePrevScript = GameObject.Find("TittlePrev").GetComponent(typeof(TittlePrev)) as TittlePrev;
				isSetForPrev = true;
			}

			// The prev button in right side bar or the prev button in title bar is touched or clicked
			if (tittlePrevScript.used|| prevButtonScript.used || ScenarioController.resetFlag)
			{
				// reset them to false, it means not clicked or touched
				prevButtonScript.used = false;
				tittlePrevScript.used = false;

				// run back to previous task
				if (ErrorLogManager.errorFlag && !ScenarioController.resetFlag) {
					checkPressScenario = true;
					return;
				}

				if (ErrorLogManager.errorFlag && ScenarioController.resetFlag) {
					ScenarioController.finishResetEngine = true;
					ScenarioController.resetFlag = false;
					
					StartCoroutine(LoadXmlFile());
					return;
				}

				autoPrev();        	
			}
			
			
	    }
		// anhnguyen sorrow experience: OnGUI just for GUI :(, which considered with frame by frame should be put on Update() instead
	    void OnGUI()
	    {    
			if (showErrorLog) {
				return;
			}
			GUI.backgroundColor = Color.gray;
			// Draw the error logs

			if (ErrorLogManager.criticalErrorLog != "") {
				GUI.Box(new Rect(0, Screen.height/2 - 50, Screen.width, 100), "", boxStyle);
				if (GUI.Button(new Rect(Screen.width/2 - 30, Screen.height/2 + 15, 60, 25), "OK")) {
					showErrorLog = true;
				}
				
				GUIStyle newGUIStyle = new GUIStyle();
				newGUIStyle.alignment = TextAnchor.MiddleCenter;
				newGUIStyle.normal.textColor = Color.white;
				newGUIStyle.fontSize = 17;
				
				GUI.TextArea(new Rect(Screen.width/2 - ErrorLogManager.criticalErrorLog.Length/2, Screen.height/2 - 25
				                      , ErrorLogManager.criticalErrorLog.Length, 20), ErrorLogManager.criticalErrorLog, newGUIStyle);
			}

			if (ErrorLogManager.normalErrorLog != "" || ErrorLogManager.warningLog != "") {
				GUI.Box(new Rect(0, Screen.height - 30, Screen.width, 30), "", boxStyle);
				GUIStyle newGUIStyle = new GUIStyle();
				newGUIStyle.alignment = TextAnchor.MiddleLeft;
				newGUIStyle.normal.textColor = Color.red;
				newGUIStyle.fontSize = 15;
				GUI.TextArea(new Rect(xTextPostion, Screen.height - 25
				                      , ErrorLogManager.normalErrorLog.Length, 15), ErrorLogManager.normalErrorLog, newGUIStyle);
				newGUIStyle.normal.textColor = Color.yellow;
				GUI.TextArea(new Rect(xTextPostion + newGUIStyle.CalcSize(new GUIContent(ErrorLogManager.warningLog)).x + 3, Screen.height - 25
				                      , ErrorLogManager.warningLog.Length, 15), ErrorLogManager.warningLog, newGUIStyle);
			}

			if(checkPressScenario)
			{
				GUI.skin = warningBox;
				GUI.skin.box.alignment = TextAnchor.UpperCenter;
				GUI.skin.box.normal.textColor = Color.white;
				GUI.Box(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 100),"The current scenario does not \r\n apply for the current model \r\n Please choose other scenario.");
				bool pressedButton = false;
				pressedButton = GUI.Toggle(new Rect(Screen.width/2 - 50, Screen.height/2 + 10, 100, 30),pressedButton, "OK");
				if (pressedButton) {
					checkPressScenario = false;
				}
			}

	    }
		/**
		 * This method is to make the training process one step next.
		 */ 
		private void autoNext()
		{		
			isAutoNext = false;
			if(stepOrder > -1 && stepOrder < lastStepOrder)
				{			  	
					 //1. Disable touch script
	                scenario[stepOrder].ListOfTask[taskOrder].Component.enableTouchScript(false);

	                //2.Set the normal material
	                scenario[stepOrder].ListOfTask[taskOrder].Component.reset();

	                //3. Set the end state
	                scenario[stepOrder].ListOfTask[taskOrder].setEndState();

	                //4. Hide component if component should be hide as scenario shows
					if (scenario[stepOrder].ListOfTask[taskOrder].Component.HideAfterDisassembled)
	                	scenario[stepOrder].ListOfTask[taskOrder].Component.showByEnableRenderer(false);

	                //5. Hide related tool
	             	scenario[stepOrder].ListOfTask[taskOrder].showTool(false);	
				
					//6. disable collider box
					scenario[stepOrder].ListOfTask[taskOrder].Component.enableBoxCollider(false);
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
	            currentComponent = scenario[stepOrder].ListOfTask[taskOrder].Component;

	            //1. Display information
				string taskImagePath = scenario[stepOrder].ListOfTask[taskOrder].ImagePath;
				if( taskImagePath.Equals(""))
	            	imageFileName = scenario[stepOrder].ImagePath;
				else imageFileName = taskImagePath;
	            displayInfo(imageFileName);
				
				// Set texture for Show Info Box on Right
				ShowButtonInfo showInfoScript = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;

				
				int widthOfInfoBox = showInfoScript.WidthOfInfoBox;
				int width =  myGUItexture.width;
				float scale = (float)widthOfInfoBox / (float)width;
				if(Mathf.Abs(1.0f - scale) >= 0.15f) 
				{
					int newHeight =(int)((float)myGUItexture.height *  scale);
					TextureScale.Bilinear(myGUItexture, widthOfInfoBox, newHeight);
				}

				showInfoScript.setGUITextTure(null);
				showInfoScript.setGUITextTure(myGUItexture);
				myGUItexture = emptyTex;
				Resources.UnloadUnusedAssets ();
				Resources.UnloadAsset (myGUItexture);
				Destroy (myGUItexture);
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
				currentComponent.setCurrentTask(currentTask);

				//9. Enable collider box
				currentComponent.enableBoxCollider(true);

			#if UNITY_STANDALONE
				//9Bis. disable video
				scriptVideo.setStop();
				scriptVideo.enabled = false;
				scriptVideo.releaseVideo();
			#endif
				//10. Do some extra requitment
				doExtraRequirement();
			
				//11 Show button video if video available
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
		}
		/**
		 * This method is to make the training process one step back.
		 */ 
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
					if(Application.loadedLevelName == "ReverseTrainingMode")
					{
						scenario[stepOrder].ListOfTask[taskOrder].Component.showByEnableRenderer(false);
					}
	                else scenario[stepOrder].ListOfTask[taskOrder].Component.showByEnableRenderer(true);

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
						if(stepOrder == -1) {
							stepOrder = 0; taskOrder = 0;
							if (ScenarioController.resetFlag && !ScenarioController.finishResetEngine) {
								ScenarioController.finishResetEngine = true;
								ScenarioController.resetFlag = false;
								finishResetEngineFlag = true;
							}	
						}
						else taskOrder = scenario[stepOrder].ListOfTask.Length - 1;;
		
		            }
				}
									
	            // find current EngineComponent
		            currentComponent = scenario[stepOrder].ListOfTask[taskOrder].Component;
		
		            //1. Display information
			
				string taskImagePath = scenario[stepOrder].ListOfTask[taskOrder].ImagePath;
				if( taskImagePath.Equals(""))
	            	imageFileName = scenario[stepOrder].ImagePath;
				else imageFileName = taskImagePath;
	            displayInfo(imageFileName);
				ShowButtonInfo showInfoScript = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
				int widthOfInfoBox = showInfoScript.WidthOfInfoBox;
				int width = myGUItexture.width;
				float scale = (float)widthOfInfoBox / (float)width;
				if(Mathf.Abs(1.0f - scale) >= 0.15f) 
				{
					int newHeight =(int)((float)myGUItexture.height *  scale);
					TextureScale.Bilinear(myGUItexture, widthOfInfoBox, newHeight);
				}

				showInfoScript.setGUITextTure(null);
				showInfoScript.setGUITextTure(myGUItexture);
				myGUItexture = emptyTex;
				Resources.UnloadUnusedAssets ();
				Resources.UnloadAsset (myGUItexture);
				Destroy (myGUItexture);
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
					currentComponent.setCurrentTask(currentTask);
			
			 		//9. Enable collider box
		            currentComponent.enableBoxCollider(true);

					#if UNITY_STANDALONE
					//9Bis. disable video	
					scriptVideo.setStop();
					scriptVideo.enabled = false;
					scriptVideo.releaseVideo();
					#endif
					//10. Do some extra requitment
					doExtraRequirement();
			
					//11. Show button video if video available

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

			if (finishResetEngineFlag) {
				StartCoroutine(LoadXmlFile());
				finishResetEngineFlag = false;
			}
		}

		/**
		 * This method is to check whether user choose the right tools, if so, the tool model 
		 * representing for tools selected by user will be shown
		 */ 
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
			int toolNotSelectedCount = 0;
			foreach (GameObject tool in tools)
			{
				ToolbarControl s;
		    	s = tool.GetComponent("ToolbarControl") as ToolbarControl;
				
				if(s != null && s.used == true) 
				{
					//print("Tem cua script: "+ s.name);
					hasSelectedTool = true;
					result[i++] = s.name.Substring(12, s.name.Length - 12);
				}
				else
				{
					toolNotSelectedCount += 1;
				}
			}
			if(toolNotSelectedCount == tools.Length) hasSelectedTool = false;
			

			string toolsNeeded = "";
			
			for (int j = 0; j < currentTask.ListOfTools.Length; j++) 
			{
				toolsNeeded+=currentTask.ListOfTools[j].Id;
			
			}
			bool rightToolsChosen = true;
			int toolsChosenLength = 0;
			for (int k = 0; k < i; k++) {
				if(!toolsNeeded.Contains(result[k])) rightToolsChosen = false;
				toolsChosenLength += result[k].Length;
			}
			if(rightToolsChosen && toolsChosenLength == toolsNeeded.Length)
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
				// set tool hidden
				currentTask.showTool(false);
				currentTask.Component.setRedSilhouette();
				hasAnyToolsChanged = false;			
				wrongTools = true;
			}
				
			
		}
	}
}
