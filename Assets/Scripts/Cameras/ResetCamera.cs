using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

public class ResetCamera : MonoBehaviour
{
	/// <summary>Ignore fingers with StartedOverGui?</summary>
	public bool IgnoreStartedOverGui = true;

	/// <summary>Ignore fingers with OverGui?</summary>
	public bool IgnoreIsOverGui;

	/// <summary>How many times must this finger tap before OnTap gets called?
	/// 0 = Every time (keep in mind OnTap will only be called once if you use this).</summary>
	public int RequiredTapCount = 0;

	/// <summary>How many times repeating must this finger tap before OnTap gets called?
	/// /// 0 = Every time (e.g. a setting of 2 means OnTap will get called when you tap 2 times, 4 times, 6, 8, 10, etc).</summary>
	public int RequiredTapInterval;

	protected virtual void OnEnable()
	{
		LeanTouch.OnFingerTap += HandleFingerTap;
	}

	protected virtual void OnDisable()
	{
		LeanTouch.OnFingerTap -= HandleFingerTap;
	}

	private void HandleFingerTap(LeanFinger finger)
	{
		// Ignore?
		if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
		{
			return;
		}

		if (IgnoreIsOverGui == true && finger.IsOverGui == true)
		{
			return;
		}
		Debug.Log(finger.TapCount);
		if (RequiredTapCount > 0 && finger.TapCount != RequiredTapCount)
		{
			return;
		}

		if (RequiredTapInterval > 0 && (finger.TapCount % RequiredTapInterval) != 0)
		{
			return;
		}

		ControlManager.Instance.ResetCamera();

	}
}