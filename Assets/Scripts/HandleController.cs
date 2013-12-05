using UnityEngine;
using System.Collections;

public class HandleController : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
			
		//var X = Mathf.Clamp( NetJoyClient.X, -maxX, maxX );
		//var Z = Mathf.Clamp( NetJoyClient.Y, -maxZ, maxZ );
		transform.eulerAngles = new Vector3( -NetJoyClient.X/1400.0f, 0, NetJoyClient.Y/1400.0f );
		

	}
}

