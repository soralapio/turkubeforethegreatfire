using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using InputEventSystem;
public class MapCameraControl : MonoBehaviour {
	private const float ORTHOMAXSIZE = 6;
	private const float ORTHOMINSIZE = 2;
	private const float ORTHODIST = 4; // orthomaxsize - orthominsize
	private const float VERTICALBOUNDING = 8; // = about map.height/2 + 2
	private const float HORIZONTALBOUNDING = 8; // = about map.width/2 + 2
	

	private InputManager im;
	private GUIManager gm;

	private bool inputsregistered;

	// Use this for initialization
	void Start () {

		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>() as InputManager;
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>() as GUIManager;
		// register listeners:
		RegisterInputListeners();
		gm.EnteredOverlayListeners += HandleOverlayEnter;
		gm.ExitedOverlayListeners += HandlerOverlayExit;

		// fix icon visibility
		UpdateIcons();
	}

	private void RegisterInputListeners(){
		if(inputsregistered) return;
		im.zoomListeners += HandleZoomEvent;
		im.pointerUpListeners += HandleClickEvent;
		im.dragListeners += HandleDragEvent;
		inputsregistered = true;
	}

	private void UnregisterInputListeners(){
		if(!inputsregistered) return;
		im.zoomListeners -= HandleZoomEvent;
		im.pointerUpListeners -= HandleClickEvent;
		im.dragListeners -= HandleDragEvent;
		inputsregistered = false;
	}

	private void HandleOverlayEnter(){
		UnregisterInputListeners();
	}

	private void HandlerOverlayExit(){
		RegisterInputListeners();
	}

	private void HandleZoomEvent(InputEvent ie){
		float camOsize = Mathf.Clamp(camera.orthographicSize - ie.force, ORTHOMINSIZE, ORTHOMAXSIZE);
		camera.orthographicSize = camOsize;
		//UpdateIcons();
	}

	private void HandleClickEvent(InputEvent ie){
		Ray panpoint = camera.ScreenPointToRay( new Vector3(ie.endpoint.x , ie.endpoint.y));
		RaycastHit hitinfo;
		if(Physics.Raycast(panpoint, out hitinfo)){
			if(hitinfo.transform.tag == "Icon"){
				hitinfo.transform.GetComponent<Icon>().Click();	
			}

		}
	}

	private void HandleDragEvent(InputEvent ie){
		// handles panning the camera over the map

		//mg.Hide(); // hides the student
		// smoothing and adjusting required
		Vector3 movector = new Vector3(ie.direction.x, 0f, ie.direction.y);
		// the magic divisor in the end: smaller = faster movement | bigger = slower movement
		transform.position -= movector * ie.force * ((camera.orthographicSize / ORTHODIST)/12);
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
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Icon")){ // maybe stick these icons into a local var
			Icon i = go.GetComponent<Icon>();
			if(camera.orthographicSize > i.visibilitylevel) i.Hide();
			else i.Show();
		}
	}

	private Vector2 GetZoomedBounds(){
		float screenrat = Screen.width / Screen.height;
		return new Vector2(HORIZONTALBOUNDING - camera.orthographicSize*screenrat, VERTICALBOUNDING - camera.orthographicSize);
	}
	
}
