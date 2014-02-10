using UnityEngine;
using System.Collections;
using System.Threading;

public class Rotator : MonoBehaviour {
	private Timer timer = null;
	
	public int Interval;
	public Vector3 RotateSteps;
	private Vector3? deltaRotate;
	// Use this for initialization
	void Start () {
		timer = new Timer( timerTick, null, 0, Interval );
	}
	void Stop ()
	{
		timer.Change(Timeout.Infinite, Timeout.Infinite);
	}
	void timerTick( object state )
	{
		deltaRotate = RotateSteps;
	}
	// Update is called once per frame
	void Update () {
		if( deltaRotate != null )
		{
			transform.Rotate( RotateSteps );
			deltaRotate = null;
		}
	}
}
