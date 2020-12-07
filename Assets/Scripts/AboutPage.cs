using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;

public class AboutPage : MonoBehaviour
{
	public List<WoCanvasGroup> pages = new List<WoCanvasGroup>();

	public bool loop = true;

	[Tooltip("The index of current page")]
	private int index = 0;

	void Start()
	{
		UpdatePage();
	}

	public void ShowNext()
	{
		index = loop ? (index + 1) % pages.Count : index + 1;
		UpdatePage();
	}

	public void ShowPrev()
	{
		index = loop ? (index + pages.Count - 1) % pages.Count : index - 1;
		UpdatePage();
	}

	private void UpdatePage()
	{
		index = Mathf.Clamp(index, 0, pages.Count - 1);
		foreach (var page in pages)
		{
			page.Hide();
		}
		pages[index].Show();
	}

}