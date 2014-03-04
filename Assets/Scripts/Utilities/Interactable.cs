using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour{

	public float activationdistance;

	protected bool VerifyDistance(){
		// see if the camera is close enough for activating GetClicked. Needs to be used by the subclass
		// if such a functionality is required
		return ((transform.position - Camera.main.transform.position).magnitude < activationdistance);
	}

	public abstract void GetClicked();

}
