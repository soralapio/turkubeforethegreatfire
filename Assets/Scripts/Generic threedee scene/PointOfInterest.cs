using UnityEngine;
using System.Collections;

using EventSystem;
public class PointOfInterest : MonoBehaviour {
	public Object objectToShow;
	public string description;
	public Vector2 placeAdjustmentVector;

	public Transform cameraoffset;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowInterest(){
		if(objectToShow == null) return;
		EventManager.Instance.OnViewHistoricalObject(this, new ViewHistoricalObjectEventArgs(objectToShow, description, (Vector3)placeAdjustmentVector));
	}
}
