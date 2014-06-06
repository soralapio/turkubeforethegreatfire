using UnityEngine;
using System.Collections;

namespace CustomGUI{
	/*
	* This script is used in 3D scenes on the secondary camera to view historical objects (needs to be added to the scene manually as a prefab).
	*
	*/ 

	using InputEventSystem;
	using EventSystem;
	public class ObjectViewerController : MonoBehaviour {
		public Object pagedTextPrefab;
		public GUIStyle edgeStyleForShowbox;
		public Texture2D closeButton;
		// these values are in percents of the screen
		public Rect objectViewArea;
		public Rect textboxViewArea;
		// where to put the object in question
		public Transform placement;

		GUIManager GM;
		InputManager IM;
		EventManager EM;

		PagedTextbox textbox;
		Rect showboxEdge;
		GameObject objectToShow;

		Rect closeButtonRect;

		void Awake () {
			IM = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
			GM = GUIManager.Instance;
			EM = EventManager.Instance;

			camera.enabled = false;
			camera.rect = objectViewArea;

			// this box goes to GUIManager, who will render it behind the camera render, creating borders.
			showboxEdge = new Rect(objectViewArea.x * Screen.width - edgeStyleForShowbox.border.left,
			                       objectViewArea.y * Screen.height - edgeStyleForShowbox.border.top,
			                       objectViewArea.width * Screen.width + edgeStyleForShowbox.border.right*2,
			                       objectViewArea.height * Screen.height + edgeStyleForShowbox.border.bottom*2);

			textbox = ((GameObject)Instantiate(pagedTextPrefab)).GetComponent<PagedTextbox>();
			textbox.SetPosition(Screen.width*textboxViewArea.x, Screen.height*textboxViewArea.y);
			// textbox is a4 shaped now: setting x wont matter
			textbox.SetSize(Screen.height*textboxViewArea.height / Mathf.Sqrt(2), Screen.height*textboxViewArea.height);
			textbox.Show = false;

			closeButtonRect = new Rect(Screen.width - closeButton.width, 0, closeButton.width, closeButton.height);

			EM.ViewHistoricalObject += HandleViewHistoricalObject;
			EM.HideHistoricalObject += HandleHideHistoricalObject;
		}

		void HandleViewHistoricalObject (object o, ViewHistoricalObjectEventArgs e)
		{
			Vector3 tempv = new Vector3(0, e.PlaceAdjustmentVector.y, e.PlaceAdjustmentVector.x);
			objectToShow = GameObject.Instantiate(e.ViewableObject, placement.position + tempv, ((GameObject)e.ViewableObject).transform.rotation) as GameObject;
			textbox.SetText(e.Description);
			Show();
		}

		void HandleHideHistoricalObject (object o, HideHistoricalObjectEventArgs e)
		{
			Hide();
		}

		void RegisterInputHandlers(){
			IM.PointerDrag += HandlePointerDrag;
			// could add zoomhandler here too, if that seems cool.
		}

		void UnRegisterInputHandlers(){
			IM.PointerDrag -= HandlePointerDrag;
		}

		void HandlePointerDrag (object o, DragEventArgs e)
		{
			if(objectToShow == null) return;
			objectToShow.transform.Rotate(Vector3.forward, -e.Direction.x *3);
		}


		void Show(){
			EM.OnEnterOverlay(this, new EnterOverlayEventArgs());
			textbox.Show = true;
			camera.enabled = true;
			RegisterInputHandlers();
			GM.OverlayGUIFuncs += textbox.GetUIDrawDelegate();
			GM.OverlayGUIFuncs += MyDrawDelegate;
		}

		void Hide(){
			textbox.Show = false;
			camera.enabled = false;
			textbox.SetText("");
			if(objectToShow != null) Destroy(objectToShow);
			UnRegisterInputHandlers();
			GM.OverlayGUIFuncs -= textbox.GetUIDrawDelegate();
			GM.OverlayGUIFuncs -= MyDrawDelegate;
			EM.OnExitOverlay(this, new ExitOverlayEventArgs());
		}

		void MyDrawDelegate(){
			GUI.Label(showboxEdge, "", edgeStyleForShowbox);

			if (GUI.Button (closeButtonRect, closeButton, "Label")) {
				Hide();
				
			}
		}
		// Update is called once per frame
		void Update () {
		
		}
	}
}
