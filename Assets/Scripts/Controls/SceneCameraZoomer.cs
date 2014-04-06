using UnityEngine;
using System.Collections;

using InputEventSystem;

public class SceneCameraZoomer : MonoBehaviour {
	private InputManager IM;
	private GUIManager GM;
	// Use this for initialization
	void Start () {
		IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		GM = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();

		IM.zoomListeners += ZoomHandler;
		GM.EnteredOverlayListeners += EnterOverlayhandler;
		GM.ExitedOverlayListeners += ExitOverlayHandler;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

	private void ZoomHandler(InputEvent ie){
		transform.position += (transform.forward * ie.force);
	}

	private void EnterOverlayhandler(){
		IM.zoomListeners -= ZoomHandler;
	}

	private void ExitOverlayHandler(){
		IM.zoomListeners += ZoomHandler;
	}
}
