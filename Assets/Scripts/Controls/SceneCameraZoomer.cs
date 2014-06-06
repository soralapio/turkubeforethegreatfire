using UnityEngine;
using System.Collections;

using EventSystem;
using InputEventSystem;

// a quick test for 3D scene camera, how pinch zooming (and mouse scroll) would feel in the scene. Doesnt adhere to scene colliders.

public class SceneCameraZoomer : MonoBehaviour {
	private InputManager IM;
	private EventManager EM;

	void Start () {
		EM = EventManager.Instance;
		IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();


		IM.PointerZoom += ZoomHandler;
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
