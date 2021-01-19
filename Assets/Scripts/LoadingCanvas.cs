using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wootopia;

public class LoadingCanvas : MonoBehaviour
{
	public Button button;
	public Image blueprintImage;
	public WoCanvasGroup canvasGroup;

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		button.interactable = false;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	/// <summary>
	/// Callbacks when new scene loaded
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="mode"></param>
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log(SceneManager.GetActiveScene().name);
		button.interactable = true;
	}

	/// <summary>
	/// Change the sprite of blueprint
	/// </summary>
	/// <param name="sprite"></param>
	public void SetBlueprintSprite(Sprite sprite)
	{
		blueprintImage.sprite = sprite;
	}

	/// <summary>
	/// Show loading panel
	/// </summary>
	public void Show()
	{
		canvasGroup.Show();
	}

	/// <summary>
	/// Destroy gameobject after the canvas faded out
	/// </summary>
	public void AutoDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		canvasGroup.Hide();
		Destroy(this.gameObject, canvasGroup.fadeDuration);
	}
}