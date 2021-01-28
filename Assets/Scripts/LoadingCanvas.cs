using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wootopia;

public class LoadingCanvas : MonoBehaviour
{
	public bool autoDestroy = false;
	public float minInterival = 1.0f;
	private float time = 0;
	[Header("Blueprint")]
	public Animator anim;
	public WoCanvasGroup canvasGroup;

	[Header("Continue Button")]
	public Button button;

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		if (button)
		{
			button.interactable = false;
		}
		else
		{
			autoDestroy = true;
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void Update()
	{
		time += Time.deltaTime;
	}

	/// <summary>
	/// Callbacks when new scene loaded
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="mode"></param>
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log(SceneManager.GetActiveScene().name);

		autoDestroy |= scene.name.Equals("Main Menu");
		Debug.Log("AutoDestroy" + autoDestroy);

		float timeRemaining = minInterival < time ? 0 : minInterival - time;

		Invoke((autoDestroy? "AutoDestroy": "ShowButton"), timeRemaining);

		// ShowButton();
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

	private void ShowButton()
	{
		if (button)
		{
			button.interactable = true;
		}
	}

	/// <summary>
	/// Show loading panel
	/// </summary>
	public void Show()
	{
		canvasGroup.Show();
		anim?.SetTrigger("Trigger");
		time = 0;
	}

}