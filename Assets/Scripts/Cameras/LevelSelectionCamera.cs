﻿using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

public class LevelSelectionCamera : MonoBehaviour
{
	[SerializeField]
	private bool controllable = true;
	public enum CoordinateType
	{
		ScaledPixels,
		ScreenPixels,
		ScreenPercentage
	}

	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	private LeanFingerFilter fingerFilter = new LeanFingerFilter(true);

	/// <summary>If the fingers didn't move, skip calling </summary>
	public bool ignoreIfStatic;

	/// <summary>The coordinate space of the OnDelta values.</summary>
	public CoordinateType coordinate;

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

	[Header("Pitch")]
	public bool pitchEnabled = true;
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

	[Header("Yaw")]
	public bool yawEnabled = true;

	/// <summary>Yaw of the rotation in degrees.</summary>
	[Tooltip("Yaw of the rotation in degrees.")]
	public float yaw;

	/// <summary>The strength of the yaw changes with horizontal finger movement.</summary>
	[Tooltip("The strength of the yaw changes with horizontal finger movement.")]
	public float yawSensitivity = 0.25f;

	/// <summary>Limit the yaw to min/max?</summary>
	[Tooltip("Limit the yaw to min / max? ")]
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

	[Header("Path")]
	public CameraPath path;
	public Stage currentStage;
	public Stage targetStage;
	private Transform target;
	public float moveSpeed;
	private int currentIndex;

	private Vector3 targetPosition;

	protected virtual void Start()
	{
		pitch = defaultRotation.x;
		yaw = defaultRotation.y;
		currentPitch = pitch;
		currentYaw = yaw;
		// SetCurrentStage(currentStage);
		Init();
	}

	protected virtual void LateUpdate()
	{
		// Get an initial list of fingers
		var fingers = fingerFilter.GetFingers();

		if (controllable && fingers.Count == 1)
		{
			// Debug.Log("Controlling Camera!!!");
			// Get delta
			var screenFrom = LeanGesture.GetLastScreenCenter(fingers);
			var screenTo = LeanGesture.GetScreenCenter(fingers);
			var finalDelta = screenTo - screenFrom;

			if (ignoreIfStatic && finalDelta.sqrMagnitude <= 0.0f)
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

			if (yawEnabled)
			{
				yaw += finalDelta.x * yawSensitivity * rotateSpeedScale;
			}
			if (pitchEnabled)
			{
				pitch -= finalDelta.y * pitchSensitivity * rotateSpeedScale;
			}

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
		UpdatePosition();
	}

	/// <summary>
	/// Set Camera can be controlled by the player or not
	/// </summary>
	/// <param name="value"></param>
	public void SetPlayerControllable(bool value)
	{
		controllable = value;
	}

	public void SetYaw(float yaw)
	{
		this.yaw = yaw;
	}

	public void MoveTo(Stage stage)
	{
		SetTargetStage(stage);
	}
	public void SetTargetStage(Stage stage)
	{
		currentStage.cameraLeave.Invoke();

		targetStage = stage;
		path = CameraPathManager.Instance.FindPath(this.currentStage, targetStage);
		if (path == null)
		{
			Debug.LogWarning("No Path Found!");
			return;
		}
		Debug.Log("Path Found: " + path.gameObject.name);

		currentIndex = 0;
		target = targetStage.transform;
		targetPosition = path.GetVertPosition(this.currentStage, currentIndex);
	}

	private void UpdatePosition()
	{
		// if (!target) { return; }

		if (!path) { return; }
		if (transform.position == targetPosition)
		{
			++currentIndex;
			if (currentIndex >= path.Count)
			{
				targetPosition = targetStage.transform.position;
			}
			else
			{
				targetPosition = path[currentIndex];
				targetPosition = path.GetVertPosition(currentStage, currentIndex);
			}

			Quaternion look = Quaternion.LookRotation(targetPosition - transform.position);
			float y = look.eulerAngles.y;
			float altY = y - 360;
			SetYaw(Mathf.Abs(y - yaw) > Mathf.Abs(altY - yaw) ? altY : y);
		}

		float distance = Vector3.Distance(targetPosition, transform.position);
		float deltaDistance = moveSpeed * Time.deltaTime;
		float factor = deltaDistance / distance;

		factor = Mathf.Clamp(factor, 0, 1);

		transform.position = Vector3.Lerp(transform.position, targetPosition, factor);

		if (transform.position == target.transform.position)
		{
			target = null;
			path = null;
			SetCurrentStage(targetStage);
		}

	}

	public void SetCurrentStage(Stage stage)
	{
		currentStage = stage;
		PlayerPrefs.SetInt("CurStage", stage.stageIndex);
		currentStage.cameraEnter.Invoke();
		StageManager.Instance.UpdateCurrentStage(currentStage);
	}

	public void Init()
	{

		int stageIndex = PlayerPrefs.GetInt("CurStage", 0);
		Stage stage = StageManager.Instance.GetStageByIndex(stageIndex);

		transform.position = stage.transform.position;
		SetCurrentStage(stage);
		if (stageIndex == 0)
		{
			SetTargetStage(StageManager.Instance.GetStageByIndex(1));
		}
		else
		{
			// PlayerPrefs.SetInt("PrevStageIndex", 3);
			GameObject.Find("Story Canvas")?.SetActive(false);
			int levelIndex = PlayerPrefs.GetInt("PrevStageIndex", 0) - (stageIndex == 1 ? 0 : 1);
			Debug.Log(stageIndex + "  " + levelIndex);
			Vector3 levelPos = stage.levels[levelIndex].transform.position;
			levelPos.y = transform.position.y;
			Vector3 rotation = Quaternion.LookRotation(levelPos - transform.position).eulerAngles;	
			SetYaw(rotation.y);
			Debug.Log(rotation.y);
		}
	}
}