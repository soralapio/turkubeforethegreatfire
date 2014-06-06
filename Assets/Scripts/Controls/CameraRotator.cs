using UnityEngine;
using System.Collections;
/*
* Little script for rotating a game object. Isn't in use anymore.
*/
public class CameraRotator : MonoBehaviour {

	public Texture2D backToMapButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, 0.3f);
	}

	void OnGUI(){
		if(GUI.Button(new Rect(10, Screen.height - 100, backToMapButton.width , backToMapButton.height), backToMapButton)){
			Application.LoadLevel(0);
		}
	}
}
