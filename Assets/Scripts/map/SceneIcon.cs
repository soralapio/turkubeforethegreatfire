using UnityEngine;
using System.Collections;

public class SceneIcon : Icon {
	public string scenename;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Click(){
		Application.LoadLevel(scenename);
	}
}
