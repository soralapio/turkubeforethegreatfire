using UnityEngine;
using System.Collections;

using EventSystem;

public class SceneIcon : Icon {
	public GUIStyle loadingstyle;
	public string scenename;
	private GUIManager gm;
	private EventManager EM;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Loading(){
		GUI.Box(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 100), "Ladataan...", loadingstyle);
	}

	public override void Click(){
		EM.OnEnterOverlay(this, new EnterOverlayEventArgs());
		gm.OverlayGUIFuncs += Loading;
		Application.LoadLevel(scenename);
	}
}
