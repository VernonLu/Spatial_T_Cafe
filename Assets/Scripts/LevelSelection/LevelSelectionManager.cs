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
		int stageIndex = PlayerPrefs.GetInt("CurStage", 1);
		Stage stage = StageManager.Instance.GetStageByIndex(stageIndex);
		levelSelectionCamera.transform.position = stage.transform.position;
		levelSelectionCamera.SetCurrentStage(stage);
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