using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PH = Utils.PlatformHelper;
public class IconFilter : MonoBehaviour {
	public enum IconTypes : int{Alueet, Tarinat};

	private Dictionary<IconTypes, List<Icon>> iconlists;
	private Dictionary<IconTypes, bool> toggles;

	private Rect togglearea;
	// Use this for initialization
	void Start () {
		togglearea = new Rect(0, 0 ,200,Screen.height);
		togglearea = PH.ScaleToRatio(togglearea);
		togglearea.x = Screen.width - togglearea.width;
		togglearea.y = 0;

		iconlists = new Dictionary<IconTypes, List<Icon>>();
		toggles = new Dictionary<IconTypes, bool>();

		iconlists[IconTypes.Alueet] = new List<Icon>();
		iconlists[IconTypes.Alueet].AddRange(gameObject.GetComponentsInChildren<SceneIcon>());
		toggles[IconTypes.Alueet] = true;

		iconlists[IconTypes.Tarinat] = new List<Icon>();
		iconlists[IconTypes.Tarinat].AddRange(gameObject.GetComponentsInChildren<MapIcon>());
		toggles[IconTypes.Tarinat] = true;
	}
	
	public void ToggleIcons(IconTypes key){
		toggles[key] = !toggles[key];
		foreach(Icon g in iconlists[key]){
			g.SetState(toggles[key]);
		}
	}

	public bool GetToggleState(IconTypes key){
		return toggles[key]; 
	}
	/*
	void OnGUI(){
		GUILayout.BeginArea(togglearea);
		foreach(string s in iconlists.Keys){
			if(GUILayout.Button(s + " [" + (toggles[s]?"x":" ") + "]", GUILayout.Height(35 * PH.GetUIScaleMulti()))){
				toggles[s] = !toggles[s];
				foreach(Icon g in iconlists[s]){
					g.SetState(toggles[s]);
				}
			}
		}

		GUILayout.EndArea();
	}
	*/
}
