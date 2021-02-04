using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatButton : MonoBehaviour
{
	public string sceneName = "scene_";
	public int count = 1;
	void Start()
	{

	}

	public void NextStage()
	{
		count--;
		if (count == 0)
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}