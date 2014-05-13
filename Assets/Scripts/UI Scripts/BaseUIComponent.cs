using UnityEngine;
using System.Collections;

namespace CustomGUI{
	/* For inheritance only!
	 * Defines some interface functions that are
	 * sometimes required from UI component in our
	 * system by UIManager or some other party.
	 * 
	 * It is not necessary to inherit this but it 
	 * is handy.
	 * 
	 * I am not prepared to say what is the future of 
	 * this class once NGUI or something kicks in.
	 */
	public abstract class BaseUIComponent : MonoBehaviour {
		
		/* Returns the delegate function that is used to draw the component.
		 * UIManager uses these to render GUI in one place ( with Unity's internal
		 *  ui system). Might get flushed when real stuff kicks in.
		 */
		public abstract UIDrawDelegate GetUIDrawDelegate();

		/* Used to check whether a point is inside the various GUI elements
		 * the component might be composed of. Mostly useful for tracking if
		 * user clicked out-side of any known components; The UI system is very
		 * ad-hoc and isn't all too self-aware, so that is why.
		 */
		public abstract bool CointainsPoint(Vector2 point);

		/* These two are just generic position and size changes
		 * in case these need supporting. Position is probably
		 * more often used.
		 */
		public abstract void SetSize(float width, float height);

		public abstract void SetPosition(float x, float y);

		// this part will maybe have size and position getters, if those are needed.
	}
}