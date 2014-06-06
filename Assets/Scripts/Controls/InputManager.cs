using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Utils; // does this need to be here?
using InputEventSystem;

public class InputManager : 
// here, depending on our platform, inherit the right control driver. hack-ish, but works
#if UNITY_ANDROID
	TouchInputDriver
#elif UNITY_IPHONE
	TouchInputDriver
#elif UNITY_IOS
	TouchInputDriver
#elif UNITY_EDITOR
	MouseInputDriver
#else
	MouseInputDriver
#endif
{
	/* This class is the entrypoint for all "touching"-like
	 * input: mouse and touchscreen.
	 * 
	 * I have yet to figure out, if I need to implement some
	 * EventConsumed function to prevent weird behaviour with
	 * proper guielements
	 */
	
	void Awake(){
		Init();
	}
	
	void Update(){
		UpdateEventState();
		if(Input.GetKey(KeyCode.Escape)) Application.Quit();
	}
	
	
}
