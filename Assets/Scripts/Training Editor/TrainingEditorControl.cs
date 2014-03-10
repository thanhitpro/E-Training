using UnityEngine;
using System.Collections;
/**
 * This class is designed for further enhancement of scenario editor
 * Now, it is kept to remind some ideas. It might be removed.
 */ 
public class TrainingEditorControl : ExpandColapseEngine {

	private string listComponentWithHand = "en0004 en0006 en0015_en0015 en0027 en0008_01 1 en0160 EN_PCV_en0012_02 1 EN_PCV_pipe_01 en0012_01";
	private string listComponentWithSpecificAniOrder = "";


	#region implemented abstract members of ExpandColapseEngine

	protected override bool doNotNeedTools (string componentName)
	{
		return listComponentWithHand.Contains(componentName.Trim());
	}


	protected override string chooseAniOrder (string componentName)
	{

		if(componentName.Equals("EN_PCV_pipe_01"))
		{
			return "3,2,4";
		}
		else if(componentName.Equals("en0160_1"))
		{
			return "5,4,6";
		}
		else if(componentName.Equals("en0160"))		
		{
			return "5,4,6";
		}
		else if(componentName.Equals("EN_PCV_en0012_02 1"))		
		{
			return "9,8,10";
		}
		else if(componentName.Equals("en0142"))
		{
			return "3,2,4";
		}
		else if(componentName.Equals("en0004"))
		{
			return "5,4,6";
		}
		else if(componentName.Equals("en00104"))
		{
			return "3,2,4";
		}
		else if(componentName.Equals("en0006"))		
		{
			return "7,6,8";
		}
		else if(componentName.Equals("en0014_04"))		
		{
			return "5,4,6";
		}
		else if(componentName.Equals("en0014_05"))
		{
			return "7,6,8";
		}
		else if(componentName.Equals("en0014_01"))
		{
			return "9,8,10";
		}
		else{ return  "";}
	}


	#endregion
}
