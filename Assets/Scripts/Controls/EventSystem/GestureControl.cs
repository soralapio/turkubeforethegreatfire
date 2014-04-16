using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InputEventSystem{

	using Utils;
	using EventSystem;

	/*
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
	*/

	public abstract class InputDriver : MonoBehaviour{
		/*
		 * The actual implementation for the getEvent should be done by
		 * using a FSM, but for now, it is quick and dirty! :<
		 */
		// used internally for figuring out event transmissions
		protected InputEventArgs lastevent;

		protected bool sending;

		// eventlistener interface: use these to register listeners
		public event EventHandler<PointerDownEventArgs> PointerDown; 
		public event EventHandler<PointerUpEventArgs> PointerUp;
		public event EventHandler<HoldingEventArgs> PointerHolding;
		public event EventHandler<DragEventArgs> PointerDrag;
		public event EventHandler<ZoomEventArgs> PointerZoom;

		// some inputsmoothing:
		protected float dragforce; // smooth this value when dragging
		protected float cutoff; // magnitude of cursor position change required for "drag" event

		protected float elapsedtime;


		// called in mb.Update(): deals with unity input layer
		protected abstract void UpdateEventState();

		// implement this and call in mb.Awake() as constructors are hard, using mbs
		protected virtual void Init(){
			sending = true;
		}

		// callsafes for raising events
		protected void OnPointerDown(PointerDownEventArgs ie){
			if(!sending) return;
			//print ("Event: pointer down");
			if(PointerDown != null) PointerDown(this, ie);
		}
		protected void OnPointerUp(PointerUpEventArgs ie){
			if(!sending) return;
			//print ("Event: pointer up");
			if(PointerUp != null) PointerUp(this, ie);
		}
		protected void OnHolding(HoldingEventArgs ie){
			if(!sending) return;
			//print ("Event: holding");
			if(PointerHolding != null) PointerHolding(this, ie);
		}
		protected void OnDrag(DragEventArgs ie){
			if(!sending) return;
			//print ("Event: drag");
			if(PointerDrag != null) PointerDrag(this, ie);
		}
		protected void OnZoom(ZoomEventArgs ie){
			if(!sending) return;
			//print ("Event: zoom");
			if(PointerZoom != null) PointerZoom(this, ie);
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
			InputEventArgs ie = new InputEventArgs();
			if(Input.touches.Length == 1){// move or tap
				lpinch = 0;

				Touch touch = Input.touches[0];

				Vector3 position = new Vector3(touch.position.x, touch.position.y);

				switch(touch.phase){
				case TouchPhase.Began:
					if(!touching){
						elapsedtime = 0; // holding timer reset
						touching = true;
						ie = new PointerDownEventArgs(position);
						OnPointerDown((PointerDownEventArgs)ie);
						print ("pointer down");
					}
					break;

				case TouchPhase.Moved:
					if(touch.deltaPosition.magnitude > cutoff){
						dragging = true;
						//ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, position, (lastevent.endpoint-position).magnitude/pinchdivider * 100);
						ie = new DragEventArgs(lastevent.Position, position, (lastevent.Position-position).magnitude/pinchdivider * 100);
						OnDrag((DragEventArgs)ie);
						//print ("dragging " + (lastevent.endpoint-position).magnitude.ToString());
					}
					else{
						elapsedtime += Time.deltaTime;
						//ie = new InputEvent((int)InputEvent.EventTypes.Holding, position, position, 0);
						ie = new HoldingEventArgs(position, elapsedtime);
						OnHolding((HoldingEventArgs)ie);
						print ("holding");
					}
					break;

				case TouchPhase.Stationary:
					if(dragging){
						//ie = new InputEvent((int)InputEvent.EventTypes.Drag, position, position, 0);
						ie = new DragEventArgs(position, position, 0);
						OnDrag((DragEventArgs)ie);
						print ("stationary drag");
					}
					else{
						elapsedtime += Time.deltaTime;
						ie = new HoldingEventArgs(position, elapsedtime);
						OnHolding((HoldingEventArgs)ie);
						print ("holding");
					}
					break;

				default: // for now: catches both ENDED and CANCELED
					touching = false;
					if(dragging){
						// end drag:
						//ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, position, (lastevent.endpoint-position).magnitude/pinchdivider * 5);
						ie = new DragEventArgs(lastevent.Position, position, (lastevent.Position-position).magnitude/pinchdivider * 5);
						OnDrag((DragEventArgs)ie);

						print ("drag ended, fuck");

						// touch up:
						ie = new PointerUpEventArgs(position);
						OnPointerUp((PointerUpEventArgs)ie);
					}
					else{
						//ie = new InputEvent((int)InputEvent.EventTypes.ClickUp, position, position, 0);
						ie = new PointerUpEventArgs(position);
						OnPointerUp((PointerUpEventArgs)ie);
						print ("pointer up");
					}
					dragging = false;
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
						
					//ie = new InputEvent((int)InputEvent.EventTypes.Zoom, meanpos, new Vector3(meanpos.x, meanpos.y), pdelta);
					ie = new ZoomEventArgs(meanpos, pdelta);
					OnZoom((ZoomEventArgs)ie);
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
		private bool dragging;
		protected override void Init ()
		{
			base.Init();
			print ("Inherited mouse driver!");
			lastevent = null;
			cutoff = 0f; // this might need tweaking for optimal experience
			dragforce = 0f;
			down = false;
			dragging = false;
		}

		protected override void UpdateEventState(){
			//if(lastevent != null)Debug.Log(lastevent.eventtype.ToString());
			//else Debug.Log("NULL");
			InputEventArgs ie = null;

			if(Input.GetMouseButton(0)){
				Vector3 position = Input.mousePosition;
				if(!down){
					elapsedtime = 0;
					//ie = new InputEvent((int)InputEvent.EventTypes.ClickDown, Input.mousePosition, Input.mousePosition, 0);
					ie = new PointerDownEventArgs(position);
					OnPointerDown((PointerDownEventArgs)ie);
					down = true;
				}
				else{

					if((lastevent.Position - position).magnitude > cutoff || dragging){
						dragging = true;
						dragforce = 1;//no idea if this is a good idea. DEPRECATED: smoothDrag((Input.mousePosition + lastevent.endpoint).magnitude);

						//ie = new InputEvent((int)InputEvent.EventTypes.Drag, lastevent.endpoint, Input.mousePosition, dragforce);
						ie = new DragEventArgs(lastevent.Position, position, dragforce);

						OnDrag((DragEventArgs)ie);
					}
					else{
						elapsedtime += Time.deltaTime;
						//ie = new InputEvent((int)InputEvent.EventTypes.Holding, Input.mousePosition, Input.mousePosition, 0);
						ie = new HoldingEventArgs(position, elapsedtime);
						OnHolding((HoldingEventArgs)ie);
					}

				}
			}
			else if(down){
				if(dragging){
					// end drag:
					ie = new DragEventArgs(lastevent.Position, Input.mousePosition, 1);
					OnDrag((DragEventArgs)ie);
					// mouseup:
					ie =  new PointerUpEventArgs(Input.mousePosition);
					OnPointerUp((PointerUpEventArgs)ie);
				}
				else{
					//ie = new InputEvent((int)InputEvent.EventTypes.ClickUp, Input.mousePosition, Input.mousePosition, 0);
					ie =  new PointerUpEventArgs(Input.mousePosition);
					OnPointerUp((PointerUpEventArgs)ie);
				}
				down = false;
				dragging = false;
			}
			else if(Input.GetAxis("Mouse ScrollWheel") != 0){
				//ie = new InputEvent((int)InputEvent.EventTypes.Zoom, Input.mousePosition, Input.mousePosition, Input.GetAxis("Mouse ScrollWheel"));
				ie = new ZoomEventArgs(Input.mousePosition, Input.GetAxis("Mouse ScrollWheel"));
				OnZoom((ZoomEventArgs)ie);
			}

			lastevent = ie;
		}

	}

}