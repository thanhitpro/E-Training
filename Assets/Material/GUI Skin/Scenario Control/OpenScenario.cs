using UnityEngine;
using System.IO;
using System.Collections;

namespace ETraining.UI.Scenario
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Open Scenario Button
	 */
	public class OpenScenario : GUIBase
	{
		GUIContent[] comboBoxList;
		private ComboBox comboBoxControl;// = new ComboBox();
		private GUIStyle listStyle = new GUIStyle();
		private bool showCombobox = false;
		private float xBoxPosition;
		private float yBoxPosition;
		private float xBoxWidth;
		FileInfo[] info;
		public bool getUsedStatus(){ return used;}
		public void setUsedStatus(bool value) {
			used = value;
		}
		private bool firstUsed = false;

		IEnumerator slideOut() 
		{ 
			if (xBoxPosition < -5) {
				xBoxPosition += 5;
				x = (int)(xBoxPosition+xBoxWidth);
			} else {
				xBoxPosition = 0;
			}
			yield return StartCoroutine(Wait(0.1F));  
		}

		IEnumerator slideIn() 
		{	 
			if (xBoxPosition > -(xBoxWidth - 5)) {
				xBoxPosition -= 5;
				if (firstUsed)
					x = x - 5;
			} else {
				xBoxPosition = -xBoxWidth;
			}
			yield return StartCoroutine(Wait(0.1F)); 
		}

		IEnumerator Wait(float waitTime) {
			yield return new WaitForSeconds(waitTime);       
		}


		
		
		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 56;
			ySize = 62;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			isFollowedShowInfoButton = false;

			// Define top left corner GUI Control should be located.
			y = 0;
			x = 0 ;

			DirectoryInfo dir;

			#if UNITY_STANDALONE_WIN
				dir = new DirectoryInfo(Application.dataPath + "/StreamingAssets/Scenario");
				info = dir.GetFiles("*.xml");
			#endif
			#if UNITY_ANDROID	
				dir = new DirectoryInfo(Application.persistentDataPath + "/Scenario");
				info = dir.GetFiles("*.xml");
				
			#endif	
			#if UNITY_IPHONE
				//dir = new DirectoryInfo(Application.dataPath + "/StreamingAssets/Scenario");
				dir = new DirectoryInfo(Application.persistentDataPath);
				info = dir.GetFiles("*.xml");
			#endif	

			if (info.Length == 0) { 
				return;
			}
			comboBoxList = new GUIContent[info.Length];
			int i = 0;
			foreach (FileInfo f in info) 
			{
				comboBoxList[i] = new GUIContent("  " + f.Name + "    ");
				i++;
			}
			
			listStyle.normal.textColor = Color.white; 
			listStyle.normal.background = new Texture2D(1, 1);
			listStyle.normal.background.SetPixel(0, 0, Color.black);
			listStyle.normal.background.Apply();
			listStyle.fontSize = 40;
			listStyle.onHover.background =
				listStyle.hover.background = new Texture2D(2, 2);
			listStyle.padding.left =
				listStyle.padding.right =
					listStyle.padding.top =
					listStyle.padding.bottom = 0;
			
			comboBoxControl = new ComboBox(this, new Rect(0, 0, 200, 40), comboBoxList[0], comboBoxList, "button", "box", listStyle);
			xBoxWidth = comboBoxControl.findMax();
			xBoxPosition = -xBoxWidth;
			comboBoxControl.updateRect(new Rect(xBoxPosition, 0, 200, 40));
		}
		
		#region implemented abstract members of MenuModeGUIBase

		void OnGUI () 
		{
			if (info.Length == 0) {
				return;
			}
			base.OnGUI();
			comboBoxControl.updateRect(new Rect(xBoxPosition, 0, 100, 100));
			comboBoxControl.Show();
			if (used) {
				StartCoroutine(slideOut());
				firstUsed = true;
			} else {
				StartCoroutine(slideIn());
			}
		}

		public override void loadMode ()
		{
			showCombobox = !showCombobox;
		}

		#endregion
	}


	class ComboBox
	{
		private int selectedItemIndex = 0;
		
		private Rect rect;
		private GUIContent buttonContent;
		private GUIContent[] listContent;
		private string buttonStyle;
		private string boxStyle;
		private GUIStyle listStyleCombobox;
		public static float maxValue;
		private OpenScenario openScenario;
		
		public ComboBox(OpenScenario scenario, Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle ){
			openScenario = scenario;
			this.rect = rect;
			this.buttonContent = buttonContent;
			this.listContent = listContent;
			this.buttonStyle = "button";
			this.boxStyle = "box";
			this.listStyleCombobox = listStyle;
		}
		
		public ComboBox(OpenScenario scenario, Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle){
			openScenario = scenario;
			this.rect = rect;
			this.buttonContent = buttonContent;
			this.listContent = listContent;
			this.buttonStyle = buttonStyle;
			this.boxStyle = boxStyle;
			this.listStyleCombobox = listStyle;
		}

		public void updateRect(Rect newRect) {
			rect.x = newRect.x;
			rect.y = newRect.y;
		}
		
		public void Show()
		{
			float max = maxValue;
			Rect listRect = new Rect( rect.x, rect.y,
			                         max, listStyleCombobox.CalcHeight(listContent[0], 1.0f) * listContent.Length );
			GUI.Box( listRect, "", listStyleCombobox );
			int newSelectedItemIndex = GUI.SelectionGrid( listRect, selectedItemIndex, listContent, 1, listStyleCombobox );
			if( newSelectedItemIndex != selectedItemIndex )
			{
				selectedItemIndex = newSelectedItemIndex;
				buttonContent = listContent[selectedItemIndex];
				ScenarioController.scenarioFileName = buttonContent.text.Trim();
				ScenarioController.LoadDataFromXmlFile();
				ScenarioController.showErrorLog = false;
				ErrorLogManager.exportNormalLog();
				openScenario.setUsedStatus(false);
			}
		}
		
		public float findMax() {
			float max = -1;
			for(int i = 0; i < listContent.Length; ++i) {
				if (max < listStyleCombobox.CalcSize(listContent[i]).x) 
					max = listStyleCombobox.CalcSize(listContent[i]).x;
			}
			maxValue = max;
			return max;
		}
		
		
		public int SelectedItemIndex{
			get{
				return selectedItemIndex;
			}
			set{
				selectedItemIndex = value;
			}
		}
	}


}	