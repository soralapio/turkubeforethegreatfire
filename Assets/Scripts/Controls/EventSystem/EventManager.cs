using UnityEngine;
using System;
using System.Collections.Generic;

namespace EventSystem{
	using Utils;
	public partial class EventManager : Singleton<EventManager> {
		/* This is the core of the centralized publisher-subscriber
		 * event system. This class won't use a string based invoke
		 * method, which would enable EventManager to remain more 
		 * independent of the events it encapsulates. Instead this 
		 * class will provide OnSomeEvent(object, eventargs) methods
		 * for every event. For this reason the class is implemented as
		 * partial: 
		 * 	adding a group of related events happens by adding a partial
		 *  class of EventManager in its own file, named accordingly.
		 * 
		 * the implementetation details are based on this article:
		 * http://www.codeproject.com/Articles/20550/C-Event-Implementation-Fundamentals-Best-Practices
		 * 
		 * THIS CLASS IS ALSO A SINGLETON. RECEIVE INSTANCE USING Instance GETTER!
		 */

		/*
		private static readonly EventManager instance = new EventManager();


		public static EventManager Instance{
			get{
				return instance;
			}
		}
		*/
	}
}
