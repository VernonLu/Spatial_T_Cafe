using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSwitch : MonoBehaviour
{
	public GameObject languageList;

	[SerializeField]
	private List<Toggle> languageOptions = new List<Toggle>();

	[SerializeField]
	private int currentIndex = 0;
	private void Start()
	{
		// Load Language Selection on Start if not exist, the default is 0
		currentIndex = PlayerPrefs.GetInt("Language", 0);

		// 
		languageList = languageList ? languageList : gameObject;
		languageOptions = languageList.GetComponentsInChildren<Toggle>().ToList();
		foreach (var toggle in languageOptions)
		{
			toggle.interactable = false;
		}
		Toggle(currentIndex);
	}

	public void ToggleNext()
	{
		currentIndex = (currentIndex + 1) % languageOptions.Count;
		Toggle(currentIndex);
		Save();
	}

	private void Toggle(int index)
	{
		if (index < 0 || index >= languageOptions.Count) { return; }
		languageOptions[currentIndex].isOn = true;
	}
	private void Save()
	{
		PlayerPrefs.SetInt("Language", currentIndex);
	}

}