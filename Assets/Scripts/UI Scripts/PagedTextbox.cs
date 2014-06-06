using UnityEngine;
using System.Collections;

namespace CustomGUI {

	/*
	* Currently the only script which follows the BaseUIComponent scheme.
	* Creates paged textbox and pages the text based on font size and available space
	*/
	public class PagedTextbox : BaseUIComponent {
		public GUIStyle textboxStyle;
		public Texture2D prevButtonTexture;
		public Texture2D nextButtonTexture;

		public bool Show{get; set;}

		Rect textboxRect;
		Rect prevButtonRect;
		Rect nextButtonRect;

		int textPageToShow;
		string[] textPages;
		// Use this for initialization
		void Awake () {
			Show = true;
			textboxRect = new Rect();
			prevButtonRect = new Rect();
			nextButtonRect = new Rect();

			textPageToShow = 0;
			textPages = new string[]{""};

			/* some fonties. could be handled elsewhere.
			 * one solution:
			 * 	move these settings into GUIManager and scale fonts with DPI if provided
			 */
			
			// local font size settings, ugly:
			#if UNITY_STANDALONE
			textboxStyle.fontSize = 27;
			#elif UNITY_EDITOR
			textboxStyle.fontSize = 22;
			#else
			textboxStyle.fontSize = 30;
			#endif
		}

		public void SetText(string text){
			textPages = SplitIntoPages(text);
			textPageToShow = 0;
			
		}

		#region implemented abstract members of BaseUIComponent

		public override void SetSize(float width, float height){
			//textboxRect.Set(textboxRect.x,textboxRect.y,width, height);
			textboxRect.width = width;
			textboxRect.height = height;
			AdjustButtonLocations();
		}

		public override void SetPosition(float x, float y){
			//textboxRect.Set(x, y, textboxRect.width, textboxRect.height);
			textboxRect.x = x;
			textboxRect.y = y;
			AdjustButtonLocations();
		}

		
		public override UIDrawDelegate GetUIDrawDelegate ()
		{
			return (DrawTextboxAndControls);
		}

		public override bool CointainsPoint (Vector2 point)
		{
			return textboxRect.Contains(point) || prevButtonRect.Contains(point) || nextButtonRect.Contains(point);
		}

		#endregion

		void AdjustButtonLocations(){
			// right placement
			prevButtonRect = new Rect (textboxRect.xMax + 20, textboxRect.y , prevButtonTexture.width, prevButtonTexture.height);
			nextButtonRect = new Rect (textboxRect.xMax + 20, textboxRect.yMax - nextButtonTexture.height, nextButtonTexture.width, nextButtonTexture.height);
		}

		void DrawTextboxAndControls(){
			// this function is passed as a delegate to GUIManager
			if(!Show)return;
			GUI.Box(textboxRect, textPages[textPageToShow], textboxStyle);

			if (textPageToShow != 0) {
				
				if (GUI.Button (prevButtonRect, prevButtonTexture, "Label")) {
					
					textPageToShow--;
					
				}
				
			}
			
			if (textPageToShow != textPages.Length - 1) {
				
				if (GUI.Button (nextButtonRect,  nextButtonTexture, "Label")) {
					textPageToShow++;	
				}	
			}


		}
		
		string[] SplitIntoPages (string rawtext)
		{
			float maxh = textboxStyle.CalcHeight(new GUIContent(rawtext), textboxRect.width - textboxStyle.padding.left - textboxStyle.padding.right);
			float fpages = maxh/(textboxRect.height );
			int charsperpage = (int)(rawtext.Length/fpages);
			
			// this is a piece modified from the Story class.
			// this splitting might be better moved to some tools class.
			string[] pages = new string[(int)fpages + 1];
			for(int a = 0; a < pages.Length; a++)pages[a]="";
			
			string[] words = rawtext.Split(' ');
			
			int page = 0;
			for(int i = 0; i < words.Length; i++){
				if(pages[page].Length + words[i].Length > charsperpage && page < pages.Length -1) page++;
				pages[page] += words[i] + " ";
				
			}
			return pages;
		}
	}

}
