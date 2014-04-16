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


	public class SetMapIconsVisibilityEventArgs : System.EventArgs{
		private string _type;
		private bool _vis;

		public SetMapIconsVisibilityEventArgs(string icontype, bool visibility){
			_type = icontype;
			_vis = visibility;
		}

		public string Type{
			get { return _type; }
		}

		public bool Visibility{
			get { return _vis; }
		}

		public static SetMapIconsVisibilityEventArgs All(bool visibility){
			 return new SetMapIconsVisibilityEventArgs("all", visibility); 
		}

		public static SetMapIconsVisibilityEventArgs Scene(bool visibility){
			return new SetMapIconsVisibilityEventArgs("scene", visibility); 
		}

		public static SetMapIconsVisibilityEventArgs Text(bool visibility){
			return new SetMapIconsVisibilityEventArgs("text", visibility); 
		}
	}

	public partial class EventManager{
		public event EventHandler<SetMapIconsVisibilityEventArgs> SetMapIconsVisibility;

		public void OnSetMapIconsVisibility(object o, SetMapIconsVisibilityEventArgs e){
			if(SetMapIconsVisibility != null) SetMapIconsVisibility(o,e);
		}

	}

}
