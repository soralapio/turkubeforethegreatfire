﻿using UnityEngine;
using System.Collections;

using Utils;

public abstract class Icon : MonoBehaviour {
	public float visibilitylevel;

	// Use this for initialization
	void Awake(){
		transform.localScale *= Utils.PlatformHelper.GetUIScaleMulti();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void Click();


	// these icons can most likely still be clicked:
	//		maybe fix that, retard joni (when time is right
	public void Hide(){
		renderer.enabled = false;
		collider.enabled = false;
	}

	public void Show(){
		renderer.enabled = true;
		collider.enabled = true;
	}

	public void SetState(bool state){
		renderer.enabled = state;
		collider.enabled = state;
	}
}
