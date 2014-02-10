using UnityEngine;
using System.Collections;

public class ThrottleController : MonoBehaviour {
	private const float maxAngle = 30;
	private const float initialAngle = 270;
	private float curAngle = initialAngle;
	// Use this for initialization
	void Start () {
	}
	public float AxisPos
	{
		get { return maxAngle*2 - NetJoyClient.Z+32767.0f; }
	}
	// Update is called once per frame
	void Update () {
		float angle = (AxisPos * maxAngle*2 / 65535.0f) - maxAngle;
		float rotationX = curAngle;
		//rotationX = Mathf.Clamp( rotationX, initialAngle - maxAngle, initialAngle + maxAngle);
		float targetAngle = initialAngle + angle;
		float delta = targetAngle - rotationX;
		if( delta == 0 )
			return;
		//Debug.Log(curAngle + " " + angle + " " + delta + " " + targetAngle);
		
		transform.Rotate( new Vector3( 0, delta, 0 ));
		curAngle = curAngle + delta;
		
	}
}
