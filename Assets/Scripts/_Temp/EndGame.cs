﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;
public class EndGame : MonoBehaviour
{
	public string sceneName;
	public WoCanvasGroup canvasGroup;

	void Start()
	{
		canvasGroup.Hide();
		bool isFinished = PlayerPrefs.GetInt(sceneName, 0) == 1;
		if (isFinished)
		{
			canvasGroup.Show();
		}
	}

}