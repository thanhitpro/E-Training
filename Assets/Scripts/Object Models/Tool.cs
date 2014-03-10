
using System.Collections;

namespace ETraining
{
	/**
	 * The Tool class represents tool used in each task
	 */
	public class Tool  {
		
		private string name; /**< Name of tool.*/
		private string id; /**< id of tool.*/

		/**
		 * Constructor
		 */ 
	    public Tool(string id, string name)
	    {
	        this.id = id;
	        this.name = name;
	    }
		
		/**
		 * Get and set id
		 */ 
		public string Id {
			get {
				return this.id;
			}
			set {
				id = value;
			}
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
	}
}