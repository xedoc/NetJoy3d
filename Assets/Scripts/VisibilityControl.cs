using UnityEngine;
using System.Collections;
using System;

public class VisibilityControl : MonoBehaviour {
	private bool curStatus = true;
	private Transform trans;
	// Use this for initialization
	void Start () {
		trans = gameObject.transform;
	}
	public virtual bool Status
	{
		get { 
			return NetJoyClient.DataValid;
		}
	}
	// Update is called once per frame
	void Update () {
		
		if( Status == curStatus )
			return;
		
		if( Status && !curStatus )
		{
			SetActiveRecurse(true);
		}
		else if( !Status && curStatus )
		{
			SetActiveRecurse(false);
		}
	}
	
	void SetActiveRecurse(bool status)
	{
		curStatus = status;
		foreach( Transform child in trans )
		{
			child.gameObject.SetActive(status);
		}
	}
}
