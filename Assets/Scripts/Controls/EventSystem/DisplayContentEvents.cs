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


	public class DisplayPictureFaderEventArgs : System.EventArgs{
		private Texture2D _p1;
		private Texture2D _p2;

		public DisplayPictureFaderEventArgs(Texture2D picture1, Texture2D picture2){
			_p1 = picture1;
			_p2 = picture2;
		}

		public Texture2D PictureOne{
			get { return _p1; }
		}

		public Texture2D PictureTwo{
			get { return _p2; }
		}
	}

	public partial class EventManager{
		public event EventHandler<DisplayPictureFaderEventArgs> DisplayPictureFader;

		public void OnDisplayPictureFader(object o, DisplayPictureFaderEventArgs e){
			if(DisplayPictureFader != null) DisplayPictureFader(o, e);
		}
	}
}