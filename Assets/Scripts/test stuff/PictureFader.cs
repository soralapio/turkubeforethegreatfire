using UnityEngine;
using System.Collections;

public class PictureFader : MonoBehaviour {

	public Texture2D bottomTexture;
	public Texture2D topTexture;

	private bool smoothFader = true;
	private bool sharpFader;

	private float sliderValue;

	private Color textureAlpha;

	// Use this for initialization
	void Start () {

		smoothFader = true;
		sharpFader = false;

		textureAlpha = Color.white;
	
		sliderValue = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

		textureAlpha.a = sliderValue;
	
	}

	void OnGUI()
	{

		sliderValue = GUI.HorizontalSlider (new Rect (Screen.width / 2 - 200, Screen.height - 50, 400, 50), sliderValue, 0f, 1f);
		
		GUI.Label(new Rect(50, 50, 50, 50), "Value: " + sliderValue);

		if (smoothFader)
		GUI.Label(new Rect(50, 100, 100, 50), "Smooth fader");

	    if (sharpFader)
		GUI.Label(new Rect(50, 100, 100, 50), "Sharp fader");

		if (GUI.Button (new Rect (50, 150, 100, 100), "Smooth fading")) 
		{
			smoothFader = true;
			sharpFader = false;
		}

		if (GUI.Button (new Rect (50, 300, 100, 100), "Sharp fading")) 
		{
			sharpFader = true;
			smoothFader = false;
		}
		if (smoothFader) 
		{
				GUI.DrawTexture (new Rect (Screen.width / 2 - bottomTexture.width / 2, Screen.height / 2 - bottomTexture.height / 2, bottomTexture.width,
				 bottomTexture.height), bottomTexture);

				GUI.color = textureAlpha;

				GUI.DrawTexture (new Rect (Screen.width / 2 - topTexture.width / 2, Screen.height / 2 - topTexture.height / 2, topTexture.width,
   				topTexture.height), topTexture);

				GUI.color = Color.white;
		} 

		else if (sharpFader) 
		{

			if (sliderValue > 0.5f)
			{

				GUI.DrawTexture (new Rect (Screen.width / 2 - topTexture.width / 2, Screen.height / 2 - topTexture.height / 2, topTexture.width,
				                           topTexture.height), topTexture);

			}

			else
				GUI.DrawTexture (new Rect (Screen.width / 2 - bottomTexture.width / 2, Screen.height / 2 - bottomTexture.height / 2, bottomTexture.width,
				                           bottomTexture.height), bottomTexture);

		}

		

	}
}
