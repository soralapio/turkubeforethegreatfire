using UnityEngine;
using System.Collections;



namespace CustomGUI{

	using EventSystem;
	using PH = Utils.PlatformHelper;

	public class StudentController : MonoBehaviour {


		public Texture studenttexture;
		public Texture2D closeButton;
		public Object textboxPrefab;

		private delegate void Mover();
		private Mover anim;

		private Vector3 studenthide;
		private Vector3 studentshow;
		private Vector3 studentcurrent;

		private Vector3 texthide;
		private Vector3 textshow;
		private Vector3 textcurrent;

		private bool readyToShow;

		private float movet;
		private bool moving;

		private Rect studentr; // the picture for student 
		private Rect closeButtonRect;

		private float textboxheightp = 0.7f;
		private float textboxwidthofheightp;

		private EventManager EM;
		private GUIManager gm;

		private PagedTextbox textbox;
		// Use this for initialization
		void Start () {
			EM = EventManager.Instance;
			gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();

			textbox = ((GameObject)Instantiate(textboxPrefab)).GetComponent<PagedTextbox>();
			textboxwidthofheightp =textboxheightp/Mathf.Sqrt(2);

			studentr = new Rect(0,0,studenttexture.width,studenttexture.height);
			studentr = PH.ScaleToRatio(studentr);

			float textx = studentr.width + 10;
			float texty = Screen.height * (1-textboxheightp)/2;
			textbox.SetSize(Screen.height*textboxwidthofheightp, Screen.height*textboxheightp);
			textbox.SetPosition(textx, texty);

			studentshow = new Vector3(0, Screen.height - studentr.height);
			studenthide = new Vector3(-studentr.width, studentshow.y);
			studentcurrent = studenthide;

			textshow = new Vector3(textx, texty);
			texthide = new Vector3(textx, Screen.height);
			textcurrent = texthide;

			closeButtonRect = new Rect(Screen.width - closeButton.width, 0, closeButton.width, closeButton.height);

			movet = 0;
			moving = false;

			EM.DisplayLetter += HandleDisplayLetter;

			gm.OverlayGUIFuncs += DrawStudent;
			gm.OverlayGUIFuncs += textbox.GetUIDrawDelegate();

		}

		void HandleDisplayLetter (object o, DisplayLetterEventArgs e)
		{
			if(moving)return;
			textbox.SetText(e.Letter);
			moving = true;
			movet = 0;
			anim = ShowComponents;
		}
		
		// Update is called once per frame
		void Update () {
			if(moving){
				anim();

				// apply position updates:
				textbox.SetPosition(textcurrent.x, textcurrent.y);
				studentr.Set(studentcurrent.x, studentcurrent.y, studentr.width, studentr.height);
			}
		}


		private void DrawStudent(){
			GUI.Label(new Rect( studentcurrent.x, studentcurrent.y, studentr.width, studentr.height), studenttexture);

			if(!readyToShow) return;

			if (GUI.Button (closeButtonRect, closeButton, "Label")) {
				Hide();
				
			}
		}

		private void CloseText(){ 
			Hide ();
		}

		// this could be wrapped  into something :))))
		private void ShowComponents(){
			if(studentcurrent != studentshow){
				movet += Time.deltaTime*3;
				studentcurrent = Vector3.Lerp(studenthide, studentshow, movet);
				textcurrent = Vector3.Lerp(texthide, textshow, movet);

			}
			else{
				moving = false;
				readyToShow = true;
			}
		}

		public void Hide(){

			if(moving) return;
			readyToShow = false;
			moving = true;
			movet = 0;
			anim = HideComponents;
		}
		private void HideComponents(){
			if(studentcurrent != studenthide){
				movet += Time.deltaTime*3;
				studentcurrent = Vector3.Lerp(studentshow, studenthide, movet);
				textcurrent = Vector3.Lerp(textshow, texthide, movet);
			}
			else{
				moving = false;
				EM.OnExitOverlay(this, new ExitOverlayEventArgs());
				EM.CountOnEnters();
			}
		}

	}
}
