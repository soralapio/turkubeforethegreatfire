using UnityEngine;
using System.Collections;

using EventSystem;
using PH = Utils.PlatformHelper;

public class StudentController: MonoBehaviour {
	private delegate void Mover();
	private Mover anim;

	public Texture studenttexture;
	public GUIStyle speakstyle;

	private Vector3 studenthide;
	private Vector3 studentshow;
	private Vector3 studentcurrent;

	private Vector3 texthide;
	private Vector3 textshow;
	private Vector3 textcurrent;

	public Texture2D previousButton;
	public Texture2D nextButton;
	public Texture2D closeButton;

	private string[] studentText;

	private bool readyToShow;

	private int textPageToShow;

	private float movet;
	private bool moving;

	private Rect studentr;
	private Rect textr;
	private float textboxheightp = 0.7f;
	private float textboxwidthofheightp;

	private EventManager EM;

	private GUIManager gm;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		//w618 h893
		// prepare student rect
		studentr = new Rect(0,0,studenttexture.width,studenttexture.height);
		print (studentr.ToString());
		//studentr = PH.ScaleToRatio(studentr);

		// prepare speech rect
		//textr = PH.ScaleToRatio(textr);
		// this is scaled after textr because textr depends on the original position of studentr
		studentr = PH.ScaleToRatio(studentr);
		textboxwidthofheightp =textboxheightp/Mathf.Sqrt(2);
		textr = new Rect(studentr.width + 10 , Screen.height * (1-textboxheightp)/2,  Screen.height*textboxwidthofheightp ,Screen.height*textboxheightp);
		print (studentr.ToString());
		

		studentshow = new Vector3(0, Screen.height - studentr.height);
		studenthide = new Vector3(-studentr.width, studentshow.y);
		studentcurrent = studenthide;

		textshow = new Vector3(textr.x, textr.y);
		texthide = new Vector3(textr.x, -textr.height);
		textcurrent = texthide;

		//text = "Teretulemast kaikki kanssaimmeiset! Oonki jo orottanu teit!";

		movet = 0;
		moving = false;

		EM.DisplayLetter += HandleDisplayLetter;

		gm.OverlayGUIFuncs += DrawStudent;
		

#if UNITY_STANDALONE
		speakstyle.fontSize = 27;
#elif UNITY_EDITOR
		speakstyle.fontSize = 22;
#else
		speakstyle.fontSize = 30;
#endif

	}

	void HandleDisplayLetter (object o, DisplayLetterEventArgs e)
	{
		if(moving)return;
		studentText = SplitIntoPages(e.Letter);
		moving = true;
		movet = 0;
		anim = ShowComponents;
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			anim();
		}
	}

	string[] SplitIntoPages (string rawtext)
	{
		float maxh = speakstyle.CalcHeight(new GUIContent(rawtext), textr.width - speakstyle.padding.left - speakstyle.padding.right);
		float fpages = maxh/(textr.height );
		int charsperpage = (int)(rawtext.Length/fpages);

		// this is a piece modified from the Story class.
		// this splitting might be better moved to some tools class.
		string[] pages = new string[(int)fpages + 1];
		for(int a = 0; a < pages.Length; a++)pages[a]="";

		string[] words = rawtext.Split(' ');

		int page = 0;
		for(int i = 0; i < words.Length; i++){
			if(pages[page].Length + words[i].Length > charsperpage && page < pages.Length -1) page++;
			pages[page] += words[i] + " ";

		}
		return pages;
	}

	private void DrawStudent(){
		// this function is passed as a delegate to GUIManager
		if(!readyToShow) return;
		GUI.Box(new Rect(textcurrent.x, textcurrent.y, textr.width, textr.height), studentText[textPageToShow], speakstyle);
		GUI.Label(new Rect( studentcurrent.x, studentcurrent.y, studentr.width, studentr.height), studenttexture);
		
		
		if (readyToShow) {
			if (textPageToShow != 0) {
				
				if (GUI.Button (new Rect (textcurrent.x + textr.width + 20, textcurrent.y , previousButton.width, previousButton.height), previousButton, "Label")) {
					
					textPageToShow--;
					
				}
				
			}
			
			if (textPageToShow != studentText.Length - 1) {
				
				if (GUI.Button (new Rect (textcurrent.x + textr.width + 20, textcurrent.y + (textr.height - previousButton.height) / 2 , previousButton.width, previousButton.height), nextButton, "Label")) {
					
					textPageToShow++;
					
				}
				
			}
			
			if (textPageToShow == studentText.Length - 1) {
				
				if (GUI.Button (new Rect (textcurrent.x + textr.width + 20, textcurrent.y + textr.height - previousButton.height, previousButton.width, previousButton.height), closeButton, "Label")) {
					CloseText();
					
				}
				
			}
		}
	}

	private void CloseText(){ 
		textPageToShow = 0;
		Hide ();
	}

	public void Show(Story story){
		if(moving)return;
		studentText = story.GetPages();
		moving = true;
		movet = 0;
		anim = ShowComponents;
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
		readyToShow = false;
		if(moving) return;
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
