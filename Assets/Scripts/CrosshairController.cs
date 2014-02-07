using UnityEngine;
using System.Collections;

public class CrosshairController : MonoBehaviour {
	private Vector2 crosshairCenter;
	private GUITexture bkgTexture;
	// Use this for initialization
	void Start () {
		bkgTexture = GameObject.Find("AimBackground").GetComponent("GUITexture") as GUITexture;
		crosshairCenter = new Vector2( guiTexture.pixelInset.x, guiTexture.pixelInset.y );
	}
	
	// Update is called once per frame
	void Update () {
		
		crosshairCenter.x = bkgTexture.pixelInset.width/2.0f - guiTexture.pixelInset.width/2.0f;
		crosshairCenter.y = bkgTexture.pixelInset.height/2.0f - guiTexture.pixelInset.height/2.0f;
		
		float x = NetJoyClient.X / 32767.0f * bkgTexture.pixelInset.width/2.0f + crosshairCenter.x;
		float y = -NetJoyClient.Y / 32767.0f * bkgTexture.pixelInset.height/2.0f + crosshairCenter.y;
		
		if( x == guiTexture.pixelInset.x && y == guiTexture.pixelInset.y )
			return;
		
		var newInset = new Rect(
			x, 
			y,
			guiTexture.pixelInset.width, guiTexture.pixelInset.height);
		
		guiTexture.pixelInset = newInset;
	}
}

