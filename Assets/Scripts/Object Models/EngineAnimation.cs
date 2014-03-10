using UnityEngine;
using System.Collections;
namespace ETraining
{
	/**
	 * This class represents Animation controller for each \ref SingleAnimationTask
	 * It manages the animation relative to the task, and split 1 task animation into 3 states: start, run, end.
	 * Start and end take very short time to be seen as static states, whereas run consumes more time to present whole meaningful animation
	 * Animator is from Unity, it organizes animation visually into 3 clips (corresponding to run clip, start clip, end clip).
	 */ 
	public class EngineAnimation  {
		
		private string animatorName; /**< The name of Animator used in each task. This name is taken from scenario xml file.*/	
		private Animator animator;	/**< The real animator in project that is taken by its name from \animatorName.*/
		private int runOrder; /**< Integer value prepresents the running state, in most cases, it is 1. */
		private int startOrder; /**< Integer value prepresents the start state, in most cases, it is 0. */
		private int endOrder; /**< Integer value prepresents the end state, in most cases, it is 2. */
	    private string aniId; /**< Id of animation. */
		private bool isEndedAnimation = false; /**< The value shows this animation is over (ended) or not.*/

		/**
		 * Get isEndedAnimation
		 */ 
		public bool getEndedAniStatus()
		{
			return isEndedAnimation;
		}

		/**
		 * Get animatorName
		 */ 
		public string AnimatorName {
			get {
				return this.animatorName;
			}		
		}

		/**
		 * Constructor
		 * @param animatorName : Name of animator, it is used to get Animator which defines states transform by using an integer control value.
		 * @param id: Id of animation.
		 */ 
	    public EngineAnimation(string name, string id)
	    {
	        animatorName = name;
	        aniId = id;
	    }

		/**
		 * Constructor
		 * @param animatorName : Name of animator, it is used to get Animator which defines states transform by using an integer control value.
		 */
	    public EngineAnimation(string name)
	    {
	        animatorName = name;       
	    }

		/**
		 * Constructor
		 * @param anim : Name of animator, it is used to get Animator which defines states transform by using an integer control value.
		 * @param run : The value represents running state, it normally is 1
		 * @param start : The value represents start (beginning state), it normally is 0
		 * @param end : The value represents end state, it normally is 2
		 */
		public EngineAnimation(string anim, int run, int start, int end)
		{
			runOrder = run;
			startOrder = start;
			endOrder = end;
			animatorName = anim;

			// try to get Animator from animatorName
			try {

				if(Application.loadedLevelName == "TransmissionMode")
				{
					// to understand, go to scene Transmissin New, hierarchy tab, -> Transmission 
					// all childs (tr001 -> tr0xx) has an Animator, and tagged engine-editor (see in inspector tab).
					// This code is to get Animator from GameObject that matchs the animatorName.
					// why can't just call like in else below, it's because there are 2 tr001 (1 child, 1 parent) in transmission,
					// but Animator just stays in parent inspector.
					GameObject []list = GameObject.FindGameObjectsWithTag("engine-editor");
					foreach (GameObject game in list) {
						if(game.name ==  animatorName)
						{
							animator = game.GetComponent(typeof(Animator)) as Animator; 	
							break;
						}
					}
				}
				else // for other modes, just simple find the animatorName
				{
					animator = GameObject.Find(animatorName).GetComponent(typeof(Animator)) as Animator; 	
				}

			} catch (System.Exception ex) {
				Debug.Log(ex.Message + "[" + animatorName + "]");
				throw;
			}
			
				
				
			isEndedAnimation = false;
		}

		/**
		 * Set state Run
		 */ 
		public void run()
		{		
			animator.ForceStateNormalizedTime(0.0f);		
			animator.SetInteger("Order", runOrder);		
			isEndedAnimation = false;
		}

		/**
		 * Set state Start
		 */
		public void start()
		{
				animator.ForceStateNormalizedTime(0.0f);		
				animator.SetInteger("Order", startOrder);									
			isEndedAnimation = false;
		}

		/**
		 * Set state End
		 */
		public void end()
		{
				animator.ForceStateNormalizedTime(0.0f);		
				animator.SetInteger("Order", endOrder);	
				isEndedAnimation = true;
			
		}
		
		// Use this for initialization
		void Start () {
		
		}	
	}
}