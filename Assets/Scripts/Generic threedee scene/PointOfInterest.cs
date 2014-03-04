using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour {

	public Vector3 cameraoffset;
	// Use this for initialization
	void Start () {
		cameraoffset = transform.position + cameraoffset;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
