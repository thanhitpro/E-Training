using UnityEngine;
using System.Collections;

/**
 * This class is inherited from BBSimpleTouchableObject, which come from the project uniTouch that we utilize in our project.
 * This class (Script) will be added into object Cube (in training mode) or touchCube (in some other modes).
 * The Cube then is assigne box collider to receive touch event and cube is made invisible by disable Cube renderer in Cube inspector.
 * The approach is to receive the touch event by touching the Cube, whenever the touch change, the Cube also changes in such a way so that
 * it can cover any touch ray before touch ray is going to engine. So Cube is in the middle of engine and camera when we need to orbit, and we put 
 * the Cube into or behind the camera position to make touch-to-orbit disable.
 * 
 * To understand how it works, please go to http://answers.unity3d.com/questions/64873/smooth-mouse-orbit.html.
 * I (Anh) modify that script to adapt our project requirement.
 */ 
public class OrbitTouch : BBSimpleTouchableObject {
	
	public Transform target ;
	public Transform cameraTran;
	public bool allowDrag = true;
	public bool allowScale = true;
	public bool allowRotate = true;
	
	public float minimumScale = 1.0f;
	public float maximumScale = 3.0f;

	private Transform saveParent;
	private Vector3 movement;
	
	protected GameObject pivot;
	
	 // min and max height of the platter:
    public float minHeight = 0.2f;
    public float maxHeight = 3.0f;
     
    // rate of elevation in transform position per unit of touch-movement:
    public float elevationRate = 0.01f;
    // rotation rate in degrees per unit of touch-movement:
    public float rotationRate = 0.3f;
	
	
	private float x = 0.0f;
	private float y = 0.0f;
	
	
	public float distance = 5.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -10.0f;
	public float yMaxLimit = 90.0f;
	
	private bool hasAssignXY = false;
	
	public OrbitTouch()
	{			  
	}

	public override void handleSingleTouch(iPhoneTouch touch) 
	{
		if(iPhoneInput.touchCount > 1) return;
		//print("Obit plane");
		if (!hasAssignXY)
		{
			y = transform.eulerAngles.x;
			x = transform.eulerAngles.y;
			hasAssignXY = true;
		}

			
        x += touch.deltaPosition.x * xSpeed * 0.0009f;
        y -= touch.deltaPosition.y * ySpeed * 0.0009f;
 		
 		y = ClampAngle((float)y, (float)yMinLimit, (float)yMaxLimit);
 		       
        var rotation = Quaternion.Euler((float)y, (float)x, 0);
        var position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
		var camPosition = rotation * (new Vector3(0.0f, 0.0f, -distance -0.5f)) + target.position;
        

        Camera.mainCamera.transform.rotation = rotation;
        Camera.mainCamera.transform.position = camPosition;
		transform.rotation = rotation;
        transform.position = position;	
	}
	
	
	
	void changeDistance(float value1)
	{
		distance -= value1;
	}
	
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}


