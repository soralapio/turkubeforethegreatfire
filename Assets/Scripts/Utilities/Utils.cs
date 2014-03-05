using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utils{
	// delegates


	// derivable for singleton Monobehaviour
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{
		protected static T instance;
		public static T Instance{
			get{
				if(instance == null)
				{
					instance = (T) FindObjectOfType(typeof(T));
					if (instance == null)
					{
						Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
					}
				}
				return instance;
			}
		}
	}

	public static class PlatformHelper{
		// could make a function for these that takes DPI into consideration
		public const float UISCALEMULTIPC = 1;
		public const float UISCALEMULTIMOBILE = 1.8f;

		public static float GetUIScaleMulti(){
			// use this function, if scaling is very, very trivial task
			// like map icons and single buttons
			#if UNITY_EDITOR
			return UISCALEMULTIPC;
			#elif UNITY_STANDALONE
			return UISCALEMULTIPC;
			#elif UNITY_ANDROID
			return UISCALEMULTIMOBILE;
			#elif UNITY_IOS
			return UISCALEMULTIMOBILE;
			#endif
		}


		public static Rect ScaleToRatio(Rect r, float designx = 1920, float designy = 1080){
			float x, y, width, height;
			float orat = designx / designy;
			float screenrat = Screen.width / Screen.height;
			float rectrat = r.width / r.height;
			if(screenrat <= orat){
				float xratio = Screen.width / designx;
				width = r.width * xratio;//r.width/designx * Screen.width;
				height = width / rectrat;
				x = r.x * xratio;
				y = r.y * xratio;
			}
			else{
				float yratio = Screen.height / designy;
				height = r.height * yratio;
				width = height * rectrat;
				x = r.x * yratio;
				y = r.y * yratio;
			}
			return new Rect(x,y,width,height);
		}

		public static Rect PercentToPixels(Rect r){
			/* r contains values as percents of screen values
			 * returns a Rect with real (pixel) values
			 */
			return new Rect();
		}

	}

}
