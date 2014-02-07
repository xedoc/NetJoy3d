using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {
	public delegate void OnScreenSizeChange(Vector2 newScreenSize);
	public event OnScreenSizeChange onScreenSizeChanged;
    Vector2 lastScreenSize;
	private static ScreenManager instance;   

    void Awake() {
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update() {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        
        if(this.lastScreenSize != screenSize) {
            this.lastScreenSize = screenSize;
            if(onScreenSizeChanged != null)
                onScreenSizeChanged(screenSize);
        }
    }
	public static ScreenManager Instance  
    {     
        get     
        {       
            if (instance ==  null)
                instance = GameObject.FindObjectOfType(typeof(ScreenManager)) as  ScreenManager;      
            return instance;    
        }   
	}
}
