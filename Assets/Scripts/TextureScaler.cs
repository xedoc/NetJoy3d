using UnityEngine;
using System.Collections;

public class TextureScaler	 : MonoBehaviour {
	public Vector2 DefaultScreenSize = new Vector2(320,240);
	private Rect originalRect;
	private Rect currentRect;
	private Vector2 originalScreenSize;
	private float aspectRatio;
	// Use this for initialization
	void Start () {
		
		ScreenManager.Instance.onScreenSizeChanged += ScreenSizeChanged;	
		originalRect = guiTexture.pixelInset;			
		originalScreenSize = DefaultScreenSize;
		aspectRatio = originalRect.width/originalRect.height;
		ScreenSizeChanged( new Vector2(Screen.width, Screen.height));	
		currentRect = originalRect;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ScreenSizeChanged( Vector2 newSize )
	{
		float scaledWidth = originalRect.width * newSize.x / originalScreenSize.x;
		float scaledHeight = originalRect.height * newSize.y / originalScreenSize.y;

		if( currentRect.width != scaledWidth )
		{
			currentRect.width = scaledWidth;
			currentRect.height = scaledWidth * aspectRatio;			
			guiTexture.pixelInset = currentRect;
		}

		if( currentRect.height != scaledHeight )
		{
			currentRect.width = scaledHeight * aspectRatio;
			currentRect.height = scaledHeight;			
			guiTexture.pixelInset = currentRect;
		}
	}
}
