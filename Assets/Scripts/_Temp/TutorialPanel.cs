using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Lean.Touch;
public class TutorialPanel : MonoBehaviour
{
	public LeanFingerFilter Use = new LeanFingerFilter(true);

	public UnityEvent onDisable;
	
	private bool success = false;

	void Start()
    {
		
	}
	private void OnEnable()
	{
		LeanTouch.OnFingerUp += HandleFingerUp;
		LeanTouch.OnFingerDown += HandleFingerDown;
		Debug.Log(gameObject.name + ": OnEnable");
	}

	private void OnDisable()
	{
		LeanTouch.OnFingerUp -= HandleFingerUp;
		LeanTouch.OnFingerDown -= HandleFingerDown;
		Debug.Log(gameObject.name + ": OnDisable");
		onDisable.Invoke();
	}

	private void HandleFingerDown(LeanFinger finger)
	{
		if (Use.GetFingers().Count == Use.RequiredFingerCount)
		{
			success = true;
		}
	}
	private void HandleFingerUp(LeanFinger finger)
	{
		//if (Use.GetFingers().Count != Use.RequiredFingerCount)
		//{
		//	return;
		//}
		if (!success)
		{
			return;
		}
		Debug.Log(gameObject.name + ": OnFingerUp");
		ControlManager.Instance.HideHighlight();
		Debug.Log(gameObject.name + ": Hide");
		gameObject.SetActive(false);
	}
}
