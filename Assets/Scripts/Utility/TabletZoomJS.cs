using UnityEngine;

namespace ETraining.UI
{
    public class TabletZoomJS : MonoBehaviour
    {
		#if UNITY_ANDROID || UNITY_IPHONE
        public float distance = 5.0f;
        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;
        public float yMinLimit = -20;
        public float yMaxLimit = 80;

        private float x = 0.0f;
        private float y = 0.0f;
        private float currentDistance = 0.0f;
        private GameObject cube;
        private GameObject engine;
		private ScenarioController scScript; /**< The script contains information about tools are selected or not.*/
		private bool first1Touch = false;  /**< Represent the 1 touch geture is followed by 2 touch gesture, be cause we want to remove unwanted orbit after 2 finger-release-1. */

        void Start()
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            // Make the rigid body not change rotation
            if (rigidbody)
                rigidbody.freezeRotation = true;

            cube = GameObject.Find("Cube");
            engine = GameObject.Find("engine_LOW");
			scScript = Camera.main.GetComponent(typeof(ScenarioController)) as ScenarioController;
        }

		void LateUpdate() {
			if(Input.touchCount == 2)
			{
				first1Touch = true;
				cube.transform.position = Camera.main.transform.position ;
				//float y = Input.GetTouch(0).position.y;
				//float x2 = Input.GetTouch(1).position.x;
				//float y2 = Input.GetTouch(1).position.y;
				
				Vector2 firstPoint =  Input.GetTouch(0).position;
				Vector2 secondPoint =  Input.GetTouch(1).position;
				//Vector2 secondPoint = new Vector2(x2 , y2 );
				
				if ( currentDistance == 0)
				{
					currentDistance = Vector2.Distance(firstPoint,secondPoint);				
				}
				//else 
				Debug.LogError("value first second " + firstPoint.ToString() + " " + secondPoint.ToString());
				//{
				if(currentDistance < Vector2.Distance(firstPoint,secondPoint))
				{
					//Debug.LogError("is zoom in");
					if(Camera.main.fieldOfView < 30.0f)
					{
						Camera.main.fieldOfView -= 1.5f;
					}
					Camera.main.fieldOfView -= 2.0f;
					currentDistance = Vector2.Distance(firstPoint,secondPoint);			
					if (Camera.main.fieldOfView < 5) Camera.main.fieldOfView = 5;
					
				}
				else if(currentDistance > Vector2.Distance(firstPoint,secondPoint))
				{
					//Debug.LogError("is zoom out");
					
					Camera.main.fieldOfView += 2.0f;
					currentDistance = Vector2.Distance(firstPoint,secondPoint);			
					if (Camera.main.fieldOfView > 90) Camera.main.fieldOfView = 90;
				}
			}
			else if (Input.touchCount == 1)
			{
				if(first1Touch)
				{
					first1Touch = false;
					return;
				}

				if(scScript.hasSelectedTool) 
				{
					cube.transform.position = Camera.main.transform.position ;
					return;
				}

				Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + engine.transform.position;
				cube.transform.position = position;

			}
		}

		static double ClampAngle (float angle, float min, float max) {
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
			return Mathf.Clamp (angle, min, max);
		}
		#endif
    }

}
