using UnityEngine;
using System.Collections;

public class NiceGlowController : MonoBehaviour {
	public float maxviewdistance;
	public float minviewdistance;
	// Use this for initialization
	void Start () {
		particleSystem.enableEmission = false;
		particleSystem.Play();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 cam2glow = transform.position - Camera.main.transform.position;

		if(cam2glow.magnitude > maxviewdistance || cam2glow.magnitude < minviewdistance){
			particleSystem.enableEmission = false;
			return;
		}

		float dot = Vector3.Dot(cam2glow.normalized, Camera.main.transform.forward);

		if(dot < 0.93f){
			particleSystem.enableEmission = true;
		}
		else{
			particleSystem.enableEmission = false;
		} 
	}
}
