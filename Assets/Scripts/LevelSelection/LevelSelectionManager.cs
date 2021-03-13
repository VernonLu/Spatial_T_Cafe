using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wootopia;
public class LevelSelectionManager : MonoBehaviour
{
	public WoCanvasGroup canvasGroup;


	private void Start()
	{
		canvasGroup.Hide();
		StageManager.Instance.ManualInit();
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