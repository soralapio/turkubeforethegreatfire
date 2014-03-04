using UnityEngine;
using System.Collections;

public class MouseCameraRotator : MonoBehaviour {
	private Vector3 last;
	// Use this for initialization
	void Start () {
		last = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moff = last - Input.mousePosition;
		last = Input.mousePosition;

		Quaternion q = transform.rotation;
		print(q.eulerAngles.ToString());
		transform.Rotate(Vector3.right, moff.y/100);
	}
}
