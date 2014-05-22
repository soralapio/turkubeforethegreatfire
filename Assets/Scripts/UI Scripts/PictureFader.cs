using UnityEngine;
using System.Collections;

using EventSystem;

public class PictureFader : MonoBehaviour {
	public Texture2D closeButton;
	public Texture2D arrowLeft;
	public Texture2D arrowRight;
	private Rect closeButtonRect;

	private EventManager EM;
	private InputManager IM;

	private Texture2D bottomTexture;
	private Texture2D topTexture;
	private Rect bottomRect;
	private Rect topRect;

	private bool smoothFader = true;
	private bool enabled;

	private float sliderValue;

	private Color textureAlpha;

	private GUIManager gm;
	//private InputManager inputmanager;

	// Use this for initialization
	void Start () {
		IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		EM = EventManager.Instance;
		gm = GUIManager.Instance;

		smoothFader = true;
		//sharpFader = false;

		textureAlpha = Color.white;
	
		sliderValue = 0.0f;

		closeButtonRect = new Rect(Screen.width - closeButton.width, 0, closeButton.width, closeButton.height);

		EM.DisplayPictureFader += HandleDisplayPictureFader;
		//inputmanager = GameObject.Find ("InputManager").GetComponent ("InputManager") as InputManager;

		//Show();

	}

	void HandleDisplayPictureFader (object o, DisplayPictureFaderEventArgs e)
	{
		bottomTexture = e.PictureOne;
		topTexture = e.PictureTwo;
		bottomRect = new Rect (Screen.width / 2 - bottomTexture.width / 2, Screen.height / 2 - bottomTexture.height / 2, bottomTexture.width, bottomTexture.height);
		topRect = new Rect (Screen.width / 2 - topTexture.width / 2, Screen.height / 2 - topTexture.height / 2, topTexture.width, topTexture.height);
		Show ();
	}
	
	// Update is called once per frame
	void Update () {

		textureAlpha.a = sliderValue;
	
	}

	private void Show()
	{
		sliderValue = 0;
		gm.OverlayGUIFuncs += MapSlider;
		EM.OnEnterOverlay(this, new EnterOverlayEventArgs());
		enabled = true;
		IM.PointerDrag += HandlePointerDrag;

	}



	private void Hide()
	{
		gm.OverlayGUIFuncs -= MapSlider;
		enabled = false;
		EM.OnExitOverlay(this, new ExitOverlayEventArgs());
		IM.PointerDrag -= HandlePointerDrag;

	}

	void HandlePointerDrag (object o, DragEventArgs e)
	{
		sliderValue = Mathf.Clamp(sliderValue + e.Direction.x/30f, 0f, 1f);
	}

	private void MapSlider()
	{

		if (enabled) 
		{
			GUI.depth = 2;
			GUI.DrawTexture (bottomRect, bottomTexture);
			
			GUI.color = textureAlpha;
			
			GUI.DrawTexture (topRect, topTexture);

			GUI.depth = 1;
			GUI.color = Color.white;
			//sliderValue = GUI.HorizontalSlider (new Rect (Screen.width / 2 - 200, Screen.height - 50, 400, 50), sliderValue, 0f, 1f);
			GUI.DrawTexture(new Rect(bottomRect.xMin*(1-sliderValue) + topRect.xMin*(sliderValue) - arrowLeft.width, (Screen.height-arrowLeft.height)/2, arrowLeft.width, arrowLeft.height),arrowLeft);
			GUI.DrawTexture(new Rect(bottomRect.xMax*(1-sliderValue) + topRect.xMax*(sliderValue), (Screen.height-arrowRight.height)/2, arrowRight.width, arrowRight.height),arrowRight);
			//GUI.Label (new Rect (50, 50, 50, 50), "Value: " + sliderValue);





			if(GUI.Button(closeButtonRect, closeButton, "Label"))Hide();


		}
	}
}
