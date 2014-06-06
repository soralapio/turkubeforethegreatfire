using UnityEngine;
using System.Collections;

using EventSystem;
using InputEventSystem;
using PH = Utils.PlatformHelper;

/*
*	Camera script for 3D scene cameras.
*	It is note-worthy that 3D scene camera prefab uses multiple scripts
*	and those scripts read the some public variables CoffeeHouseCamera exposes.
*	Those should have been handled with either EventManager or Unity's send messages.
*/

public class CoffeeHouseCamera : MonoBehaviour {

	public Texture2D backToMapIcon;
	public Texture2D resetCameraIcon;
	public LayerMask poiMask;
	private InputManager im;

	private EventManager EM;


	// adventuring camera stuff:
	private Vector3 originalCameraPosition;
	private Vector3 start;
	private Vector3 target;
	// should handle these with messages {
	public bool travelling;
	public bool turnable; // false == we are in gui overlay
	// }
	private bool notInOriginalPosition;
	private float timetravelled;
	
	// dont read PointerUp-event if PointerDrag just happened.
	private bool actualclick; // this is used in the mapcameracontrol.cs, too. 

	private PointOfInterest poi; // this is here for showing historical objects. why? because i was lazy and didnt bother to write CameraArrivedToPOI-Event

	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;

		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		im.PointerUp += HandleClick;
		im.PointerDrag += HandlePointerDrag;

		EM.EnterOverlay += HandleEnterOverlay;
		EM.ExitOverlay += HandleExitOverlay;

		originalCameraPosition = gameObject.transform.position;


		start = Vector3.zero;
		target = Vector3.zero;
		travelling = false;

		turnable = true;
	}

	void HandlePointerDrag (object o, DragEventArgs e)
	{
		actualclick = false;
	}

	private void HandleEnterOverlay(object o, EnterOverlayEventArgs e){
		turnable = false;
		im.PointerUp -= HandleClick;
		im.PointerDrag -= HandlePointerDrag;

	}

	private void HandleExitOverlay(object o, ExitOverlayEventArgs e){
		turnable = true;
		im.PointerUp += HandleClick;
		im.PointerDrag += HandlePointerDrag;
	}

	private void HandleClick(object o, PointerUpEventArgs e){
		// drag generates clicks too, and we dont want those:
		if(!actualclick){
			actualclick = true;
			return;
		}
		// SEARCH FOR POI
		if(travelling) return;
		Ray panpoint = camera.ScreenPointToRay( new Vector3(e.Position.x , e.Position.y));
		RaycastHit hitinfo;
		if(Physics.Raycast(panpoint, out hitinfo, 30, poiMask)){
			if(hitinfo.transform.GetComponent<PointOfInterest>() != null){
				notInOriginalPosition = true;
				poi = hitinfo.transform.GetComponent<PointOfInterest>();
				start = transform.position;
				target = poi.cameraoffset.position;
				travelling = true;
				gameObject.GetComponent<SmoothLookAt>().target = hitinfo.transform;
				timetravelled = 0;
			}
			else if(hitinfo.transform.GetComponent<Interactable>() != null){
				hitinfo.transform.GetComponent<Interactable>().GetClicked();
			}
		}
		actualclick = true;
	}



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
			gameObject.SendMessage("ForceRotationChange", transform.rotation);
			if(poi != null){
				poi.ShowInterest();
				poi = null;
			}
		}
	}

	private void OnGUI()
	{
		/*
		* This section should be made into a delegate and passed to GUIManager
		*
		*/
		
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
