using UnityEngine;
using System.Collections;

using Utils;
namespace EventSystem{
	public class DisplayLetterEventArgs : System.EventArgs{
		private string _story;
		private string _date;

		public DisplayLetterEventArgs(string letter, string date){
			_story = letter;
			_date = date;
		}

		public string Letter{
			get { return _story; }
		}

		public string Date{
			get { return _date; }
		}
	}


	public partial class EventManager{
		public event EventHandler<DisplayLetterEventArgs> DisplayLetter;

		public void OnDisplayLetter(object o, DisplayLetterEventArgs e){
			if( DisplayLetter != null) DisplayLetter(o, e);
		}
	}
}