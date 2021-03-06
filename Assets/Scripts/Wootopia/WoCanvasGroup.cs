﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wootopia
{

	[RequireComponent(typeof(CanvasGroup))]
	public class WoCanvasGroup : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private bool isActive = true;

		public float fadeDuration = 0.2f;

		private float time;

		private void Reset()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
		private void Start()
		{
			if (isActive)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		private void Update()
		{
			time += Time.deltaTime;
			time = Mathf.Clamp(time, 0, fadeDuration);
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, isActive? 1 : 0, time / fadeDuration);
		}

		[ContextMenu("Hide")]
		public void Hide()
		{
			// canvasGroup.alpha = 0;
			isActive = false;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			time = 0;
		}

		[ContextMenu("Show")]
		public void Show()
		{
			// canvasGroup.alpha = 1;
			isActive = true;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			time = 0;
		}
	}
}