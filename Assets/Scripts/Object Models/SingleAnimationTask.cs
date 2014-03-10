
using System.Collections;
namespace ETraining
{
	/**
	 * Child class of \ref AbstractCommonTask
	 * This class represent the task which has only one animation.
	 * At first, I (Anh) intended to make another \ref MultipleAnimationTask for further development.
	 * Then, i changed development strategy, made a restriction that all task should have only 1 animation.
	 * So the \ref MultipleAnimationTask becomes useless.
	 */
	public class SingleAnimationTask : AbstractCommonTask {

		private EngineAnimation taskAnimation; /**< Task animation*/
		private bool isEnded = false; /**< Status of animation*/

		/**
		 * Get and set isEnded
		 */ 
		public bool IsEnded {
			get {
				return this.isEnded;
			}
			set {
				isEnded = value;
			}
		}

		/**
		 * Empty Constructor
		 */
	    public SingleAnimationTask() { }

		/**
		 * Get task animation
		 */
		public EngineAnimation getAnimation()
		{		
			return taskAnimation;
		}

		/**
		 * Constructor do task animation assignment
		 */ 
		public SingleAnimationTask(EngineAnimation ani)
		{
	    
			taskAnimation = ani;
		}

		/**
		 * Implement the method runAnimation from base class
		 * set animation into run state and set status isEnded to true
		 */ 
		public override void runAnimation()
		{
			taskAnimation.run();
			isEnded = true;
		}

		/**
		 * Implement the method runAnimation from base class.
		 * Set animation into the beginning state and set status isEnded to false
		 */ 
		public override void setStartState()
		{
			taskAnimation.start();
			isEnded = false;
		}

		/**
		 * Implement the method runAnimation from base class.
		 * Set animation into the end state and set status isEnded to true
		 */ 
		public override void setEndState()
		{
			taskAnimation.end();
			isEnded = true;
		}

		/**
		 * Get animator name of the task animation
		 */ 
		public override string getAnimatorName()
		{
			return taskAnimation.AnimatorName;
		}
	}
}