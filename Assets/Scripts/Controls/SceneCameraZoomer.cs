using UnityEngine;
using System.Collections;

using EventSystem;
using InputEventSystem;

public class SceneCameraZoomer : MonoBehaviour {
	private InputManager IM;
	private EventManager EM;
	//private GUIManager GM;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		//GM = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();

		IM.PointerZoom += ZoomHandler;
		/*
		GM.EnteredOverlayListeners += EnterOverlayhandler;
		GM.ExitedOverlayListeners += ExitOverlayHandler;
		*/
		EM.EnterOverlay += HandleEnterOverlay;
		EM.ExitOverlay += HandleExitOverlay;
	}

	void HandleExitOverlay (object o, ExitOverlayEventArgs e)
	{
		IM.PointerZoom += ZoomHandler;
	}

	void HandleEnterOverlay (object o, EnterOverlayEventArgs e)
	{
		IM.PointerZoom -= ZoomHandler;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

	private void ZoomHandler(object o, ZoomEventArgs e){
		transform.position += (transform.forward * e.Force);
	}

}
