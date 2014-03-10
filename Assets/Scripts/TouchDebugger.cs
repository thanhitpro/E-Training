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

namespace TouchScript.Debugging {
    /// <summary>
    /// Visual debugger to show touches as GUI elements.
    /// </summary>
    public class TouchDebugger : MonoBehaviour
    {

        #region Unity fields
		private Vector2 firstPoint;
		private Vector2 secondPoint;
		private float currentDistance = 0;
		private float change = 0;
		private bool isRight ;
		private Vector2 [] _5fingers;
		private int [] _5distances;
		private Vector2 centerPoint;
		private bool hasCenterPoint;
        /// <summary>
        /// Texture to use
        /// </summary>
        public Texture2D TouchTexture;
        /// <summary>
        /// Font color for touch ids
        /// </summary>
        public Color FontColor;

        #endregion

        #region Private variables

        private readonly Dictionary<int, TouchPoint> dummies = new Dictionary<int, TouchPoint>();
        private GUIStyle style;

        #endregion

        #region Unity

        private void Start() {
            if (camera == null) throw new Exception("A camera is required.");
            
            if (TouchManager.Instance == null) throw new Exception("A TouchManager is required.");
            TouchManager.Instance.TouchPointsAdded += OnTouchPointsAdded;
            TouchManager.Instance.TouchPointsRemoved += OnTouchPointsRemoved;
            TouchManager.Instance.TouchPointsUpdated += OnTouchPointsUpdated;
            TouchManager.Instance.TouchPointsCancelled += OnTouchPointsCancelled;
			
			_5fingers = new Vector2[4];
			hasCenterPoint = false;
			
			_5distances = new int[4];
        }

        private void Update() {
            camera.orthographicSize = Screen.height * .5f;
        }

