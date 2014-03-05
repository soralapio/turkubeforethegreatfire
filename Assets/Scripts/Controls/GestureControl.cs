using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InputEventSystem{

	using Utils;

	public delegate void InputEventListener(InputEvent e);

	public class InputEvent{ // should maybe be derived into many different subtypes
		// this event type thing needs some reconsideration

		public enum EventTypes : int {ClickDown, ClickUp, Holding, Drag, DragEnd, Zoom}; // this will probably get deprecated
		private int etype; // change to eventtype
		private Vector3 startp;
		private Vector3 endp;
		private Vector3 dir;
		private float f;

		public InputEvent(int eventtype, Vector3 startp, Vector3 endp, float force){
			etype = eventtype;
			this.startp = startp;
			this.endp = endp;
			this.dir = (endp - startp).normalized;
			this.f = force;
		}

		public Vector3 startpoint{
			get{ return startp; }
		}
		public Vector3 endpoint{
			get{ return endp; }
		}
		public Vector3 direction{
			get{ return dir; }
		}
		public int eventtype{
			get{return etype; }
		}
		public float force{
			get{return f; }
		}

	}


	public abstract class InputDriver : MonoBehaviour{
		/*
		 * The actual implementation for the getEvent should be done by
		 * using a FSM, but for now, it is quick and dirty! :<
		 */
		// used internally for figuring out event transmissions
		protected InputEvent lastevent;

		protected bool sending;

		// eventlistener interface: use these to register listeners
		public event InputEventListener pointerDownListeners; 
		public event InputEventListener pointerUpListeners;
		public event InputEventListener holdingListeners;
		public event InputEventListener dragListeners;
		public event InputEventListener zoomListeners;

		// some inputsmoothing:
		protected float dragforce; // smooth this value when dragging
		protected float cutoff; // magnitude of cursor position change required for "drag" event


		// called in mb.Update(): deals with unity input layer
		protected abstract void UpdateEventState();

		// implement this and call in mb.Awake() as constructors are hard, using mbs
		protected virtual void Init(){
			sending = true;
		}

		// callsafes for raising events
		protected void RaisePointerDown(InputEvent ie){
			if(!sending) return;
			//print ("Event: pointer down");
			if(pointerDownListeners != null) pointerDownListeners(ie);
		}
		protected void RaisePointerUp(InputEvent ie){
			if(!sending) return;
			//print ("Event: pointer up");
			if(pointerUpListeners != null) pointerUpListeners(ie);
		}
		protected void RaiseHolding(InputEvent ie){
			if(!sending) return;
			//print ("Event: holding");
			if(holdingListeners != null) holdingListeners(ie);
		}
		protected void RaiseDrag(InputEvent ie){
			if(!sending) return;
			//print ("Event: drag");
			if(dragListeners != null) dragListeners(ie);
		}
		protected void RaiseZoom(InputEvent ie){
			if(!sending) return;
			//print ("Event: zoom");
			if(zoomListeners != null) zoomListeners(ie);
		}

		public void SetState(bool working){
			sending = working;
		}
	}

	public class TouchInputDriver : InputDriver{
		/*This class is the touch input handler
		 * it has only been tested for android
		 * and contains various magic numbers for scaling.
		 * these numbers are only tested on nexus 7. 
		 */

		// TODO: figure out how to fix pinch-zoom asynchronous release
		// TODO: maybe try some timer based release for pointer up -event

		private bool touching;
		private bool dragging;
		private float lpinch;
		private float pinchdivider;
		protected override void Init(){
			base.Init();
			print ("Inherited touch driver!");
			lastevent = null;
			cutoff = 0.02f; // might require adjustments
			touching = false;
			dragging = false;
			lpinch = 0;
			pinchdivider = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
		}

		protected override void UpdateEventState(){
			InputEvent ie = null;
			
			if(Input.touches.Length == 1){// move or tap
				lpinch = 0;

				Touch touch = Input.touches[0];

				Vector3 position = new Vector3(touch.position.x, touch.position.y);

				switch(touch.phase){
				case TouchPhase.Began:
					if(!touching){
						touching = true;
						ie = new InputEvent((int)InputEvent.EventTypes.ClickDown, position, position, 0f);
						RaisePointerDown(ie);
					}
					break;

				case TouchPhase.Moved:
					if(touch.deltaPosition.magnitude > cutoff){
						dragging = true;
						ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, position, (lastevent.endpoint-position).magnitude/pinchdivider * 100);
						RaiseDrag(ie);
						//print ("dragging " + position.magnitude.ToString());
					}
					break;

				case TouchPhase.Stationary:
					if(dragging){
						ie = new InputEvent((int)InputEvent.EventTypes.Drag, position, position, 0);
						RaiseDrag(ie);
					}
					else{
						ie = new InputEvent((int)InputEvent.EventTypes.Holding, position, position, 0);
						RaiseHolding(ie);
					}
					break;

				default: // for now: catches both ENDED and CANCELED
					touching = false;
					if(dragging){
						ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, position, (lastevent.endpoint-position).magnitude/pinchdivider * 5);
						RaiseDrag(ie);
						dragging = false;
					}
					else{
						ie = new InputEvent((int)InputEvent.EventTypes.ClickUp, position, position, 0);
						RaisePointerUp(ie);
					}
					break;
				}

			}
			else if(Input.touches.Length == 2){ // pinch zoom

				Touch touch1 = Input.touches[0];
				Touch touch2 = Input.touches[1];
				
				float pinch = Vector2.Distance(touch1.position, touch2.position);
				if(lpinch > 0){
					Vector2 meanpos = (touch1.position + touch2.position)*0.5f;

					float pdelta = (pinch - lpinch)/pinchdivider *5f; // fuck :D
						
					ie = new InputEvent((int)InputEvent.EventTypes.Zoom, meanpos, new Vector3(meanpos.x, meanpos.y), pdelta);
					RaiseZoom(ie);
				}
				lpinch = pinch;
			}
			else{
				lpinch = 0;
			}

			lastevent = ie;
		}


	}

	public class MouseInputDriver : InputDriver{

		private bool down;

		protected override void Init ()
		{
			base.Init();
			print ("Inherited mouse driver!");
			lastevent = null;
			cutoff = 0f; // this might need tweaking for optimal experience
			dragforce = 0f;
			down = false;
		}

		protected override void UpdateEventState(){
			//if(lastevent != null)Debug.Log(lastevent.eventtype.ToString());
			//else Debug.Log("NULL");
			InputEvent ie = null;

			if(Input.GetMouseButton(0)){
				if(!down){
					ie = new InputEvent((int)InputEvent.EventTypes.ClickDown, Input.mousePosition, Input.mousePosition, 0);
					RaisePointerDown(ie);
					down = true;
				}
				else{
					if(lastevent.eventtype == (int)InputEvent.EventTypes.ClickDown || lastevent.eventtype == (int)InputEvent.EventTypes.Holding){
						if((lastevent.endpoint - Input.mousePosition).magnitude > cutoff){

							dragforce = 1;//no idea if this is a good idea. DEPRECATED: smoothDrag((Input.mousePosition + lastevent.endpoint).magnitude);

							ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, Input.mousePosition, dragforce);
							RaiseDrag(ie);
						}
						else{
							ie = new InputEvent((int)InputEvent.EventTypes.Holding, Input.mousePosition, Input.mousePosition, 0);
							RaiseHolding(ie);
						}
					}
					else if(lastevent.eventtype == (int)InputEvent.EventTypes.Drag){
						ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, Input.mousePosition, dragforce);
						RaiseDrag(ie);
					}
					else{
						ie = new InputEvent((int)InputEvent.EventTypes.Holding, Input.mousePosition, Input.mousePosition, 0);
						RaiseHolding(ie);
					}
				}
			}
			else if(!Input.GetMouseButton(0) && down){
					if(lastevent.eventtype == (int)InputEvent.EventTypes.Drag){
						ie = new InputEvent((int)InputEvent.EventTypes.DragEnd, lastevent.endpoint, Input.mousePosition, dragforce);
						RaiseDrag(ie);
					}
					else{
						ie = new InputEvent((int)InputEvent.EventTypes.ClickUp, Input.mousePosition, Input.mousePosition, 0);
						RaisePointerUp(ie);
					}
					down = false;
			}
			else if(Input.GetAxis("Mouse ScrollWheel") != 0){
				ie = new InputEvent((int)InputEvent.EventTypes.Zoom, Input.mousePosition, Input.mousePosition, Input.GetAxis("Mouse ScrollWheel"));
				RaiseZoom(ie);
			}

			lastevent = ie;
		}

	}



}