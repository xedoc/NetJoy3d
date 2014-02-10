using UnityEngine;
using System.Collections;

public class HandleController : MonoBehaviour {
	private Vector3 handlePos;

	// Use this for initialization
	void Start () {
		handlePos = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {

		var x = -NetJoyClient.X/1400.0f;
		var z = NetJoyClient.Y/1400.0f;
		if( handlePos.x != x || handlePos.z != z )
		{
			handlePos.x = x;
			handlePos.z = z;
			transform.eulerAngles = handlePos;
		}
		

	}
}

