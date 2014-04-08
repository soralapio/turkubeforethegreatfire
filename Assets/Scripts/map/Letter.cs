using UnityEngine;
using System.Collections;

public class Letter : Interactable {
	public Story story;
	private GUIManager gm;
	private StudentController student;
	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		student = GameObject.FindGameObjectWithTag("StudentController").GetComponent<StudentController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void GetClicked ()
	{
		gm.EnterOverlay();
		student.Show (story);
	}
}
