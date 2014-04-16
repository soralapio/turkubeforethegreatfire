using UnityEngine;
using System;
using System.Collections;

namespace EventSystem{

	// these events are stubs for now.
	public class EnterOverlayEventArgs : System.EventArgs {
		public EnterOverlayEventArgs(){
		}
	}

	public class ExitOverlayEventArgs : System.EventArgs {
		public ExitOverlayEventArgs(){

		}
	}
	

	public partial class EventManager{
		public event EventHandler<EnterOverlayEventArgs> EnterOverlay;
		public event EventHandler<ExitOverlayEventArgs> ExitOverlay;

		public void OnEnterOverlay(object o, EnterOverlayEventArgs e){
			// this is vurnerable to exceptions thrown in the handler methods
			if(EnterOverlay != null){
				EnterOverlay(o, e);
			}
		}

		public void OnExitOverlay(object o, ExitOverlayEventArgs e){
			// this is vurnerable to exceptions thrown in the handler methods
			if(ExitOverlay != null){
				ExitOverlay(o, e);
			}
		}

		//debug:
		public void CountOnEnters(){
			foreach(Delegate d in EnterOverlay.GetInvocationList()){
				Debug.Log(d.Method.GetHashCode().ToString());
			}
		}
	}
}
