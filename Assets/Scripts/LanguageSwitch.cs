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

	public List<LanguageProfile> languages = new List<LanguageProfile>();
	private void Start()
	{
		// Load Language Selection on Start if not exist, the default is 0
		currentIndex = PlayerPrefs.GetInt("Language", 0);
		SetLanguageDisplay(currentIndex);

	}

	public void ToggleNext()
	{
		currentIndex = (currentIndex + 1) % languages.Count;
		SetLanguageDisplay(currentIndex);
		Save();
	}

	private void Save()
	{
		PlayerPrefs.SetInt("Language", currentIndex);
	}
	private void SetLanguageDisplay(int index)
	{
		titleText.text = languages[index].Title;
		titleText.font = languages[index].Font;
	}

}