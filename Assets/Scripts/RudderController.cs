using UnityEngine;
using System.Collections;

public class RudderController : MonoBehaviour {
	private const float minX = 2.0f;
	private const float maxX = 3.0f;
	private float curX = 2.5f;
	public bool RightPedal = true;
	private float originalPos;

	// Use this for initialization
	void Start () {
		
	}
	public float RudderPos
	{
		get { return NetJoyClient.RZ+32767.0f; }
	}
	// Update is called once per frame
	void Update () {
		float rPos = RudderPos;		
		float delta = (rPos * 1.0f / 65535.0f) - curX;
		
		if( delta == 0 )
			return;
		
		float newX = minX + (rPos * 1.0f / 65535.0f);
			
		if( !RightPedal )
			newX = maxX - (rPos * 1.0f / 65535.0f);
			
		transform.position = new Vector3( newX ,transform.position.y,transform.position.z);
		curX = curX + delta;
	
	}
}
