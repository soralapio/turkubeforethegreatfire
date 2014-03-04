using UnityEngine;
using System.Collections;

public class NewsPaperShower : MonoBehaviour {

	public Texture2D[] paperPages;
	public Texture2D backButton;
	public Texture2D forwardButton;
	public Texture2D closeButton;

	private int pageToShow;

	private bool showingPaper;

	private GUIManager gm;

	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		pageToShow = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.P)) 
		{

			StartShowing();

		}
		*/
	
	}

	public void StartShowing()
	{
		gm.EnterOverlay();
		showingPaper = true;

	}

	void OnGUI()
	{

		if (showingPaper) 
		{

			GUI.DrawTexture(new Rect(Screen.width / 2 - paperPages[pageToShow].width /2, Screen.height / 2 - paperPages[pageToShow].height / 2, paperPages[pageToShow].width,
			                         paperPages[pageToShow].height), paperPages[pageToShow]);

			if (pageToShow != 0)
			{
				//Debug.Log ("Showing back button");

				if (GUI.Button (new Rect(Screen.width / 2 - paperPages[pageToShow].width / 2 - backButton.width - 25, Screen.height - backButton.height - 50,
				                backButton.width, backButton.height), backButton))
				{

					pageToShow -= 1;

				}

			}

			if (pageToShow != paperPages.Length - 1)
			{

				//Debug.Log ("Showing forward button");
				
				if (GUI.Button (new Rect(Screen.width / 2 + paperPages[pageToShow].width / 2 + 25, Screen.height - backButton.height - 50,
				                backButton.width, backButton.height), forwardButton))
				{
					
					pageToShow += 1;
					
				}
				
			}

			if (pageToShow == paperPages.Length - 1)
			{

				//Debug.Log ("Showing close button");
				
				if (GUI.Button (new Rect(Screen.width / 2 + paperPages[pageToShow].width / 2 + 25, Screen.height - backButton.height - 50,
				                         backButton.width, backButton.height), closeButton))
				{
					
					showingPaper = false;
					pageToShow = 0;
					gm.ExitOverlay();
				}
				
			}

		}


	}
}
