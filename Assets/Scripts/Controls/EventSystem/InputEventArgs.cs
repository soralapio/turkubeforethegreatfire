using UnityEngine;
using System;
using System.Collections;

namespace EventSystem{
	/*
	 * The naming of this file is a bit different than for the other files
	 * that contain EventArgs. This is because InputEventSystem is outside
	 * EventSystem. EventSystem uses a centralized manager and the input 
	 * events are so frequent, that they were taken out of the general 
	 * system.
	 */


	public class InputEventArgs : System.EventArgs{
		protected Vector3 _pos;
		public Vector3 Position{
			get { return _pos; }
		}
	}

	public class PointerDownEventArgs : InputEventArgs{

		public PointerDownEventArgs(Vector3 position){
			_pos = position;
		}
	}

	public class PointerUpEventArgs : InputEventArgs{

		public PointerUpEventArgs(Vector3 position){
			_pos = position;
		}
		
	}

	public class HoldingEventArgs : InputEventArgs{
		private float _holdtime;

		public HoldingEventArgs(Vector3 position, float elapsedtime){
			_pos = position;
			_holdtime = elapsedtime;
		}

		public float ElapsedTime{
			get { return _holdtime; }
		}

		
	}

	public class DragEventArgs : InputEventArgs{
		private Vector3 _startpos;
		private Vector3 _direction;
		private float _force; // this might not be  useful;

		public DragEventArgs(Vector3 startposition, Vector3 endposition, float force = 0){
			_startpos = startposition;
			_pos = endposition;
			_force = force;
			_direction = (endposition-startposition).normalized;
		}

		public Vector3 StartPosition{
			get { return _startpos; }
		}

		public Vector3 EndPosition{
			get { return _pos; }
		}

		public float Force{
			get { return _force; }
		}
		
		public Vector3 Direction{
			get { return _direction; }
		}
		
	} 

	public class ZoomEventArgs : InputEventArgs{
		private float _force;

		public ZoomEventArgs(Vector3 position, float force){
			_pos = position;
			_force = force;
		}


		public float Force{
			get { return _force; }
		}
	} 
}