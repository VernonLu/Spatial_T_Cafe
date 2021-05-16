using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;
using Lean.Touch;

public class RotateCamera : MonoBehaviour
{
	public bool canRotate = true;
	public enum CoordinateType
	{
		ScaledPixels,
		ScreenPixels,
		ScreenPercentage
	}

	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	public LeanFingerFilter fingerFilter = new LeanFingerFilter(true);

	/// <summary>If the fingers didn't move, skip calling </summary>
	public bool ignoreIfStatic;

	/// <summary>The coordinate space of the OnDelta values.</summary>
	public CoordinateType coordinate;

	/// <summary>The delta values will be multiplied by this when output.</summary>
	public float multiplier = 1.0f;

	[Header("Camera Parameters")]
	/// <summary>If you want the rotation to be scaled by the camera FOV, then set the camera here.</summary>
	[Tooltip("If you want the rotation to be scaled by the camera FOV, then set the camera here.")]
	public Camera Camera;

	/// <summary>This allows you to set the Pitch andYaw rotation value when calling the ResetRotation method.</summary>
	[Tooltip("This allows you to set the Pitch and Yaw rotation value when calling the ResetRotation method.")]
	public Vector2 defaultRotation;

	/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
	/// -1 = Instantly change.
	/// 1 = Slowly change.
	/// 10 = Quickly change.</summary>
	[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
	public float dampening = -1.0f;

	[Space]

	/// <summary>Pitch of the rotation in degrees.</summary>
	[Tooltip("Pitch of the rotation in degrees.")]
	public float pitch;

	/// <summary>The strength of the pitch changes with vertical finger movement.</summary>
	[Tooltip("The strength of the pitch changes with vertical finger movement.")]
	public float pitchSensitivity = 0.25f;

	/// <summary>Limit the pitch to min/max?</summary>
	[Tooltip("Limit the pitch to min/max?")]
	public bool pitchClamp = true;

	/// <summary>The minimum pitch angle in degrees.</summary>
	[Tooltip("The minimum pitch angle in degrees.")]
	public float pitchMin = -90.0f;

	/// <summary>The maximum pitch angle in degrees.</summary>
	[Tooltip("The maximum pitch angle in degrees.")]
	public float pitchMax = 90.0f;

	[Space]

	/// <summary>Yaw of the rotation in degrees.</summary>
	[Tooltip("Yaw of the rotation in degrees.")]
	public float yaw;

	/// <summary>The strength of the yaw changes with horizontal finger movement.</summary>
	[Tooltip("The strength of the yaw changes with horizontal finger movement.")]
	public float yawSensitivity = 0.25f;

	/// <summary>Limit the yaw to min/max?</summary>
	[Tooltip("Limit the yaw to min/max?")]
	public bool yawClamp;

	/// <summary>The minimum yaw angle in degrees.</summary>
	[Tooltip("The minimum yaw angle in degrees.")]
	public float yawMin = -45.0f;

	/// <summary>The maximum yaw angle in degrees.</summary>
	[Tooltip("The maximum yaw angle in degrees.")]
	public float yawMax = 45.0f;

	[HideInInspector]
	[SerializeField]
	private float currentPitch;

	[HideInInspector]
	[SerializeField]
	private float currentYaw;

	public float rotateSpeedScale = 1.0f;
	public AnimationCurve rotateSpeedScaleCurve;

	/// <summary>This method resets the Pitch and Yaw values to the DefaultRotation value.</summary>
	[ContextMenu("Reset Rotation")]
	public virtual void ResetRotation()
	{
		pitch = defaultRotation.x;
		yaw = defaultRotation.y;
	}

	public void Rotate(Vector2 delta)
	{
		var sensitivity = GetSensitivity();

		// rotateSpeedScale = rotateSpeedScaleCurve.Evaluate(Camera.fieldOfView);
		rotateSpeedScale = CameraSpeedController.rotateSensitivity;

		yaw += delta.x * yawSensitivity * sensitivity * rotateSpeedScale;
		pitch -= delta.y * pitchSensitivity * sensitivity * rotateSpeedScale;
	}

#if UNITY_EDITOR
	protected virtual void Reset()
	{
		fingerFilter.UpdateRequiredSelectable(gameObject);
	}
#endif

	protected virtual void Awake()
	{
		fingerFilter.UpdateRequiredSelectable(gameObject);
	}

	protected virtual void Start()
	{
		currentPitch = pitch;
		currentYaw = yaw;
	}

	protected virtual void LateUpdate()
	{
		// Get an initial list of fingers
		var fingers = fingerFilter.GetFingers();

		if (fingers.Count == 1)
		{
			// Debug.Log(canRotate);
			if (!canRotate)
			{
				// Debug.Log("Controlling Object!!!");
				return;
			}
			// Debug.Log("Controlling Camera!!!");
			// Get delta
			var screenFrom = LeanGesture.GetLastScreenCenter(fingers);
			var screenTo = LeanGesture.GetScreenCenter(fingers);
			var finalDelta = screenTo - screenFrom;

			if (ignoreIfStatic == true && finalDelta.sqrMagnitude <= 0.0f)
			{
				return;
			}

			switch (coordinate)
			{
			case CoordinateType.ScaledPixels:
				finalDelta *= LeanTouch.ScalingFactor;
				break;
			case CoordinateType.ScreenPercentage:
				finalDelta *= LeanTouch.ScreenFactor;
				break;
			}

			finalDelta *= multiplier;

			Rotate(finalDelta);

		}

		if (pitchClamp == true)
		{
			pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
		}

		if (yawClamp == true)
		{
			yaw = Mathf.Clamp(yaw, yawMin, yawMax);
		}

		// Get t value
		var factor = LeanTouch.GetDampenFactor(dampening, Time.deltaTime);

		// Lerp the current values to the target ones
		currentPitch = Mathf.Lerp(currentPitch, pitch, factor);
		currentYaw = Mathf.Lerp(currentYaw, yaw, factor);

		// Rotate to pitch and yaw values
		transform.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0.0f);
	}

	private float GetSensitivity()
	{
		// Has a camera been set?
		if (Camera != null)
		{
			// Adjust sensitivity by FOV?
			if (Camera.orthographic == false)
			{
				return Camera.fieldOfView / 90.0f;
			}
		}

		return 1.0f;
	}

	private void OnApplicationQuit()
	{
		Debug.Log("Rotate Speed Scale: " + rotateSpeedScale);
	}
	public void UpdateSpeed(float speed)
	{
		rotateSpeedScale = speed;
	}

	public void SetRotateCamera(bool status)
	{
		// Debug.Log("FK " + status);
		canRotate = status;
	}
}