        private void OnGUI() {
           // if (TouchTexture == null) return;
			bool isRight = false;

            GUI.color = FontColor;
			//Debug.LogError(dummies.Count);
			int i = 0;
            foreach (KeyValuePair<int, TouchPoint> dummy in dummies)
            {
				//Debug.LogError("dummie value " + dummy.ToString() + "\n");
				//Debug.LogError("Dumi count " + dummies.Count);
				i++;
				//Debug.LogError("1");
                var x = dummy.Value.Position.x;
                var y = Screen.height - dummy.Value.Position.y;
				//Debug.LogError("2");
               // GUI.DrawTexture(new Rect(x/* - TouchTexture.width / 2*/, y /*- TouchTexture.height / 2*/, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
               // GUI.Label(new Rect(x + TouchTexture.width, y - 9, 60, 25), dummy.Value.Id.ToString());
				//Debug.LogError("3");
				if(dummies.Count==3 && i==1)
				{
					hasCenterPoint = false;
					GameObject cube;
					cube = GameObject.Find("Cube");
					//print (cube.name);
					cube.transform.position = Camera.main.transform.position;
					
				//	Debug.LogError("4");
					//GameObject.Find("Cube").transform.position = Camera.main.transform.position;
				//	Debug.LogError("41");
					firstPoint = new Vector2(x ,y );
				//	Debug.LogError("42");
				//Debug.LogError("going to first point");
				}
				else if (dummies.Count==3 && i==2)
				{
					hasCenterPoint = false;
					GameObject cube;
					cube = GameObject.Find("Cube");
					//print (cube.name);
					cube.transform.position = Camera.main.transform.position;
					
					//Debug.LogError("5");
					secondPoint = new Vector2(x ,y );
					//Debug.LogError("going to 2nd point");
				}
//				else if(dummies.Count == 5)// && i == 5)
//				{
//					GameObject cube;
//					cube = GameObject.Find("Cube");
//					//print (cube.name);
//					cube.transform.position = Camera.main.transform.position;
//					Debug.LogError("going to 5nd point");
//					
//					if(i == 1) _5fingers[0] = new Vector2(x ,y );
//					if(i == 2) _5fingers[1] = new Vector2(x ,y );
//					if(i == 3) _5fingers[2] = new Vector2(x ,y );
//					if(i == 4) 
//					{
//						_5fingers[3] = new Vector2(x ,y );
//							Vector2 fiPoint = _5fingers[0];
//						Vector2 sePoint = _5fingers[1];
//						if(!hasCenterPoint )
//						{
//							
//							float lenght = float.MinValue;
//							
//							for (int i1 = 0; i1 < 3; i1++) {
//								for(int j = i1 + 1; j < 4; j ++)
//								{
//									float tLength = Vector2.Distance(_5fingers[i1],_5fingers[ j]);
//									if(tLength > lenght)
//									{
//										lenght = tLength;
//										fiPoint = _5fingers[ i1];
//										sePoint = _5fingers[ j];
//									}
//								}
//							}
//							
//							centerPoint = (sePoint - firstPoint) * 0.5f + firstPoint;//new Vector2((firstPoint.x + sePoint.x)/2, (firstPoint.y + sePoint.y)/2);
//							GUI.DrawTexture(new Rect((int)centerPoint.x - TouchTexture.width / 2, (int)centerPoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
//							GUI.Label(new Rect((int)centerPoint.x + TouchTexture.width, (int)centerPoint.y - 9, 60, 25), "Middle point");
//								GUI.DrawTexture(new Rect((int)fiPoint.x - TouchTexture.width / 2, (int)fiPoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
//								GUI.DrawTexture(new Rect((int)sePoint.x - TouchTexture.width / 2, (int)sePoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
//							hasCenterPoint = true;
//						}
//						else
//						{
////								
////								float maxLength = 0.0f;
////								for (int i1 = 0; i1 < 3; i1++) {
////								for(int j = i1; j < 4; j ++)
////								{
////									float tLength = Vector2.Distance(_5fingers[i1],_5fingers[ j]);
////									if(tLength > maxLength)
////									{
////										maxLength = tLength;									
////									}
////								}
////								
////								for (int i3 = 0; i3 < 4; i3++) {
////									// recompute center point if it accidentally out of 4 fingers region
////									if(Vector2.Distance(centerPoint, _5fingers[i3]) > maxLength)
////									{
////										float lenght = 0.0f;
////										 fiPoint = new Vector2(0,0);
////										 sePoint = new Vector2(0,0);
////										for (i1 = 0; i1 < 3; i1++) {
////											for(int j = i1 + 1; j < 4; j ++)
////											{
////												float tLength = Vector2.Distance(_5fingers[i1],_5fingers[ j]);
////												if(tLength > lenght)
////												{
////													lenght = tLength;
////													fiPoint = _5fingers[ i1];
////													sePoint = _5fingers[ j];
////												}
////											}
////										}
////							
//										//centerPoint = new Vector2((firstPoint.x + sePoint.x)/2, (firstPoint.y + sePoint.y)/2);
//										GUI.DrawTexture(new Rect((int)centerPoint.x - TouchTexture.width / 2, (int)centerPoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
//							GUI.Label(new Rect((int)centerPoint.x + TouchTexture.width, (int)centerPoint.y - 9, 60, 25), "Middle point");
//								GUI.DrawTexture(new Rect((int)fiPoint.x - TouchTexture.width / 2, (int)fiPoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
//								GUI.DrawTexture(new Rect((int)sePoint.x - TouchTexture.width / 2, (int)sePoint.y - TouchTexture.height / 2, TouchTexture.width, TouchTexture.height), TouchTexture, ScaleMode.ScaleToFit);
////										break;
////									}
////								}
////								
//								
//							//}
//						}
//					}
//					
//					
				else if (dummies.Count==5 )
				{	
					GameObject cube;
					cube = GameObject.Find("Cube");
					//print (cube.name);
					cube.transform.position = Camera.main.transform.position;
					if (dummy.Value.PreviousPosition.x < dummy.Value.Position.x - 10 )
					{						
						isRight = true;
					}					
					//					if(i == 1) _5fingers[0] = new Vector2(x ,y );
//					if(i == 1) _5fingers[0] = new Vector2(x ,y );				
				}
				else
				{
					hasCenterPoint = false;
					GameObject cube;
					cube = GameObject.Find("Cube");
					//print ("dua cube len truoc");
					//cube.transform.position = Camera.main.transform.position ;
		
					// Dua tro ve truoc as same as OrbitTouch code
					//var distance = GameObject.Find("touchCube").GetComponent("OrbitTouch").distance;
					string name1 = "engine_LOW";
					string name2 = "Transmission";

					Vector3 position =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + GameObject.Find(name2).transform.position;
		
					cube.transform.position = position;
				}
				
				if (Vector2.Distance(firstPoint,secondPoint)< 1) {
						
					return;}
				//Debug.LogError("6");
            }
//			Debug.LogError("value of i "+ i + "\n");
			if (dummies.Count != 3 && dummies.Count != 5) 
			{ 
				hasCenterPoint = false;
				currentDistance = 0;				
				return;
			}
			if ( currentDistance == 0)
			{
				currentDistance = Vector2.Distance(firstPoint,secondPoint);				
			}
			//else 
			//Debug.LogError("value first second " + firstPoint.ToString() + " " + secondPoint.ToString());
			//{
				if(currentDistance - Vector2.Distance(firstPoint,secondPoint)< -2 )
				{
					//Debug.LogError("is zoom in");
					if(Camera.main.fieldOfView < 30.0f)
					{
					Camera.main.fieldOfView -= 0.3f;
					}
					Camera.main.fieldOfView -= 0.8f;
					currentDistance = Vector2.Distance(firstPoint,secondPoint);			
					if (Camera.main.fieldOfView < 5) Camera.main.fieldOfView =5;
					
				}
				else if(currentDistance - Vector2.Distance(firstPoint,secondPoint) > 2 )
				{
					//Debug.LogError("is zoom out");
				
					Camera.main.fieldOfView += 0.8f;
					currentDistance = Vector2.Distance(firstPoint,secondPoint);			
					if (Camera.main.fieldOfView > 90) Camera.main.fieldOfView = 90;
				}
				
		//	}
			
			if(dummies.Count == 5)
			{
				GameObject.Find("Cube") .transform.position = Camera.main.transform.position;
				
				if(isRight)
				{						
					Animator aniM = GameObject.Find("engine_LOW").GetComponent(typeof(Animator)) as Animator;
						aniM.SetInteger("Order", 1);
				}
				else 
				{						
					Animator aniM = GameObject.Find("engine_LOW").GetComponent(typeof(Animator)) as Animator;
					aniM.SetInteger("Order", 0);
				}
				//Material myMaterial = (Material) Resources.Load("testMaterial", typeof(Material));
//				Debug.LogError("Go to dummies.Count == 5");
//					Debug.LogError(_5distances[0]);
//					Debug.LogError(_5distances[1]);
//					Debug.LogError(_5distances[2]);
//					Debug.LogError(_5distances[3]);
//					//Debug.LogError(_5distances[4]);
//				if(_5distances[0] == 0 && _5distances[1] == 0 && _5distances[2] == 0 && _5distances[3] == 0 )
//				{
//					for (int i2 = 0; i2 < 4; i2++) {
//						_5distances[i2] = (int) Vector2.Distance(_5fingers[i2], centerPoint);
//					}
//				}
//				else
//				{
//					if(_5distances[0] - (int)Vector2.Distance(_5fingers[0], centerPoint) < -3 
//					&& _5distances[1] - (int)Vector2.Distance(_5fingers[1], centerPoint) < -3
//					&& _5distances[2] - (int)Vector2.Distance(_5fingers[2], centerPoint) < -3
//					&& _5distances[3] - (int)Vector2.Distance(_5fingers[3], centerPoint) < -3
//						)
//					{
//						
//						Animator aniM = GameObject.Find("engine_LOW").GetComponent(typeof(Animator)) as Animator;
//						aniM.SetInteger("Order", 1);
//							for (int i2 = 0; i2 < 4; i2++) {
//							_5distances[i2] = (int) Vector2.Distance(_5fingers[i2], centerPoint);
//						}
//					}
//					else if(_5distances[0] - (int)Vector2.Distance(_5fingers[0], centerPoint) > 3
//					&& _5distances[1] - (int)Vector2.Distance(_5fingers[1], centerPoint) > 3 
//					&& _5distances[2] - (int)Vector2.Distance(_5fingers[2], centerPoint) > 3 
//					&& _5distances[3] - (int)Vector2.Distance(_5fingers[3], centerPoint) > 3 
//					)
//					{
//						Animator aniM = GameObject.Find("engine_LOW").GetComponent(typeof(Animator)) as Animator;
//						aniM.SetInteger("Order", 0);
//							for (int i2 = 0; i2 < 4; i2++) {
//							_5distances[i2] = (int) Vector2.Distance(_5fingers[i2], centerPoint);	
//						}						
//					}
//				}
			}
			
			//Debug.LogError(firstPoint.ToString() +" "+secondPoint.ToString() +"  "+ Vector2.Distance(firstPoint,secondPoint)+ " dumi count "+ dummies.Count);
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