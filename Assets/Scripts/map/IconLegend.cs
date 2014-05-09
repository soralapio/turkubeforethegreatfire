using UnityEngine;
using System.Collections;

using EventSystem;
public class IconLegend : MonoBehaviour {
	public string name;
	private Vector3 pos;
	
	private Vector2 tw; // legend text width
	private GUIManager GM;
	private EventManager EM;
	private string type;

	private Rect renderbounds;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		GM = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
		pos = Vector3.zero;
		print (transform.localScale.ToString());

		tw = GM.legendstyle.CalcSize(new GUIContent(name));
		tw += new Vector2(10, 0);
		GM.NormalGUIFuncs += RenderNames;

		if(gameObject.GetComponent<SceneIcon>() != null) type = "scene";
		else type = "text";

		EM.SetMapIconsVisibility += HandleSetMapIconsVisibility;

		renderbounds = gameObject.GetComponent<SpriteRenderer>().sprite.textureRect;
		//print("lol " + renderbounds.ToString());
	}

	void HandleSetMapIconsVisibility (object o, SetMapIconsVisibilityEventArgs e)
	{
		if(e.Type == type){
			if(e.Visibility) GM.NormalGUIFuncs += RenderNames;
			else GM.NormalGUIFuncs -= RenderNames;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		pos = Camera.main.WorldToScreenPoint(transform.position);// - new Vector3(0,0,renderbounds.height / 2f));
	}
	

	void RenderNames(){
		GUI.Label(new Rect(pos.x-tw.x/2,Screen.height - pos.y ,tw.x,tw.y), name, GM.legendstyle);
	}
}
