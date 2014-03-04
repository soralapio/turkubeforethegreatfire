using UnityEngine;
using System.Collections;

public class NewspaperClickHandler :  Interactable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public override void GetClicked()
	{
		if(!VerifyDistance()) return;
		gameObject.BroadcastMessage ("StartShowing");

	}
}
