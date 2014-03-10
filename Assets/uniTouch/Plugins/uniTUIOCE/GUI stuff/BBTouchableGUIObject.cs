using UnityEngine;
using System.Collections;

// works with GUIText or GUITexture

public class BBTouchableGUIObject : MonoBehaviour {

	private bool touchDown = false;
	public GameObject notificationObject;
	public string touchDownMessage = "doTouchDown";
	public string touchUpMessage = "doTouchUp";
	
	void Start () 
	{ 
		if (notificationObject == null) notificationObject = gameObject;
	}	

	void Update()
	{
		bool didTouch = false;
		// get all the touches, see if one hits me
		int i;
		for (i = 0; i < iPhoneInput.touchCount; i++) {
			iPhoneTouch touch = iPhoneInput.GetTouch(i);
			Vector3 pos = new Vector3(touch.position.x,touch.position.y,0.0f);
			if (guiText != null) {
				if (guiText.HitTest(pos)) {
					didTouch = true;
					this.doTouchDown();
				}				
			} else {
				if (guiTexture.HitTest(pos)) {
					print ("gui texture hit touch");
					didTouch = true;
					this.doTouchDown();
				}								
			}
		}
		if (!didTouch && touchDown) {
			doTouchUp();
		}
	}
	
	public void doTouchDown()
	{
		print ("nhan xuong");
		if (touchDown) return;
		touchDown = true;
		notificationObject.SendMessage(touchDownMessage,SendMessageOptions.DontRequireReceiver);
	}

	public void doTouchUp()
	{
		if (!touchDown) return;
		touchDown = false;
		notificationObject.SendMessage(touchUpMessage,SendMessageOptions.DontRequireReceiver);
	}

}

