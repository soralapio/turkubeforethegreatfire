using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Controls;

/*
 * DON'T DESTROY!
 * this file is deprecated but holds the old touch-screen 
 * implementation. some of the code might get reused in
 * TouchInputDriver implementation.
 */

public class Control : MonoBehaviour {
	/*
	private bool panning;
	private Vector3 panvector;
	private List<float> panvelvec;
	private float panvelocity;
	private float cutoff;
	private bool fling;
	private float flingtimer;

	private float lpinch;
	private float pinch;

	private string content;

	private InputManager im;
	private InputEvent ie;
	
	// Use this for initialization
	void Start () {
		/*
		#if UNITY_EDITOR
			control = new DesktopGestureControl();
		#elif UNITY_ANDROID
			control = new AndroidGestureControl();
		#endif

		panning = false;
		panvelocity = 0;
		panvelvec = new List<float>();
		panvector = Vector3.zero;
		fling = false;
		flingtimer = 1;
		cutoff = 0.2f;
		content = "";
		lpinch = 0;
		pinch = 0;

		im = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>() as InputManager;
		im.registerEventListener(handleInput);

	}

	private void handleInput(InputEvent ie){
		this.ie = ie;
	}

	// Update is called once per frame
	void Update () {
		if(ie != null){
			if(ie.eventtype == (int)InputEvent.EventTypes.Zoom){
				float camOsize = Mathf.Clamp(camera.orthographicSize - ie.force, 2, 6);
				camera.orthographicSize = camOsize;
			}
		}

		content = "";
		A2D("Ts: " + Input.touchCount.ToString());
		A2D("Cam p: " + camera.transform.position.ToString());
		A2D("panning: "+panning.ToString());
		A2D("pan vel: "+panvelocity.ToString());
		A2D("pan vec: "+panvector.ToString());
		A2D("fling: "+fling.ToString());
		// use some control abstractor

		if(!fling && Input.touchCount == 1){// move or tap
			print("lol");
			Touch touch = Input.touches[0];

			A2D("");

			if(panvelvec.Count > 20) panvelvec.RemoveAt(0);
			panvelvec.Add(Mathf.Clamp( touch.deltaPosition.magnitude, 0, 10));
			float sum =0;
			foreach( float a in panvelvec) sum += a;
			panvelocity = sum/panvelvec.Count;


			A2D("D pos: " + touch.deltaPosition.ToString());
			A2D("D time: " + touch.deltaTime.ToString());

			if(panvelocity > cutoff)panning = true;




			if(touch.phase == TouchPhase.Ended){
				if(panning){ // fling maybe
					fling = true;
					flingtimer = panvelocity/10;
					print ("fling! " + flingtimer.ToString());
				}
				else{
					Ray panpoint = camera.ScreenPointToRay( new Vector3(touch.position.x, touch.position.y));
					RaycastHit hitinfo;
					if(Physics.Raycast(panpoint, out hitinfo)){
						if(hitinfo.transform.tag == "Icon"){
							hitinfo.transform.GetComponent<MapIcon>().Click();	
						}
					}
				}
				panning = false;
			}
			else{
				panvector = new Vector3(touch.deltaPosition.x, 0,touch.deltaPosition.y).normalized;
			}
		}
		else if(Input.touchCount == 2){ // pinch zoom
			panning = false;
			panvelocity = 0;
			fling = false;
			panvector = Vector3.zero;

			Touch touch1 = Input.touches[0];
			Touch touch2 = Input.touches[1];

			pinch = Vector2.Distance(touch1.position, touch2.position);

			if(lpinch > 0){
				float pdelta = pinch - lpinch;
				A2D("D pinch: " + pdelta.ToString());

				float camOsize = Mathf.Clamp(camera.orthographicSize - pdelta/50, 2, 6);
				camera.orthographicSize = camOsize;
			}

			lpinch = pinch;
		}
		else{
			lpinch = 0;
		}

		MoveCamera();
	}

	void OnGUI(){
		GUI.TextField(new Rect(5,5,250,Screen.height-10), "LOG:"+content);
	}

	private void MoveCamera(){
		// this does some power scaling and control reversion for touching
		camera.transform.position -= panvector * panvelocity * flingtimer / (55 - (camera.orthographicSize-2)*10); // so fucking random

		if(fling){ 
			flingtimer -= Time.deltaTime;
			print ("ft " + flingtimer.ToString());
			if(flingtimer <= 0){
				panvector = Vector3.zero;
				panvelocity = 0;
				fling = false;
				flingtimer = 1;
				panvelvec = new List<float>();
				print ("end fling!");
			}
		}
	}

	private void A2D(string s){
		content += "\n" + s;
	}
	*/
}
