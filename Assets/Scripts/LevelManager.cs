using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wootopia;

public class LevelManager : MonoBehaviour
{
	private static LevelManager instance;
	public static LevelManager Instance { get { return instance; } }
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

	public WoCanvasGroup endGamePanel;
	void Start()
	{
		endGamePanel.Hide();
	}

	public void ShowEndGame()
	{
		endGamePanel.Show();
	}

	public void Back2Menu()
	{
		Scene scene = SceneManager.GetActiveScene();
		PlayerPrefs.SetInt(scene.name, 1);
		SceneManager.LoadScene("Level Selection");
	}
}