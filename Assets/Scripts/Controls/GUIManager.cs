using UnityEngine;
using System.Collections;

using Utils;
using EventSystem;
using CustomGUI;
public class GUIManager : Singleton<GUIManager> {
	/*
	 * This class is responsible for rendering GUI elements in a centralized fashion.
	 * Some old scripts mights have their own OnGUI functions. They shouldn't.
	 * 
	 * When in overlay-mode (rendering things passed to OverlayGUIFuncs), all other scene actors should unsubscribe from input events.
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
			GUI.depth = 2; // the bigger the number, the closer to the foreground
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
