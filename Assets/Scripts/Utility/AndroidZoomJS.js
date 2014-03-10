#if UNITY_ANDROID || UNITY_IPHONE
var distance = 5.0;

var xSpeed = 250.0;
var ySpeed = 120.0;

var yMinLimit = -20;
var yMaxLimit = 80;

private var x = 0.0;
private var y = 0.0;
private var currentDistance = 0.0;
private var cube : GameObject;
private var engine : GameObject;

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;
		
	cube = GameObject.Find("Cube");
	engine = GameObject.Find("engine_LOW");
}

function LateUpdate () {
        if(Input.touchCount == 2)
        {
        	//cube.transform.position = Camera.main.transform.position ;
        	//float y = Input.GetTouch(0).position.y;
        	//float x2 = Input.GetTouch(1).position.x;
        	//float y2 = Input.GetTouch(1).position.y;
        	
        	var firstPoint :  Vector2 =  Input.GetTouch(0).position;
        	var secondPoint :  Vector2 =  Input.GetTouch(1).position;
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
//        	if(iPhoneInput.touchCount != 1)
//        	{
        			//var position : Vector3 =  Camera.main.transform.rotation * (new Vector3(0.0f, 0.0f, -3.5f)) + engine.transform.position;
					//cube.transform.position = position;
//					}
        }
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}

#endif