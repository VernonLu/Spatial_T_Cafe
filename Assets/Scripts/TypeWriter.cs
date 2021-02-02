﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypeWriter : MonoBehaviour
{
	public Text textComponent;
	private string content;

	public float charInterval = 0.1f;

	public UnityEvent onStartType;
	public UnityEvent onTyping;
	public UnityEvent onFinishType;

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		// StartType();
	}

	public void Init()
	{
		content = textComponent.text;
		textComponent.text = "";
	}

	public void StartType()
	{
		StartCoroutine("ShowText");
	}

	private IEnumerator ShowText()
	{
		onStartType.Invoke();
		WaitForSeconds interval = new WaitForSeconds(charInterval);
		for (int i = 0; i < content.Length; ++i)
		{

			onTyping.Invoke();
			textComponent.text += content[i];
			yield return interval;
		}
		onFinishType.Invoke();

	}

}