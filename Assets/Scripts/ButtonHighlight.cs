using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlight : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public float interival = 0.1f;

	private float time;
	// Start is called before the first frame update
	void Start()
	{
		time = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	// Update is called once per frame
	void Update()
	{
		time += Time.deltaTime;
		canvasGroup.alpha = 1 - Mathf.Cos(2 * Mathf.PI * time / interival);
	}
}