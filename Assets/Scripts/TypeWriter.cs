using System.Collections;
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

	[Tooltip("This event is called every frame")]
	public UnityEvent onTyping;
	public UnityEvent onFinishType;

	private void Awake()
	{
		Init();
	}
	
	public void Init()
	{
		SetText(textComponent.text);
		ClearContent();
	}

	public void SetText(string text)
	{
		content = text;
	}

	public void StartType()
	{
		ClearContent();
		StartCoroutine("ShowText");
	}

	public void ClearContent()
	{
		textComponent.text = "";
	}


	private IEnumerator ShowText()
	{
		textComponent.text = "";
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

	[ContextMenu("Get Text Component")]
	private void GetTextComponent()
	{
		textComponent = GetComponent<Text>();
	}

}