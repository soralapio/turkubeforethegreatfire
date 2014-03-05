using UnityEngine;
using System.Collections;

public class SettingsGui : MonoBehaviour {

	public GameObject Icons;

	private IconFilter ifilter;

	public Texture back;
	public Texture scenes;
	public Texture texts;

	private Rect backr;
	private Rect scenesr;
	private Rect textsr;

	private GUIManager gm;


	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();

		ifilter = Icons.GetComponent<IconFilter>();

		float w = back.width/2;
		float h = back.height/2;
		backr = new Rect(Screen.width-w, 0-h*0.1f, w, h);

		w = scenes.width/2;
		h = scenes.height/2;
		scenesr = new Rect(Screen.width - backr.width / 2 - w / 2 + 3, backr.height*0.15f, w,h);

		w = texts.width/2;
		h = texts.height/2;
		textsr = new Rect(Screen.width - backr.width / 2 - w / 2 + 7, backr.height*0.455f, w,h);


		gm.NormalGUIFuncs += DrawMenu;
	}

	private void DrawMenu(){
		GUI.DrawTexture( backr, back);//, ScaleMode.ScaleToFit);
		if(ifilter.GetToggleState(IconFilter.IconTypes.Alueet)){
			if(GUI.Button(scenesr, scenes, "Label")) ifilter.ToggleIcons(IconFilter.IconTypes.Alueet);
		}
		else{
			if(GUI.Button(scenesr, "", "Label")) ifilter.ToggleIcons(IconFilter.IconTypes.Alueet);
		}

		if(ifilter.GetToggleState(IconFilter.IconTypes.Tarinat)){
			if(GUI.Button(textsr, texts, "Label"))ifilter.ToggleIcons(IconFilter.IconTypes.Tarinat);
		}
		else{
			if(GUI.Button(textsr, "", "Label"))ifilter.ToggleIcons(IconFilter.IconTypes.Tarinat);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
