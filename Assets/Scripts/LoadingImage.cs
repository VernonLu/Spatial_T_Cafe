using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImage : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public float lastingTime = 2f;
	public float fadeTime = 0.5f;

	private float time;

	private bool isDestroying = false;

	void Start()
	{
		time = 0;
	}

	void Update()
	{
		time += Time.deltaTime;
		if (time > lastingTime)
		{
			canvasGroup.alpha = 1 - (time - lastingTime) / fadeTime;
		}

		if (canvasGroup.alpha == 0 && !isDestroying)
		{
			AutoDestroy();
		}
	}

	private void AutoDestroy()
	{
		isDestroying = true;
		Destroy(this.gameObject);
	}
}