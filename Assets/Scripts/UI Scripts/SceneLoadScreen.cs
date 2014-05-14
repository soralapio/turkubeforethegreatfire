using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using EventSystem;
public class SceneLoadScreen : MonoBehaviour {
	public GUIStyle backStyle;
	public GUIStyle textStyle;
	// this is a stupid pairing: need to make some exposable pairing class for speed and clearness.
	public string[] sceneNames;
	public Texture2D[] loadingPictures;
	public string[] loadingTexts;
	public Texture2D defaultPicture;

	public float loadTime;


	EventManager EM;
	InputManager IM;
	

	bool display;
	float loadStarted;

	Texture2D loadPicture;
	string loadText;
	Rect loadPictureRect;
	// Use this for initialization
	void Start () {
		EM = EventManager.Instance;
		EM.SceneSwap += HandleSceneSwap;

		display = false;
		loadStarted = 0;

		GameObject.DontDestroyOnLoad(this);
		// one possibility for scaling texture:
		// wiki.unity3d.com/index.php/TextureScale
		// or implement using 2D/3D objects instead of textures
	}

	void HandleSceneSwap (object o, SceneSwapEventArgs e)
	{
		int li = GetLoadingIndex(e.SceneName);
		loadPicture = li==-1?defaultPicture:loadingPictures[li];
		loadText = li==-1?"Ladataan...":loadingTexts[li];
		loadPictureRect = new Rect((Screen.width - loadPicture.width)/2, 0,loadPicture.width, loadPicture.height);
		print (loadPictureRect);
		display = true;
		loadStarted = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void OnGUI() {
		if(!display) return;
		GUI.depth = 3;
		GUI.Label(new Rect(0,0, Screen.width, Screen.height), "", backStyle);
		GUI.depth = 2;
		GUI.DrawTexture(loadPictureRect, loadPicture);
		GUI.depth = 1;
		GUI.Label(new Rect((Screen.width-800)/2, Screen.height-275, 800, 250), loadText, textStyle);
	}

	void OnLevelWasLoaded(int level){
		if(!display) return;
		IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
		IM.PointerUp += HandlePointerUp;
		StartCoroutine("DestroyAfterLoad");
	}

	void HandlePointerUp (object o, PointerUpEventArgs e)
	{
		print ("premature destruction");
		StopCoroutine("DestroyAfterLoad");
		Destroy(this);
	}

	IEnumerator DestroyAfterLoad(){
		float waits = loadTime + Time.realtimeSinceStartup - loadStarted;
		if(waits <= 0) Destroy(this); // if already loaded more than wait time, destroy
		yield return new WaitForSeconds(waits);
		Destroy(this);
	}

	int GetLoadingIndex(string name){
		int num = -1;
		if(sceneNames == null || sceneNames.Length == 0) return num;
		for(int i = 0; i < sceneNames.Length; i++){
			if(sceneNames[i] == name){
				num = i;
				break;
			}
		}
		return num;
	}

	void OnDestroy(){
		if(IM != null) IM.PointerUp -= HandlePointerUp;
	}
}
