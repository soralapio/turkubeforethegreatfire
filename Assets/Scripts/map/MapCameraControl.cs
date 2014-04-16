using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using EventSystem;
using InputEventSystem;
public class MapCameraControl : MonoBehaviour {
	private const float ORTHOMAXSIZE = 6;
	private const float ORTHOMINSIZE = 2;
	private const float ORTHODIST = 4; // orthomaxsize - orthominsize
	private const float VERTICALBOUNDING = 8; // = about map.height/2 + 2
	private const float HORIZONTALBOUNDING = 8; // = about map.width/2 + 2
	
	private bool actualclick; // this should go into the input events but dont have time to fix that right now.

	private InputManager im;
	private GUIManager gm;

	private EventManager EM;

	private bool inputsregistered;

	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>() as InputManager;
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>() as GUIManager;
		// register listeners:
		RegisterInputListeners();
		EM.EnterOverlay += HandleEnterOverlay;
		EM.ExitOverlay += HandleExitOverlay;
	
		actualclick = true;
		// fix icon visibility
		UpdateIcons();
	}

	void HandleExitOverlay (object o, ExitOverlayEventArgs e)
	{
		RegisterInputListeners();
	}

	void HandleEnterOverlay (object o, EnterOverlayEventArgs e)
	{
		UnregisterInputListeners();
	}

	private void RegisterInputListeners(){
		if(inputsregistered) return;
		im.PointerZoom += HandleZoomEvent;
		im.PointerUp += HandleClickEvent;
		im.PointerDrag += HandleDragEvent;
		// old ways:
		//im.zoomListeners += HandleZoomEvent;
		//im.pointerUpListeners += HandleClickEvent;
		//im.dragListeners += HandleDragEvent;
		inputsregistered = true;
	}

	private void UnregisterInputListeners(){
		if(!inputsregistered) return;
		im.PointerZoom -= HandleZoomEvent;
		im.PointerUp -= HandleClickEvent;
		im.PointerDrag -= HandleDragEvent;
		//im.zoomListeners -= HandleZoomEvent;
		//im.pointerUpListeners -= HandleClickEvent;
		//im.dragListeners -= HandleDragEvent;
		inputsregistered = false;
	}


	private void HandleZoomEvent(object o, ZoomEventArgs e){
		float camOsize = Mathf.Clamp(camera.orthographicSize - e.Force, ORTHOMINSIZE, ORTHOMAXSIZE);
		camera.orthographicSize = camOsize;
		//UpdateIcons();
	}

	private void HandleClickEvent(object o, PointerUpEventArgs e){
		if(!actualclick){
			actualclick = true;
			return;
		}
		Ray panpoint = camera.ScreenPointToRay( new Vector3(e.Position.x , e.Position.y));
		RaycastHit hitinfo;
		if(Physics.Raycast(panpoint, out hitinfo)){
			if(hitinfo.transform.tag == "Icon"){
				hitinfo.transform.GetComponent<Icon>().Click();	
			}

		}
		actualclick = true;
	}

	private void HandleDragEvent(object o, DragEventArgs e){
		// handles panning the camera over the map

		//mg.Hide(); // hides the student
		// smoothing and adjusting required
		Vector3 movector = new Vector3(e.Direction.x, 0f, e.Direction.y);
		// the magic divisor in the end: smaller = faster movement | bigger = slower movement
		transform.position -= movector * e.Force * ((camera.orthographicSize / ORTHODIST)/12);

		actualclick = false;

	}

	// Update is called once per frame
	void Update () {
		//fix camera position ( this can be optimized to be done only when the camera actually moves :D
		//for some reason, z is y, and x is x
		Vector2 bounds = GetZoomedBounds();
		Vector3 ctp = camera.transform.position;
		camera.transform.position = new Vector3(Mathf.Clamp(ctp.x, -bounds.x, bounds.x), ctp.y, Mathf.Clamp(ctp.z, -bounds.y, bounds.y));
	}

	private void UpdateIcons(){
		// at the moment, this isn't actually used.
		/*
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Icon")){ // maybe stick these icons into a local var
			Icon i = go.GetComponent<Icon>();
			if(camera.orthographicSize > i.visibilitylevel) i.Hide();
			else i.Show();
		}
		*/
	}

	private Vector2 GetZoomedBounds(){
		float screenrat = Screen.width / Screen.height;
		return new Vector2(HORIZONTALBOUNDING - camera.orthographicSize*screenrat, VERTICALBOUNDING - camera.orthographicSize);
	}
	
}
