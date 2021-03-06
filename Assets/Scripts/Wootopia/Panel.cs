﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Wootopia
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Panel : MonoBehaviour
	{
		
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private bool isActive = true;
		public UnityEvent onShow;
		public UnityEvent onHide;

		private void Reset()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			canvasGroup = canvasGroup ?? GetComponent<CanvasGroup>();
		}

		public void Show()
		{
			Toggle(true);
		}
		public void Hide()
		{
			Toggle(false);
		}
		public void Toggle(bool isOn)
		{

			isActive = isOn;
			canvasGroup.alpha = isActive ? 1 : 0;
			canvasGroup.interactable = isActive;
			canvasGroup.blocksRaycasts = isActive;
			(isOn ? onShow : onHide).Invoke();
		}
	}
}
