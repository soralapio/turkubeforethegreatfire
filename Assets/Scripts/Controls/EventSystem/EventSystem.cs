using UnityEngine;
using System.Collections;

namespace EventSystem{

	// use this delegate as a base for all events. much convenient. such power.
	public delegate void EventHandler<TEventArgs>(object o, TEventArgs e) where TEventArgs : System.EventArgs;

}