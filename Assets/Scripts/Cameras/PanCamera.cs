using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
	/// <summary>The camera that will be used during calculations.
	/// None = MainCamera.</summary>
	public Camera Camera;

	/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
	public LeanScreenDepth screenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	public LeanFingerFilter panFinger = new LeanFingerFilter(true);

	/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
	/// -1 = Instantly change.
	/// 1 = Slowly change.
	/// 10 = Quickly change.</summary>
	[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
	public float panDampening = -1.0f;

	/// <summary>This allows you to control how much momenum is retained when the dragging fingers are all released.
	/// NOTE: This requires <b>Dampening</b> to be above 0.</summary>
	[Tooltip("This allows you to control how much momenum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Dampening</b> to be above 0.")]
	[Range(0.0f, 1.0f)]
	public float inertia;

	[HideInInspector]
	[SerializeField]
	private Vector3 remainingDelta;

	[Tooltip("The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.")]
	public float panSpeedScale = 1f;

#if UNITY_EDITOR
	protected virtual void Reset()
	{
		panFinger.UpdateRequiredSelectable(gameObject);
	}
#endif

	protected virtual void Awake()
	{
		panFinger.UpdateRequiredSelectable(gameObject);
	}

	protected virtual void LateUpdate()
	{
		Pan();
	}

	private void Pan()
	{
		var fingers = panFinger.GetFingers();

		// Get the last and current screen point of all fingers
		var lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
		var screenPoint = LeanGesture.GetScreenCenter(fingers);

		// Get the world delta of them after conversion
		var worldDelta = screenDepth.ConvertDelta(lastScreenPoint, screenPoint, gameObject);

		// Store the current position
		var oldPosition = transform.localPosition;

		// Pan the camera based on the world delta
		transform.position -= worldDelta * panSpeedScale * CameraSpeedController.panSensitivity;

		// Add to remainingDelta
		remainingDelta += transform.localPosition - oldPosition;

		// Get t value
		var factor = LeanTouch.GetDampenFactor(panDampening, Time.deltaTime);

		// Dampen remainingDelta
		var newRemainingDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

		// Shift this position by the change in delta
		transform.localPosition = oldPosition + remainingDelta - newRemainingDelta;

		if (fingers.Count == 0 && inertia > 0.0f && panDampening > 0.0f)
		{
			newRemainingDelta = Vector3.Lerp(newRemainingDelta, remainingDelta, inertia);
		}

		// Update remainingDelta with the dampened value
		remainingDelta = newRemainingDelta;
	}

	private void OnApplicationQuit()
	{
		Debug.Log("Pan Speed Scale: " + panSpeedScale);
	}
	public void UpdateSpeed(float speed)
	{
		panSpeedScale = speed;
	}

	public void ResetCamera()
	{
		Vector3 pos = transform.localPosition;
		pos.x = 0;
		pos.y = 0;
		transform.localPosition = pos;
	}
}