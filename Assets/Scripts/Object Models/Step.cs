using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETraining
{
	/**
	 * The Step class models the Step concept in E-Training.
	 */
	public class Step
	{
		private string imagePath;/**< The path to the information image.*/
	    private string videoPath; /**< The path to the video.*/
		private int id; /**< The id of step.*/
		private string name; /**< The name of step.*/
		private string linkedInfo; /**< The path to linked information content (swf file).*/
		private int length; /**< The number of tasks that step holds.*/
		private AbstractCommonTask [] listOfTask; /**< List of tasks step holds.*/
		private int numberOfTask; /**< The number of ták*/

		/**
		 * Get and set linkedInfo
		 */
		public string LinkedInfo {
			get {
				return this.linkedInfo;
			}
			set {
				linkedInfo = value;
			}
		}

		/**
		 * Get and set listOfTask
		 */
		public AbstractCommonTask[] ListOfTask {
			get {
				return this.listOfTask;
			}
			set {
				listOfTask = value;
			}
		}

		/**
		 * Constructor function
		 * @param name : name of step
		 * @param id : id of step
		 * 
		 */
	    public Step(string name, int id)
	    {
	        this.name = name;
	        this.id = id;
	       	this.length = 0;
	        //this.numberOfTask = length;
			videoPath = "";
			linkedInfo= "";
	    }

		/**
		 * Add another task to listOfTask
		 */
	    public void addTask(AbstractCommonTask task)
	    {
	        listOfTask[length++] = task;
	    }
		
		/**
		 * Get and set videoPath
		 */
	    public string VideoPath
	    {
	        get { return videoPath; }
	        set { videoPath = value; }
	    }

		/**
		 * Get and set name
		 */
		public string Name {
			get {
				return this.name;
			}
			set {
				name = value;
			}
		}

		/**
		 * Get and set imagePath
		 */
	    public string ImagePath
	    {
	        get { return imagePath; }
	        set { imagePath = value; }
	    }

		/**
		 * Set numberOfTask
		 */
		public void setNumberOfTask(int value)
		{
			this.numberOfTask = value;
			listOfTask = new AbstractCommonTask[numberOfTask];
		}
	}
}