using UnityEngine;
using System.Collections;

using EventSystem;
public class Letter : Interactable {
	public Story story;
	private StudentController student;

	private EventManager EM;

	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		
		student = GameObject.FindGameObjectWithTag("StudentController").GetComponent<StudentController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void GetClicked ()
	{
		EM.OnEnterOverlay(this, new EnterOverlayEventArgs());
		student.Show (story);
	}
}
