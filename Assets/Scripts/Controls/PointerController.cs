﻿using UnityEngine;
using System.Collections;

using EventSystem;
using InputEventSystem;
public class PointerController : MonoBehaviour {
	private InputManager im;

	private Vector3 lastdir;
	private float xrot, yrot;

	public bool pointerEnabled;

	private CoffeeHouseCamera chc;
	// Use this for initialization
	void Start () {
		chc = gameObject.GetComponent<CoffeeHouseCamera>();

		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>() as InputManager;
		im.PointerDrag += HandleDrag;

		lastdir = Vector3.zero;
		yrot = -transform.localEulerAngles.x;
		xrot = transform.localEulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
		//lastdir = transform.forward;
	}

	private void HandleDrag(object o, DragEventArgs e){
		// ROTATE VIEW
		if(!pointerEnabled)return;
		if(!chc.turnable) return;
		if(chc.travelling) return;
		// following two lines were inside the IF above
		//yrot = -transform.localEulerAngles.x;
		//xrot = transform.localEulerAngles.y;

		Vector3 applydir = lastdir *0.3f + e.Direction * 0.7f;
		
		xrot = transform.localEulerAngles.y + applydir.x * e.Force *1.5f;
		
		yrot += applydir.y * e.Force*1.5f;
		yrot = Mathf.Clamp (yrot, -60, 60);
		
		transform.localEulerAngles = new Vector3(-yrot, xrot, 0);
	}
}
