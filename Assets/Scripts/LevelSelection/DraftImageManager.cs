using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wootopia;

public class DraftImageManager : MonoBehaviour
{
	private static DraftImageManager instance;
	public static DraftImageManager Instance
	{
		get { return instance; }
	}
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

	public delegate void LoadSceneDelegate(string sceneName);
	public LoadSceneDelegate loadScene;

	public Image draftImage;

	public string sceneName;
	public WoCanvasGroup canvasGroup;

	private float startTime = 0;

	public float fadeInTime = 0.2f;

	public CanvasGroup sticker;

	void Start()
	{
		HideImage();
	}

	public void SetImage(Sprite sprite)
	{
		draftImage.sprite = sprite;
	}
	public void SetSceneName(string sceneName)
	{
		this.sceneName = sceneName;
	}

	public void ShowImage()
	{
		if (canvasGroup)
		{
			canvasGroup.Show();
		}
	}

	public void HideImage()
	{
		if (canvasGroup)
		{
			canvasGroup.Hide();
		}
	}

	public void LoadScene()
	{
		// SceneManager.LoadScene(sceneName);
		StartCoroutine(LoadAsyncScene());
	}

	IEnumerator LoadAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		startTime = Time.time;

		float factor = 0;

		while (factor < 1 && !asyncLoad.isDone)
		{
			factor = (Time.time - startTime) / fadeInTime;

			sticker.alpha = factor;

			yield return null;
		}
	}
}