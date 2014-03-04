using UnityEngine;
using System.Collections;

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

	public string[] studentText;

	private bool readyToShow;

	private int textPageToShow;

	private float movet;
	private bool moving;

	private Rect studentr;
	private Rect textr;

	private GUIManager gm;
	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		//w618 h893
		// prepare student rect
		studentr = new Rect(0,0,618,893);
		print (studentr.ToString());
		//studentr = PH.ScaleToRatio(studentr);

		// prepare speech rect
		textr = new Rect(studentr.width + 10, Screen.height / 2 -200, 400, 550);
		textr = PH.ScaleToRatio(textr);
		// this is scaled after textr because textr depends on the original position of studentr
		studentr = PH.ScaleToRatio(studentr);
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


	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			anim();
		}
	}

	void OnGUI(){

		GUI.TextArea(new Rect(textcurrent.x, textcurrent.y, textr.width, textr.height), studentText[textPageToShow], speakstyle);
		GUI.Label(new Rect( studentcurrent.x, studentcurrent.y, studentr.width, studentr.height), studenttexture);


		if (readyToShow) {
			if (textPageToShow != 0) {

					if (GUI.Button (new Rect (textcurrent.x - previousButton.width / 2, textcurrent.y + textr.height + 25, previousButton.width, previousButton.height), previousButton)) {

							textPageToShow--;

					}

			}

			if (textPageToShow != studentText.Length - 1) {

					if (GUI.Button (new Rect (textcurrent.x - previousButton.width / 2 + textr.width + 25, textcurrent.y + textr.height + 25, previousButton.width, previousButton.height), nextButton)) {
	
							textPageToShow++;
	
					}

			}

			if (textPageToShow == studentText.Length - 1) {
					
					if (GUI.Button (new Rect (textcurrent.x - previousButton.width / 2 + textr.width + 25, textcurrent.y + textr.height + 25, previousButton.width, previousButton.height), closeButton)) {
					textPageToShow = 0;
							Hide ();
	
					}
			
						}
				}

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
			gm.ExitOverlay();
		}
	}

}
