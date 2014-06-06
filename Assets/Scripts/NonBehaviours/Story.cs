using UnityEngine;
using System.Collections;

[System.Serializable]
public class Story {
	/*
	 * DEPRECATED!
	 * Contains a story or info element 
	 * and some associated funtions. For
	 * now only stores the story and can
	 * split it into pages.
	 * 
	 * I am undecided if this serves any 
	 * purpose
	 */

	public string caption;
	public string story;
	public int pages;
	private string[] pagedstory;

	public Story(){
		caption = "";
		story = "";
		pages = 0;
	}

	public Story(string cap, string sto, int pag){
		caption = cap;
		story = sto;
		pages = pag;
	}

	public string[] SplitIntoPages(int pagecount){
		// this splits the story into array of length pagecount
		if(pagedstory != null) return pagedstory;

		// splitting done only once
		string[] pages = new string[pagecount];
		int wpp = story.Length / pagecount;
		string[] words = story.Split(' ');

		int page = 0;
		for(int i = 0; i < words.Length; i++){
			pages[page] += words[i] + " ";
			if(pages[page].Length > wpp && page < pages.Length -1)page++;
		}
		pagedstory = pages;
		return pages;
	}

	public string[] GetPages(){
		return SplitIntoPages(this.pages);
	}
}
