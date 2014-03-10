using UnityEngine;
using System.Collections;
namespace ETraining.UI.StepControls
{
	/**
	 * The child class of \ref GUIBase.
	 * This class inherits GUIBase, and is used to control Prev Button
	 * which move learning process to previous component of engine or transmission
	 */
	public class PrevButton: GUIBase
	{
		public bool Used {
			get {
				return this.used;
			}
			set {
				used = value;
			}
		}

		void Start ()
		{
			// The size is hard-assigned using size of GUI Image
			xSize = 62;		
			ySize = 72;

			// Scale to ratio of Device.width and user-selected standardWidth
			xSize = Mathf.CeilToInt(((float)Screen.width / (float)standardWidth) * (float)xSize);
			// Scale to ratio of Device.height and user-selected standardHeight
			ySize = Mathf.CeilToInt(((float)Screen.height / (float)standardHeight) * (float)ySize);

			// Define top left (y coordinate only) corner GUI Control should be located.
			y =   findYPosFor(70)
				+ findYPosFor(74)
				+ findYPosFor(74)
				+ findYPosFor(83)
				+ findYPosFor(101)
				+ findYPosFor(67);
		
		}

		#region implemented abstract members of MenuModeGUIBase

		public override void loadMode ()
		{
			// Simply do nothing
			return;
		}
		#endregion
	}

}