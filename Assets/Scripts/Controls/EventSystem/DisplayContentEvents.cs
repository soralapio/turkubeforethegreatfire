using UnityEngine;
using System.Collections;

using Utils;
namespace EventSystem{
	public class DisplayLetterEventArgs : System.EventArgs{
		private Story _story;
		private string _date;

		public DisplayLetterEventArgs(Story letter, string date){
			_story = letter;
			_date = date;
		}

		public Story Letter{
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