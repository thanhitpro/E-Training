using UnityEngine;
using System;
using System.Collections;

namespace ETraining
{
	/**
	 * This class will be deprecated soon
	 */
	[Obsolete("This class is deprecated and will be removed.")]
	public class MultipleAnimationTask : AbstractCommonTask {

	    private EngineAnimation[] listOfAnimation;
		private bool isDone = false;
		
		IEnumerator WaitSomeSeconds() 
		{
	        print("Starting " + Time.time);
	        yield return StartCoroutine(WaitAndPrint(2.0F));
	        print("Done " + Time.time);
			isDone = true;
	    }
		
			
		
	    IEnumerator WaitAndPrint(float waitTime) {
	        yield return new WaitForSeconds(waitTime);
	        print("WaitAndPrint " + Time.time);
			isDone = true;
	    }
		
		void OnGUI()
		{ 
			if(true)
			{
				if(listOfAnimation!=null && listOfAnimation[0] != null)			
				{
				
					//listOfAnimation[1].run();
				}
				isDone = false;
			}
			
			
		}
	    public EngineAnimation[] ListOfAnimation
	    {
	        get { return listOfAnimation; }
	        set { listOfAnimation = value; }
	    }
		private int length;
		private int index;

	    public MultipleAnimationTask(EngineAnimation[] list)
	    {
	        ListOfAnimation = list;
	    }
		
		public void addAnimation(EngineAnimation []aniList)
		{
			for(int i = 0; i < aniList.Length; i ++)
			{
				listOfAnimation[i] = aniList[i];
			}
		}

		public override void runAnimation()
		{
				listOfAnimation[0].run();
		}
	}
}