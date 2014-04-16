﻿using UnityEngine;
using System.Collections;

using EventSystem;

public class MapIcon : Icon {
	private EventManager EM;
	public Story story;
	public string date;
	public int identity;
	private StudentController student;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		student = GameObject.FindGameObjectWithTag("StudentController").GetComponent<StudentController>();

		date = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Click(){
		EM.OnEnterOverlay(this, new EnterOverlayEventArgs());
		EM.OnDisplayLetter(this, new DisplayLetterEventArgs(story, date));
		//student.Show (story);
	}
}
