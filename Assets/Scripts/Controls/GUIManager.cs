using UnityEngine;
using System.Collections;

using Utils;
using EventSystem;
using CustomGUI;
public class GUIManager : Singleton<GUIManager> {
	/*
	 * Handles gui for now.
	 * The event things should be centralized!
	 * Lol.
	 */

	public GUISkin globalskin;
	public GUIStyle overlaystyle;
	public GUIStyle legendstyle;


	// add all ongui things here. makes things more efficient:
	public UIDrawDelegate NormalGUIFuncs; 
	public UIDrawDelegate OverlayGUIFuncs;


	private EventManager EM;

	private bool overlay;

	void Awake(){
		EM = EventManager.Instance;
		overlay = false;
		// nameless eventhandlers.
		EM.EnterOverlay += delegate {
			overlay = true;
				};
		EM.ExitOverlay += delegate {
			overlay = false;
				};

		this.useGUILayout = false; // disable GUILayouts. doubles performance, or somesuch
	}


	void OnGUI(){
		if(overlay){
			GUI.depth = 2; // i am guessing everything else is placed in depth 1?.. mysterious
			//GUI.Label(new Rect(0,0,Screen.width, Screen.height),"", overlaystyle);
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
