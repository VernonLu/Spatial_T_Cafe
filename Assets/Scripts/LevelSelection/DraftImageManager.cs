using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wootopia;

public class DraftImageManager : MonoBehaviour
{

	[SerializeField]
	private string sceneName;
	public WoCanvasGroup canvasGroup;

	public LoadingCanvas loadingCanvas;

	public Animator animator;
	public float animTime = 1f;
	public UnityEvent onShow;
	public UnityEvent onLetterOpened;
	void Start()
	{
		HideImage();
	}

	public void SetSceneName(string sceneName)
	{
		this.sceneName = sceneName;
	}

	public void ShowImage()
	{
		canvasGroup?.Show();
		onShow.Invoke();
		if (animator)
		{
			animator.SetTrigger("Open");
			//StartCoroutine("LetterOpened");
			Invoke("LetterOpened", animTime);
		}
	}

	private void LetterOpened()
	{
		// yield return new WaitForSeconds(animTime);
		onLetterOpened.Invoke();
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