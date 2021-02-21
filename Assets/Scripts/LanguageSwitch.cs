using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSwitch : MonoBehaviour
{
	[System.Serializable]
	public class LanguageProfile
	{
		public string Title;
		public Font Font;
	}
	[SerializeField]
	private int currentIndex = 0;

	public Text titleText;

	[SerializeField]
	private List<LanguageProfile> languages = new List<LanguageProfile>();
	private void Start()
	{
		// Load Language Selection on Start if not exist, the default is 0
		currentIndex = PlayerPrefs.GetInt("Language", 0);

	}

	public void ToggleNext()
	{
		currentIndex = (currentIndex + 1) % languages.Count;
		titleText.text = languages[currentIndex].Title;
		titleText.font = languages[currentIndex].Font;
		Save();
	}

	private void Save()
	{
		PlayerPrefs.SetInt("Language", currentIndex);
	}

}