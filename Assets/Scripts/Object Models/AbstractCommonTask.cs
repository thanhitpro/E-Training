
using System.Collections;
using UnityEngine;

namespace ETraining
{
	/**
	 * The Base class or Task
	 */
	public class AbstractCommonTask : MonoBehaviour{
		
		private EngineComponent component; /**< Each task holds an EngineComponent.*/
		private string toolModelName; /**< The name of tool model.*/
		private string imagePath; /**< The path to the information image.*/
		private string taskName; /**< The name of task.*/
		private string groupId; /**< The id of group this task belongs to.*/
		private Tool[] listOfTools; /**< The list of tools used in this task.*/
		private int id; /**< The id of task.*/
		private string description; /**< The description of task.*/
		private string animatorName=""; /**< The animator name.*/

		/**
		 * Get animatorName
		 */
		public virtual string getAnimatorName(){return animatorName;}

		/**
		 * Get and set listOfTools
		 */
		public Tool[] ListOfTools // this content the list of tools should be chosen
		{
			get { return listOfTools; }
			set { listOfTools = value; }
		}

		/**
		 * Get and set imagePath
		 */
		public string ImagePath {
			get {
				return this.imagePath;
			}
			set {
				imagePath = value;
			}
		}	

		/**
		 * Get and set toolModelName
		 */
		public string ToolModelName {
			get {
				return this.toolModelName;
			}
			set {
				toolModelName = value;
			}
		}

		/**
		 * Get and set component
		 */
		public EngineComponent Component
		{
			get { return component; }
			set { component = value; }
		} 

		/**
		 * Get and set description
		 */
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
		
		/**
		 * Empty constructor
		 */
		public AbstractCommonTask() { }

		/**
		 * Constructor function
		 * @param id : id of step
		 * @param taskName : name of step
		 * @param groupId : group of task
		 */
		public AbstractCommonTask(int id, string taskName, string groupId)
		{
			this.id = id;
			this.taskName = taskName;
			this.groupId = groupId;
		}

		/**
		 * Constructor function
		 * @param length : the number of tools
		 */
		public AbstractCommonTask(int length)
		{
			listOfTools = new Tool[length];
			imagePath = "";
		}

		/**
		 * Get and set id
		 */
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

	
		/**
		 * Get and set taskName
		 */
		public string TaskName
		{
			get { return taskName; }
			set { taskName = value; }
		}

		/**
		 * Get and set groupId
		 */
		public string GroupId
		{
			get { return  groupId; }
			set { groupId = value; }
		}

		/**
		 * Whenever the task is the current task, this function will decide whether tool model should show
		 * @param show : bool value indicates tool model related to this task should be shown or hidden
		 */
		public void showTool(bool show)
		{
			// if this task has tool model
			if(toolModelName != "")
			{
				// 1 task can have tool model, in some cases, that tool model has many 3d meshs (because of modeling from other university)
				// So we "|" to separate tool model parts. Ex: A plier model has 2 parts, so we use the value "EN_PCV_sorkets_01|EN_PCV_sorkets_02"

				// if toolModelName does not have character "|"
				if(!toolModelName.Contains("|"))
				{
					GameObject go = GameObject.Find(getAnimatorName()) as GameObject;
					Transform toolTf = getToolTransform(toolModelName, go.transform);
					toolTf.renderer.enabled = show;				
				}
				else
				{
					string []modelPartName = toolModelName.Split('|');

					// Because multiple 3D meshes of tool, we have to loop and show them all
					foreach (string item in modelPartName) {
						GameObject go = GameObject.Find(getAnimatorName()) as GameObject;
						Transform toolTf = getToolTransform(item, go.transform);					
						toolTf.renderer.enabled = show;		
					}
				}
			}
		}

		/**
		 * Recursively get right transform for tool using name of it.
		 * Ex: From the Hierarchy tab in unity3d e-training source code, we go to Engine and 
		 * find step10-task1, from step10-task1 we go sorkets04, and continue go to sorkets01 1.
		 * Now, we can return this sorket.
		 * 
		 * @param name : name of transform (in this case is tool name)
		 * @param tf : tf we found
		 */
		private Transform getToolTransform(string name, Transform tf)
		{
			if(tf.name == name) return tf;	
			int childNum = tf.GetChildCount();		
			for (int i = 0; i < childNum; i++) 
			{
				if(getToolTransform(name, tf.GetChild(i))== null) continue;		
				else return getToolTransform(name, tf.GetChild(i));
			}
			return null;
		}


		/**
		 * Set of virtual functions that will be implemented in child classes.
		 */ 
		public virtual void runAnimation(){}
		public virtual void setStartState(){}
		public virtual void setEndState(){}

	}

}
