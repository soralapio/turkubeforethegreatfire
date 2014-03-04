using UnityEngine;
using System.Collections;

using InputEventSystem;
using PH = Utils.PlatformHelper;
public class CoffeeHouseCamera : MonoBehaviour {

	public Texture2D backToMapIcon;
	public Texture2D resetCameraIcon;

	private InputManager im;

	private GUIManager gm;


	// adventuring camera stuff:
	private Vector3 originalCameraPosition;
	private Vector3 start;
	private Vector3 target;
	public bool travelling;
	public bool turnable; // false == we are in overlay
	private bool notInOriginalPosition;
	private float timetravelled;

	// Use this for initialization
	void Start () {
		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		im.pointerUpListeners += HandleClick;

		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		gm.EnteredOverlayListeners += HandleOverlayEnter;
		gm.ExitedOverlayListeners += HandleOverlayExit;

		originalCameraPosition = gameObject.transform.position;


		start = Vector3.zero;
		target = Vector3.zero;
		travelling = false;

		turnable = true;
	}

	private void HandleOverlayEnter(){
		turnable = false;
		im.pointerUpListeners -= HandleClick;

	}

	private void HandleOverlayExit(){
		turnable = true;
		im.pointerUpListeners += HandleClick;
	}

	private void HandleClick(InputEvent ie){
		// SEARCH FOR POI
		if(travelling) return;
		Ray panpoint = camera.ScreenPointToRay( new Vector3(ie.endpoint.x , ie.endpoint.y));
		RaycastHit hitinfo;
		if(Physics.Raycast(panpoint, out hitinfo)){
			if(hitinfo.transform.GetComponent<PointOfInterest>() != null){
				notInOriginalPosition = true;
				PointOfInterest poi = hitinfo.transform.GetComponent<PointOfInterest>();
				start = transform.position;
				target = poi.cameraoffset;
				travelling = true;
				gameObject.GetComponent<SmoothLookAt>().target = hitinfo.transform;
				timetravelled = 0;
			}
			else if(hitinfo.transform.GetComponent<Interactable>() != null){
				hitinfo.transform.GetComponent<Interactable>().GetClicked();
			}
		}
	}



	// Update is called once per frame
	void Update () {
		if(travelling){
			travelFromP2P();
		}
	}
	
	
	private void travelFromP2P(){
		if(transform.position != target){
			timetravelled += Time.deltaTime;
			transform.position = Vector3.Slerp(start, target, timetravelled);
		}
		else{
			travelling = false;
			gameObject.GetComponent<SmoothLookAt>().target = null;

		}
	}

	private void OnGUI()
	{
		if(!turnable) return;
		if (notInOriginalPosition) {
		
			if (GUI.Button (new Rect (50, Screen.height - 150, resetCameraIcon.width, resetCameraIcon.height), resetCameraIcon)) 
			{

				notInOriginalPosition = false;
				start = transform.position;
				target = originalCameraPosition;
				travelling = true;
				gameObject.GetComponent<SmoothLookAt>().target = GameObject.Find ("cameraLookTarget").GetComponent("Transform") as Transform;
				timetravelled = 0;

			}

		}


			
		if (GUI.Button (new Rect(Screen.width - 50 - backToMapIcon.width, Screen.height - 150, backToMapIcon.width, backToMapIcon.height), backToMapIcon))
		{

			Application.LoadLevel ("map");
			                         
		}

		GUILayout.BeginArea(new Rect(Screen.width-150, 0, 150,200));
		if(GUILayout.Button("Pointer Controlled", GUILayout.Height(35* PH.GetUIScaleMulti()))){
			gameObject.GetComponent<GyroController>().gyroEnabled = false;
			gameObject.GetComponent<PointerController>().pointerEnabled = true;
		}
		if(GUILayout.Button("Gyro Controlled", GUILayout.Height(35* PH.GetUIScaleMulti()))){
			gameObject.GetComponent<GyroController>().gyroEnabled = true;
			gameObject.GetComponent<PointerController>().pointerEnabled = false;
		}
		GUILayout.EndArea();

	
	}
}
