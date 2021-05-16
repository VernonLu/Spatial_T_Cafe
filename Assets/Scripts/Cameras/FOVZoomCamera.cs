using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class FOVZoomCamera : MonoBehaviour
{
	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	protected LeanFingerFilter Use = new LeanFingerFilter(true);

	/// <summary>The camera that will be used during calculations.
	/// None = MainCamera.</summary>
	public Camera Camera;

	private float defaultZoom = 50f;
	/// <summary>The current FOV/Size.</summary>
	public float Zoom = 50.0f;

	/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
	/// -1 = Instantly change.
	/// 1 = Slowly change.
	/// 10 = Quickly change.</summary>
	public float Dampening = -1.0f;

	/// <summary>Limit the FOV/Size?</summary>
	public bool Clamp;

	/// <summary>The minimum FOV/Size we want to zoom to.</summary>
	public float ClampMin = 10.0f;

	/// <summary>The maximum FOV/Size we want to zoom to.</summary>
	public float ClampMax = 60.0f;

	/// <summary>Should the zoom be performanced relative to the finger center?</summary>
	public bool Relative;

	/// <summary>Ignore changes in Z translation for 2D?</summary>
	public bool IgnoreZ;

	/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
	public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

	[HideInInspector]
	[SerializeField]
	private float currentZoom;

	[HideInInspector]
	[SerializeField]
	private Vector3 remainingTranslation;

	private float zoomSpeedScale = 1f;

	public void ContinuouslyZoom(float direction)
	{
		var factor = LeanTouch.GetDampenFactor(Mathf.Abs(direction), Time.deltaTime);

		if (direction > 0.0f)
		{
			Zoom = Mathf.Lerp(Zoom, ClampMax, factor);
		}
		else if (direction <= 0.0f)
		{
			Zoom = Mathf.Lerp(Zoom, ClampMin, factor);
		}
	}

	/// <summary>This method allows you to multiply the current <b>Zoom</b> value by the specified scale. This is useful for quickly changing the zoom from UI button clicks, or <b>LeanMouseWheel</b> scrolling.</summary>
	public void MultiplyZoom(float scale)
	{
		Zoom *= scale;

		if (Clamp == true)
		{
			Zoom = Mathf.Clamp(Zoom, ClampMin, ClampMax);
		}
	}

	protected virtual void Start()
	{
		currentZoom = Zoom;
		// defaultZoom = currentZoom;
	}

	protected virtual void LateUpdate()
	{
		// Get the fingers we want to use
		var fingers = Use.GetFingers();

		// Get the pinch ratio of these fingers
		var pinchRatio = LeanGesture.GetPinchRatio(fingers);

		pinchRatio = (pinchRatio - 1) * CameraSpeedController.zoomSensitivity + 1;
		
		Debug.Log(pinchRatio);

		// Store
		var oldPosition = transform.localPosition;

		// Make sure the zoom value is valid
		Zoom = TryClamp(Zoom);

		if (pinchRatio != 1.0f)
		{
			// Store old zoom value and then modify zoom
			var oldZoom = Zoom;

			Zoom = TryClamp(Zoom * pinchRatio);

			// Zoom relative to a point on screen?
			if (Relative == true)
			{
				var screenPoint = default(Vector2);

				if (LeanGesture.TryGetScreenCenter(fingers, ref screenPoint) == true)
				{
					// Derive actual pinchRatio from the zoom delta (it may differ with clamping)
					pinchRatio = Zoom / oldZoom;

					zoomSpeedScale = CameraSpeedController.zoomSensitivity;

					pinchRatio *= zoomSpeedScale;

					var worldPoint = ScreenDepth.Convert(screenPoint);

					transform.position = worldPoint + (transform.position - worldPoint) * pinchRatio;

					// Increment
					remainingTranslation += transform.localPosition - oldPosition;

					if (IgnoreZ == true)
					{
						remainingTranslation.z = 0.0f;
					}
				}
			}
		}

		// Get t value
		var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

		// Lerp the current value to the target one
		currentZoom = Mathf.Lerp(currentZoom, Zoom, factor);

		// Set the new zoom
		SetZoom(currentZoom);

		// Dampen remainingDelta
		var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

		// Shift this transform by the change in delta
		transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;

		// Update remainingDelta with the dampened value
		remainingTranslation = newRemainingTranslation;
	}

	protected void SetZoom(float current)
	{
		// Make sure the camera exists
		var camera = LeanTouch.GetCamera(Camera, gameObject);

		if (camera != null)
		{
			if (camera.orthographic == true)
			{
				camera.orthographicSize = current;
			}
			else
			{
				camera.fieldOfView = current;
			}
		}
		else
		{
			Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
		}
	}

	private float TryClamp(float z)
	{
		if (Clamp == true)
		{
			z = Mathf.Clamp(z, ClampMin, ClampMax);
		}

		return z;
	}

	public void ResetCamera()
	{
		Zoom = defaultZoom;
		SetZoom(defaultZoom);
	}
}