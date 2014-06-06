using UnityEngine;
using System.Collections;

using EventSystem;
using InputEventSystem;

/*
* A component used by 3D scene camera. This is enabled when the gyro is not in use. Basicly turns the camera according to player drag.
*/

public class PointerController : MonoBehaviour {
	private InputManager im;

	private Vector3 lastdir;
	private float xrot, yrot;

	private Vector3 wantToBeEuler;
	private Quaternion wantToBeRot;
	public bool pointerEnabled;

	private CoffeeHouseCamera chc;

	float inverseMulti;
	// Use this for initialization
	void Start () {
		wantToBeEuler = transform.localEulerAngles;
		chc = gameObject.GetComponent<CoffeeHouseCamera>();

		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>() as InputManager;
		im.PointerDrag += HandleDrag;

		lastdir = Vector3.zero;
		yrot = -transform.localEulerAngles.x;
		xrot = transform.localEulerAngles.y;

		#if UNITY_ANDROID
		inverseMulti = -1;
		#elif UNITY_IPHONE
		inverseMulti = -1;
		#elif UNITY_IOS
		inverseMulti = -1;
		#elif UNITY_EDITOR
		inverseMulti = 1;
		#else
		inverseMulti = 1;
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		if(!CanRotate()) return;

		if(Quaternion.Equals(transform.localRotation, wantToBeRot)) transform.localRotation = wantToBeRot;
		else transform.localRotation = Quaternion.Lerp(transform.localRotation, wantToBeRot, 0.1f);

	}

	private void HandleDrag(object o, DragEventArgs e){
		if(!CanRotate()) return;
		if(e.StartPosition == e.EndPosition)return;

		Vector3 applydir = lastdir *0.3f + e.Direction*inverseMulti;// * 0.7f;
		
		xrot = transform.localEulerAngles.y + applydir.x * e.Force *10f;
		
		yrot += applydir.y * e.Force*1.5f;
		yrot = Mathf.Clamp (yrot, -60, 60);
		
		wantToBeEuler = new Vector3(-yrot, xrot, 0);
		wantToBeRot = Quaternion.Euler(wantToBeEuler);
		
	}

	bool CanRotate(){
		return (pointerEnabled) && (chc.turnable) && (!chc.travelling);
	}

	void ForceRotationChange(Quaternion rot){
		wantToBeRot = rot;
	}
}
