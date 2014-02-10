using UnityEngine;
using System.Collections;

public class FlapsController : MonoBehaviour {
	private float curAngle = -30.0f;
	private NetJoyClient.FlapsPos lastFlapsPos;
	// Use this for initialization
	void Start () {
		lastFlapsPos = NetJoyClient.Flaps;
	}
	
	// Update is called once per frame
	void Update () {
		if( lastFlapsPos == NetJoyClient.Flaps )
			return;

		float newAngle = curAngle;
		Material newMaterial = renderer.sharedMaterial;
		switch( NetJoyClient.Flaps )
		{
			case NetJoyClient.FlapsPos.Raised:
				newAngle = -30.0f;
				newMaterial = Resources.Load("Green") as Material;
				break;
			case NetJoyClient.FlapsPos.Combat:
				newAngle = -15.0f;
				newMaterial = Resources.Load("Yellow") as Material;
				break;
			case NetJoyClient.FlapsPos.Takeoff:
				newAngle = 0.0f;
				newMaterial = Resources.Load("Yellow") as Material;
				break;
			case NetJoyClient.FlapsPos.Landing:
				newAngle = 30.0f;
				newMaterial = Resources.Load("Red") as Material;
				break;			
		}
		float delta = newAngle - curAngle;
		if( delta != 0 )
		{
			transform.Rotate( new Vector3( 0, 0, delta ));
			if( !newMaterial.Equals( renderer.sharedMaterial ) )
				renderer.sharedMaterial = newMaterial;
			curAngle = newAngle;
		}
	}
}
