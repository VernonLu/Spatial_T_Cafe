using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Wootopia;

public class Postcard : MonoBehaviour
{
	private bool isFliped;
	private bool isFlipping;

	private float time;

	[Tooltip("Time used to flip the post card")]
	public float duration;

	private RectTransform postcard;

	public WoCanvasGroup front;
	public WoCanvasGroup back;

	// public Animator animator;

	public UnityEvent onStartFlip;
	public UnityEvent onSideChange;
	public UnityEvent onFinishFlip;
	void Start()
	{
		postcard = GetComponent<RectTransform>();
	}

	private void FixedUpdate()
	{
		if (isFlipping)
		{
			time += Time.fixedDeltaTime;
			float scaleX = Mathf.Abs(Mathf.Cos(Mathf.PI * time / duration));
			postcard.localScale = new Vector3(scaleX, 1, 1);
			if (!isFliped && time > duration / 2)
			{
				front.Hide();
				back.Show();
				isFliped = true;
			}
			if (time > duration)
			{
				FinishFlip();
			}
		}
	}

	public void StartFlip()
	{
		// animator.SetTrigger("Flip");

		if (!isFlipping)
		{
			isFlipping = true;
			time = 0;
		}
		onStartFlip.Invoke();
	}

	public void FinishFlip()
	{
		isFliped = false;
		isFlipping = false;
		onFinishFlip.Invoke();
	}

}