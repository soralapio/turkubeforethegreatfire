using UnityEngine;
using System.Collections;

public class MapIcon : Icon {
	public Story story;
	public int identity;
	private StudentController student;
	private GUIManager gm;
	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		student = GameObject.FindGameObjectWithTag("StudentController").GetComponent<StudentController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Click(){
		gm.EnterOverlay();
		student.Show (story);
	}
}
