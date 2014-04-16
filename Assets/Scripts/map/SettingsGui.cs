using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using EventSystem;

public class SettingsGui : MonoBehaviour {

	private EventManager EM;

	public Texture back;
	public Texture scenes;
	public Texture texts;

	private Rect backr;
	private Rect scenesr;
	private Rect textsr;

	private GUIManager gm;

	public enum IconTypes : int{Alueet, Tarinat};
	private Dictionary<IconTypes, bool> toggles;
	private Dictionary<IconTypes, string> iconnames;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();

		float w = back.width/2;
		float h = back.height/2;
		backr = new Rect(Screen.width-w, 0-h*0.1f, w, h);

		w = scenes.width/2;
		h = scenes.height/2;
		scenesr = new Rect(Screen.width - backr.width / 2 - w / 2 + 3, backr.height*0.15f, w,h);

		w = texts.width/2;
		h = texts.height/2;
		textsr = new Rect(Screen.width - backr.width / 2 - w / 2 + 7, backr.height*0.455f, w,h);

		toggles = new Dictionary<IconTypes, bool>();
		toggles[IconTypes.Alueet] = true;
		toggles[IconTypes.Tarinat] = true;
		// dafug : really?
		iconnames = new Dictionary<IconTypes, string>();
		iconnames[IconTypes.Alueet] = "scene";
		iconnames[IconTypes.Tarinat] = "text";


		gm.NormalGUIFuncs += DrawMenu;
	}

	private void DrawMenu(){
		GUI.DrawTexture( backr, back);//, ScaleMode.ScaleToFit);
		if(toggles[IconTypes.Alueet]){
			if(GUI.Button(scenesr, scenes, "Label")) Toggle (IconTypes.Alueet);
		}
		else{
			if(GUI.Button(scenesr, "", "Label")) Toggle (IconTypes.Alueet);
		}

		if(toggles[IconTypes.Tarinat]){
			if(GUI.Button(textsr, texts, "Label"))Toggle (IconTypes.Tarinat);
		}
		else{
			if(GUI.Button(textsr, "", "Label"))Toggle (IconTypes.Tarinat);
		}
	}

	private void Toggle(IconTypes it){
		toggles[it] = !toggles[it];
		EM.OnSetMapIconsVisibility(this, new SetMapIconsVisibilityEventArgs(iconnames[it], toggles[it]));
	}
	// Update is called once per frame
	void Update () {
	
	}
}
