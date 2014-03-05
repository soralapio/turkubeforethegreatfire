using UnityEngine;
using System.Collections;

using Utils;

public class GUIManager : Singleton<MonoBehaviour> {
	public GUISkin globalskin;
	public GUIStyle overlaystyle;

	public delegate void GuiHappening();
	public delegate void OnGUIFunction();

	public event GuiHappening EnteredOverlayListeners;
	public event GuiHappening ExitedOverlayListeners;

	// add all ongui things here. makes things more efficient:
	public event OnGUIFunction NormalGUIFuncs; 
	public event OnGUIFunction OverlayGUIFuncs;

	private bool overlay;

	void Awake(){
		overlay = false;
	}

	public void EnterOverlay(){
		overlay = true;
		if(EnteredOverlayListeners != null)EnteredOverlayListeners();
	}

	public void ExitOverlay(){
		overlay = false;
		if(ExitedOverlayListeners != null)ExitedOverlayListeners();
	}

	void OnGUI(){
		if(overlay){
			GUI.depth = 2; // i am guessing everything else is placed in depth 1?.. mysterious
			GUI.Label(new Rect(0,0,Screen.width, Screen.height),"", overlaystyle);
			if(OverlayGUIFuncs != null){
				GUI.depth = 1;
				OverlayGUIFuncs();
			}
		}
		else{
			if(NormalGUIFuncs != null){
				GUI.depth = 1;
				NormalGUIFuncs();
			}
		}
		

	}
}
