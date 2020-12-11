using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;
public class StageSwitchButton : MonoBehaviour
{
	public Stage stage;

	public WoCanvasGroup button;

	public bool isActive = false;

	void Start()
	{
		SetActive(isActive);
	}

	public void SetActive(bool active)
	{
		isActive = active;
		if (isActive)
		{
			button.Show();
		}
		else
		{
			button.Hide();
		}
	}
}