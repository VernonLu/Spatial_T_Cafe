using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

	public Image draftImage;

	[SerializeField]
	private string sceneName;
	public WoCanvasGroup canvasGroup;

	public LoadingCanvas loadingCanvas;

	public UnityEvent onShow;
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
		canvasGroup?.Show();
		onShow.Invoke();
	}

	public void HideImage()
	{
		canvasGroup?.Hide();
	}

	public void LoadScene()
	{
		loadingCanvas?.Show();
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
	}
}