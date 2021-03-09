using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;
public class StageSwitchButton : MonoBehaviour
{
	public Stage currentStage;
	public Stage targetStage;

	public WoCanvasGroup button;

	public bool isActive = false;

	void Start()
	{
		SetActive(isActive);
	}

	public void SetActive(bool active)
	{
		isActive = active && !targetStage.IsLocked;
		if (isActive)
		{
			button.Show();
		}
		else
		{
			button.Hide();
		}
	}
	public void UpdateState()
	{
		
	}
}