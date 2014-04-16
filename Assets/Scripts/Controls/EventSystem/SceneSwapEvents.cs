using UnityEngine;
using System;
using System.Collections;

namespace EventSystem{
	// custom event argument for the SceneSwap.
	// always inherit System.EventArgs.
	public class SceneSwapEventArgs : System.EventArgs {
		private string _sceneName;
		public SceneSwapEventArgs(string scenename){
			this._sceneName = scenename;
		}
		
		public string SceneName{
			get { return _sceneName; }
		}
	}


	// SceneSwap part of the event manager.
	public partial class EventManager{
		// event handler for SceneSwap-event
		public event EventHandler<SceneSwapEventArgs> SceneSwap;

		// call when this event happens
		public void OnSceneSwap(object o, SceneSwapEventArgs e){
			// this is vurnerable to exceptions thrown in the handler methods
			if(SceneSwap != null){
				SceneSwap(o, e);
			}
		}
	}



}
