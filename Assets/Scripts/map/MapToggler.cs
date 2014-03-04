using UnityEngine;
using System.Collections;

public class MapToggler : MonoBehaviour {
	public GameObject map1;
	public GameObject map1_l;
	public GameObject map2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(GUI.Button(new Rect(Screen.width-100, 0, 100, 80),"SMap")){
			map1.renderer.enabled = !map1.renderer.enabled;
			map1_l.renderer.enabled = !map1_l.renderer.enabled;
			map2.renderer.enabled = !map2.renderer.enabled;
		}
	}
}
