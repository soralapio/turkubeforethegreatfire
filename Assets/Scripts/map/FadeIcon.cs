using UnityEngine;
using System.Collections;

using EventSystem;
public class FadeIcon : Icon {
	public Texture2D fadePictureNew;
	public Texture2D fadePictureOld;

	EventManager EM;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		EM.SetMapIconsVisibility += HandleSetMapIconsVisibility;
	}

	void HandleSetMapIconsVisibility (object o, SetMapIconsVisibilityEventArgs e)
	{
		if(e.Type == "text"){
			if(e.Visibility) Show();
			else Hide ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region implemented abstract members of Icon

	public override void Click ()
	{
		print ("lol");
		EM.OnDisplayPictureFader(this, new DisplayPictureFaderEventArgs(fadePictureNew, fadePictureOld));
	}

	#endregion
}
