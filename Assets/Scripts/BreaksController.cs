using UnityEngine;
using System.Collections;

public class BreaksController : MonoBehaviour {
	public bool RightPedal = true;	
	private const float maxAngle = 20;
	private float initialAngle;
	private float curAngle;
	private Vector2 oldPos;
	
	void Start () {
		oldPos = new Vector2(NetJoyClient.RX, NetJoyClient.RY);
		initialAngle = transform.rotation.y; 
		curAngle = initialAngle;
	}
	private float AxisPos
	{
		get { 
			if( RightPedal )
				return maxAngle - NetJoyClient.RX+32767.0f; 
			else
				return maxAngle - NetJoyClient.RY+32767.0f; 
		}
	}
	// Update is called once per frame
	void Update () {
		float angle = (AxisPos * maxAngle / 65535.0f) - maxAngle;
		float delta = (initialAngle + angle) - curAngle;
		if( delta == 0 )
			return;
		
		transform.Rotate( new Vector3( 0, delta , 0 ));
		curAngle = curAngle + delta;
	}
}
