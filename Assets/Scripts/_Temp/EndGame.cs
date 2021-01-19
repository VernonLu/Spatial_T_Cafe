using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;
public class EndGame : MonoBehaviour
{

	public WoCanvasGroup canvasGroup;
	// Start is called before the first frame update
	void Start()
	{
		canvasGroup.Hide();
		bool isFinished = PlayerPrefs.GetInt("scene_1-1", 0) == 1;
		if (isFinished)
		{
			canvasGroup.Show();
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

}