using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PH = Utils.PlatformHelper;
public class IconFilter : MonoBehaviour {
	private Dictionary<string, List<Icon>> iconlists;
	private Dictionary<string, bool> toggles;

	private Rect togglearea;
	// Use this for initialization
	void Start () {
		togglearea = new Rect(0, 0 ,200,Screen.height);
		togglearea = PH.ScaleToRatio(togglearea);
		togglearea.x = Screen.width - togglearea.width;
		togglearea.y = 0;

		iconlists = new Dictionary<string, List<Icon>>();
		toggles = new Dictionary<string, bool>();

		iconlists["Alueet"] = new List<Icon>();
		iconlists["Alueet"].AddRange(gameObject.GetComponentsInChildren<SceneIcon>());
		toggles["Alueet"] = true;

		iconlists["Tarinat"] = new List<Icon>();
		iconlists["Tarinat"].AddRange(gameObject.GetComponentsInChildren<MapIcon>());
		toggles["Tarinat"] = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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

}
