using UnityEngine;
using System.Collections;

using Utils;
public class LogHandler : Singleton<MonoBehaviour> {
	private static bool displayerrors;
	private static bool shortform;

	private static string errorstream;

	public bool DEBUG;

	// Use this for initialization
	void Awake () {
		GameObject.DontDestroyOnLoad(this);
		Application.RegisterLogCallback(logHandler);

		displayerrors = false;
		shortform = true;

		errorstream = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void logHandler(string log, string trace, LogType type){
		//if (log.Substring(0,1)!="%")return;
		//log = log.Substring(1);
		//if(!displayerrors) return;
		errorstream =  log + "\n" + (shortform?"n/a":("STACK: "+trace)) + "---\n\n" + errorstream;
	}

	void OnGUI(){
		if(!DEBUG) return;
		GUILayout.BeginArea(new Rect(0,0,300,Screen.height));

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Errors [" + (displayerrors?"true":"false") + "]")) displayerrors = !displayerrors;
		if(GUILayout.Button("Verbose [" + (shortform?"false":"true") + "]")) shortform = !shortform;
		GUILayout.EndHorizontal();
		GUILayout.Space(20);
		if(displayerrors)GUILayout.Box(errorstream, GUILayout.Height(Screen.height));

		GUILayout.EndArea();
	}
}
