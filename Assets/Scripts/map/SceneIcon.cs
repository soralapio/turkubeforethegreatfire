using UnityEngine;
using System.Collections;

public class SceneIcon : Icon {
	public string scenename;
	public GUIManager gm;
	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Loading(){
		GUI.Box(new Rect(Screen.width/2 - 50, Screen.height/2 - 30, 100, 60), "Ladataan...");
	}

	public override void Click(){
		gm.NormalGUIFuncs += Loading;
		Application.LoadLevel(scenename);
	}
}
