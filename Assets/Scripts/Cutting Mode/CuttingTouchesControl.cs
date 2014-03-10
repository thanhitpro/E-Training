/*
 * Copyright (C) 2012 Interactive Lab
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation  * files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,  * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the  * Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the  * Software.
 *  * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE  * WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR  * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR  * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using TouchScript.Events;
using UnityEngine;
using TouchScript;

namespace ETraining.MultiTouch {
	/**
     * This script handles multi touch.
     * First, Unity does not support Window Multitouch.
     * We use another tools called TouchScript (http://interactivelab.github.io/TouchScript/). 
     * This script is modified to handle multiple touch in Cutting mode.
     */
    public class CuttingTouchesControl : MonoBehaviour
    {

		#region E-Training Project fields
		private Vector2 firstPoint; /**< The first finger touch position.*/
		private Vector2 secondPoint; /**< The 2nd finger touch position.*/
		private float currentDistance = 0; /**< The value of distance between 2 fingers touch.*/
		private GameObject touchCube; /**< The cube that contains touch script to make orbitting gesture (1 finger rotation).*/
		public GameObject engine; /**< The engine that is focused.*/
		#endregion


        #region Private variables
        private readonly Dictionary<int, TouchPoint> dummies = new Dictionary<int, TouchPoint>();        
        #endregion

        #region Unity

        private void Start() {
            if (camera == null) throw new Exception("A camera is required.");
            
            if (TouchManager.Instance == null) throw new Exception("A TouchManager is required.");
            TouchManager.Instance.TouchPointsAdded += OnTouchPointsAdded;
            TouchManager.Instance.TouchPointsRemoved += OnTouchPointsRemoved;
            TouchManager.Instance.TouchPointsUpdated += OnTouchPointsUpdated;
            TouchManager.Instance.TouchPointsCancelled += OnTouchPointsCancelled;

			touchCube = GameObject.Find("touchCube");
        }

		/**
		 * We modify the orginal script of TouchScript library.
		 * In Update(), we deal with zoom gesture (2 fingers)
		 * and Cutting gesture (3 fingers move up/down). Please focus on
		 * this function of this script only.
		 */ 
        private void Update() {
            //camera.orthographicSize = Screen.height * .5f;
			bool isUp = false;
			int i = 0;
            foreach (KeyValuePair<int, TouchPoint> dummy in dummies)
            {
				i++;
                var x = dummy.Value.Position.x;
                var y = Screen.height - dummy.Value.Position.y;
				if(dummies.Count==2 && i==1) 
				{
					// 2 finger touch, and save the first touch position
					firstPoint = new Vector2(x ,y );				
				}
				else if (dummies.Count==2 && i==2)
				{						
					// 2 finger touch, and save the 2nd touch position
					secondPoint = new Vector2(x ,y );
				}	
				else if (dummies.Count==3 )
				{	
					// 3 finger touch, it would be cutting
					if (dummy.Value.PreviousPosition.y < dummy.Value.Position.y )
					{						
						isUp = true;
					}					
				}
            }

			if (dummies.Count == 3 )
			{ 
				// Now, we put touchCube into camera pos, so there is no 1-touch-to-orbit available
				touchCube.transform.position = Camera.main.transform.position;

				// Cutting is done by assigning testMaterial, so this material does cutting effect
				// by a parameter called "_Height", which represent the high level of cutting.
				Material myMaterial = (Material) Resources.Load("testMaterial", typeof(Material));
				float current = myMaterial.GetFloat("_Height");

				if(isUp) // 3 fingers are moving up.
				{						
					if (current > 3.5) myMaterial.SetFloat("_Height", 3.5f);
			    	else myMaterial.SetFloat("_Height",current + 0.02f); // increment by 0.02f
				}
				else 
				{						
					if (current < -2.0f)  myMaterial.SetFloat("_Height", -2.0f);
					else myMaterial.SetFloat("_Height",current - 0.02f); // decrement by 0.02f
				}
				
			}
			else if (dummies.Count == 1)
			{
				// bring touchCube front of camera so touchCube can receive touch event (touchCube has collider to be touchable)
				touchCube.transform.position = Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + engine.transform.position;	
			}
			else if (dummies.Count == 0){
				// When not touch, dummies.COunt ==0, this script do nothing,
				// It does not control the touch cube position

			}
			else // count == 2 or greater than 3
			{
				// Now, we put touchCube into camera pos, so there is no 1-touch-to-orbit available
				touchCube.transform.position = Camera.main.transform.position;
			}

			// reset curretDistance and do nothing else, if no 2-finger-touch
			if (dummies.Count != 2) 
			{ 
				currentDistance = 0;				
				return;
			}

			// first, if currentDistance equals to 0 then assign distance of 2 fingers touch to it
			if ( currentDistance == 0)
			{
				currentDistance = Vector2.Distance(firstPoint,secondPoint);				
			}
		
			//Do zoom in
			if(currentDistance < Vector2.Distance(firstPoint,secondPoint) - 3)
			{
				if(Camera.main.fieldOfView < 30.0f)
				{
					Camera.main.fieldOfView -= 0.3f;
				}
				Camera.main.fieldOfView -= 0.8f;
				currentDistance = Vector2.Distance(firstPoint,secondPoint);			
				if (Camera.main.fieldOfView < 5) Camera.main.fieldOfView =5;
				
			}
			//Do Zoom out
			else if(currentDistance > Vector2.Distance(firstPoint,secondPoint) + 3)
			{

				Camera.main.fieldOfView += 0.8f;
				currentDistance = Vector2.Distance(firstPoint,secondPoint);			
				if (Camera.main.fieldOfView > 90) Camera.main.fieldOfView = 90;
			}
						
        }
		        
        private void OnDestroy() {
            TouchManager.Instance.TouchPointsAdded -= OnTouchPointsAdded;
            TouchManager.Instance.TouchPointsRemoved -= OnTouchPointsRemoved;
            TouchManager.Instance.TouchPointsUpdated -= OnTouchPointsUpdated;
            TouchManager.Instance.TouchPointsCancelled -= OnTouchPointsCancelled;
        }

        #endregion

        #region Private functions

        private void updateDummy(TouchPoint dummy)
        {
            dummies[dummy.Id] = dummy;
        }

        #endregion

        #region Event handlers

        private void OnTouchPointsAdded(object sender, TouchEventArgs e) {
            if (!enabled) return;

            foreach (var touchPoint in e.TouchPoints) {
                dummies.Add(touchPoint.Id, touchPoint);
            }
        }

        private void OnTouchPointsUpdated(object sender, TouchEventArgs e) {
            if (!enabled) return;

            foreach (var touchPoint in e.TouchPoints) {
                TouchPoint dummy;
                if (!dummies.TryGetValue(touchPoint.Id, out dummy)) return;
                updateDummy(touchPoint);
            }
        }

        private void OnTouchPointsRemoved(object sender, TouchEventArgs e) {
            if (!enabled) return;

            foreach (var touchPoint in e.TouchPoints) {
                TouchPoint dummy;
                if (!dummies.TryGetValue(touchPoint.Id, out dummy)) return;
                dummies.Remove(touchPoint.Id);
            }
        }

        private void OnTouchPointsCancelled(object sender, TouchEventArgs e) {
            OnTouchPointsRemoved(sender, e);
        }

        #endregion

    }
}