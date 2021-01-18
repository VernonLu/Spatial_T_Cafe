﻿using System.Collections;
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
		SceneManager.LoadScene("Level Selection");
	}

	/// <summary>
	/// Save Player progress when the level is finished
	/// </summary>
	public void SaveProgress()
	{
		Scene scene = SceneManager.GetActiveScene();
		PlayerPrefs.SetInt(scene.name, 1);
	}

	public void TestFunc(int value)
	{
		Debug.Log(value);
	}

	public void Flip(GameObject go)
	{
		go.transform.rotation = Quaternion.Euler(0, 180, 0);
	}
}