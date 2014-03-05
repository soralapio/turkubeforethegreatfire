using UnityEngine;
using System.Collections;

public class PictureFader : MonoBehaviour {

	public Texture2D bottomTexture;
	public Texture2D topTexture;

	private bool smoothFader = true;
	private bool enabled;

	private float sliderValue;

	private Color textureAlpha;

	private GUIManager gm;
	//private InputManager inputmanager;

	// Use this for initialization
	void Start () {

		smoothFader = true;
		//sharpFader = false;

		textureAlpha = Color.white;
	
		sliderValue = 1.0f;

		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		//inputmanager = GameObject.Find ("InputManager").GetComponent ("InputManager") as InputManager;
		gm.OverlayGUIFuncs += MapSlider;


	}
	
	// Update is called once per frame
	void Update () {

		textureAlpha.a = sliderValue;
	
	}

	public void Show()
	{
		gm.EnterOverlay ();
		enabled = true;

	}

	public void Hide()
	{

		enabled = false;
		gm.ExitOverlay ();

	}

	private void MapSlider()
	{

		if (enabled) 
		{

				sliderValue = GUI.HorizontalSlider (new Rect (Screen.width / 2 - 200, Screen.height - 50, 400, 50), sliderValue, 0f, 1f);

				GUI.Label (new Rect (50, 50, 50, 50), "Value: " + sliderValue);


				GUI.DrawTexture (new Rect (Screen.width / 2 - bottomTexture.width / 2, Screen.height / 2 - bottomTexture.height / 2, bottomTexture.width,
			bottomTexture.height), bottomTexture);

				GUI.color = textureAlpha;

				GUI.DrawTexture (new Rect (Screen.width / 2 - topTexture.width / 2, Screen.height / 2 - topTexture.height / 2, topTexture.width,
			topTexture.height), topTexture);

				GUI.color = Color.white;
		}
	}
}
