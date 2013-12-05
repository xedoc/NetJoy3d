using UnityEngine;
using System.Collections;

public class GearController : MonoBehaviour {
	private float curAngle = 30.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if( NetJoyClient.Gear && curAngle != 30.0f )
		{
			curAngle = 30.0f;
			transform.Rotate( new Vector3( 0, 0, 60 ));
			renderer.sharedMaterial = Resources.Load("Green") as Material;
		}
		else if( !NetJoyClient.Gear && curAngle != -30.0f )
		{
			curAngle = -30.0f;
			transform.Rotate( new Vector3( 0, 0, -60));
			
			renderer.sharedMaterial = Resources.Load("Red") as Material;
		}
	}
}
