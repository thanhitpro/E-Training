using UnityEngine;
using System.Collections;
using ETraining;
using ETraining.UI.StepControls;

namespace ETraining.UI.ToolControls
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control TOOL gui in the center bottom of Screen
	 */
	public class ToolbarControl : GUIBase
	{

		public bool firstRun = true;
		/**
		 * It needs ShowButtonInfo position, so it should use ShowButtonInfo as a private field
		 */
		private ShowButtonInfo scriptShowInfo;
		

		// Use this for initialization
		void Awake ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 110;
			ySize = 110;

			// scale width and height of GUI to ratio
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);


			int border = 0; // all in pixel
			int sizeOfAllGui = 9 * xSize; // 9 is 9 tools we have
			
			int h = Screen.height;				
			int w = Screen.width;

			// Do some computation to find top-left (x,y)
			int firstPost = (w/2) + (sizeOfAllGui/2);
			int deltaPos = w - firstPost;
			y = h - (int)(ySize + border);
			x = w - (int)(order * (xSize + border)+ deltaPos) ;

			// Find the ShowInfoButton script
			scriptShowInfo = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
		}
		
		void OnGUI () 
		{
			GUI.depth = 1000;
			// Only run GUI when splash screen is done for the first time.
			if(SplashScreenControl.isFirstTime) return;


			// Assign the skin to be the one currently used.
			GUI.skin = mySkin;
				

			// for the first run: we show the button
			if(firstRun)
			{
				
				if (GUI.Button (new Rect (x - (scriptShowInfo.getZeroToBoxInfoWidth()/2),y, xSize, ySize),""))
				{
					firstRun = false;	
					used = true;	
				}
			}
			else
			{
				
				used = GUI.Toggle (new Rect (x - (scriptShowInfo.getZeroToBoxInfoWidth()/2),y,xSize, ySize), used,"");
				if(!used)
				{						
					
					GUI.Button (new Rect (x - (scriptShowInfo.getZeroToBoxInfoWidth()/2),y, xSize, ySize),"");			
				}
			}			
		}
		
		// Update is called once per frame
		void Update ()
		{
			if(used != checkUsed) // if tool button is changed ( might be touched, or released)
			{
				// Get ScenarioController script to check we have gone into learning (next, prev) yet ?
				// If Current learning step is not existing -> just do nothing by return;
				ScenarioController trainingScript = Camera.main.GetComponent(typeof(ScenarioController)) as ScenarioController;		
				if(trainingScript.getCurrentStep() == null) return;

				// Tell ScenarioController script that at least one tool is changed
				// So please do scanning all tools to decide user select right tools or not
				trainingScript.hasAnyToolsChanged = true;								

				// Find the invisible cube containing script which controls 1 touch rotation.
				GameObject cube = GameObject.Find("Cube");

				// Find all ToolbarControl script and check if it
				var tools = GameObject.FindGameObjectsWithTag("tools");
				foreach (GameObject tool in tools)
				{
					ToolbarControl script = tool.GetComponent("ToolbarControl") as ToolbarControl;
					if( script.used == true) 
					{
						// put touch Cube to camera position so it cannot get touch event
						cube.transform.position = Camera.main.transform.position;
						checkUsed = used;	

						// we only need the first one tool is selected, so dont have to loop more and we go out of Update()
						return;
					}
				}

				// After checking all tools, but no tool is selected, we bring touch cube a little front of camera, so it can get touch event for rotation around engine
				Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find("engine_LOW").transform.position;
				cube.transform.position = position;

				// keep track of used by checkUsed
				checkUsed = used;
			}	
		}

		public override void loadMode ()
		{
			//do nothing
			return;
		}

	}
} 
