using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wootopia;
public class LevelSelectionManager : MonoBehaviour
{
	public WoCanvasGroup canvasGroup;

	public LevelSelectionCamera levelSelectionCamera;

	private void Start()
	{
		canvasGroup.Hide();
		StageManager.Instance.ManualInit();
		int stageIndex = PlayerPrefs.GetInt("CurStage", 0);
		
		Stage stage = StageManager.Instance.GetStageByIndex(stageIndex);
		levelSelectionCamera.transform.position = stage.transform.position;
		levelSelectionCamera.SetCurrentStage(stage);
		if (stageIndex == 0)
		{
			levelSelectionCamera.SetTargetStage(StageManager.Instance.GetStageByIndex(1));
		}
		else
		{
			GameObject.Find("Story Canvas")?.SetActive(false);
		}
	}

	public void ShowPopUp()
	{
		canvasGroup.Show();
	}
	public void HidePopUp()
	{
		canvasGroup.Hide();
	}

	public void Back2Menu()
	{
		SceneManager.LoadScene("Main Menu");
	}
}