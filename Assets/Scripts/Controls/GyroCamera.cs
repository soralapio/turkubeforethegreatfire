using UnityEngine;
using System.Collections;

/*
* This file contains some reference implementation regarding gyro. Not in use.
*/

public class GyroCamera : MonoBehaviour {

	private bool gyroReady = false;
	private static Quaternion offset;



	void Start() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		ReadyGyro();
	}

	void Update() {
		if (gyroReady) {
			Quaternion q = ConvertRotation(/*offset */ Input.gyro.attitude);
			transform.rotation = q;

		}
	}

	void OnGUI() {
		//GUI.Label(new Rect(10, 10, 200, 20), "Acc: " + (Input.acceleration*360.0f));
		GUI.Label(new Rect(10, 10, 200, 20), "Gyro: " + ConvertRotation(Input.gyro.attitude).eulerAngles);
		//GUI.Label(new Rect(10, 70, 200, 20), "Offs: " + gyroEuler);
		GUI.Label(new Rect(10, 40, 200, 20), "Gravity: " + Input.gyro.gravity);
		//GUI.Label(new Rect(10, 70, 200, 20), "Loc. Serv.: " + (Input.location != null));
		GUI.Label(new Rect(10, 70, 200, 20), "Compass: " + Input.compass.trueHeading);
	}



	private void ReadyGyro() {
		if (SystemInfo.supportsGyroscope) {


			// Start the location services and the compass
			Input.location.Start();
			Input.compass.enabled = true;
			Input.gyro.enabled = true;
			// Rotate the parent transform so that virtual north corresponds with real north
			//transform.parent.rotation = transform.parent.rotation * Quaternion.AngleAxis(Input.compass.trueHeading, Vector3.up);

			// Calculate the rotation offset
			offset = Quaternion.Inverse(Quaternion.Euler(90, 0, 0)) * Quaternion.Inverse(Input.gyro.attitude) * Quaternion.AngleAxis(Input.compass.trueHeading, Vector3.up);
			print (offset);
			// Finally, set the gyroReady status to true
			gyroReady = true;

		} else {
			Debug.Log("Gyroscope not supported!");
		}
	}

	// Magic
	private static Quaternion ConvertRotation(Quaternion q) {
    	return new Quaternion(q.x, q.y, -q.z, -q.w);
	}

}
