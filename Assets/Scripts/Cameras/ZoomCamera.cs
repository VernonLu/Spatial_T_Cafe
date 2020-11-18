using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

public class ZoomCamera : MonoBehaviour
{
	/// <summary>The camera that will be used during calculations.
	/// None = MainCamera.</summary>
	public Camera Camera;

	/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
	public LeanScreenDepth screenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	public LeanFingerFilter zoomFinger = new LeanFingerFilter(true);

	/// <summary>The current FOV/Size.</summary>
	public float zoom = 50.0f;

	/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
	/// -1 = Instantly change.
	/// 1 = Slowly change.
	/// 10 = Quickly change.</summary>
	public float zoomDampening = -1.0f;

	/// <summary>Limit the FOV/Size?</summary>
	public bool clamp;

	/// <summary>The minimum FOV/Size we want to zoom to.</summary>
	public float clampMin = 10.0f;

	/// <summary>The maximum FOV/Size we want to zoom to.</summary>
	public float clampMax = 60.0f;

	/// <summary>Should the zoom be performanced relative to the finger center?</summary>
	public bool relative;

	/// <summary>Ignore changes in Z translation for 2D?</summary>
	public bool ignoreZ;

	[HideInInspector]
	[SerializeField]
	private float currentZoom;

	[HideInInspector]
	[SerializeField]
	private Vector3 remainingTranslation;

	public float zoomSpeedScale = 1f;

	public void ContinuouslyZoom(float direction)
	{
		var factor = LeanTouch.GetDampenFactor(Mathf.Abs(direction), Time.deltaTime);

		if (direction > 0.0f)
		{
			zoom = Mathf.Lerp(zoom, clampMax, factor);
		}
		else if (direction <= 0.0f)
		{
			zoom = Mathf.Lerp(zoom, clampMin, factor);
		}
	}

	/// <summary>This method allows you to multiply the current <b>Zoom</b> value by the specified scale. This is useful for quickly changing the zoom from UI button clicks, or <b>LeanMouseWheel</b> scrolling.</summary>
	public void MultiplyZoom(float scale)
	{
		zoom *= scale;

		if (clamp == true)
		{
			zoom = Mathf.Clamp(zoom, clampMin, clampMax);
		}
	}

#if UNITY_EDITOR
	protected virtual void Reset()
	{
		zoomFinger.UpdateRequiredSelectable(gameObject);
	}
#endif

	protected virtual void Awake()
	{
		zoomFinger.UpdateRequiredSelectable(gameObject);
	}

	protected virtual void Start()
	{
		currentZoom = zoom;
	}

	protected virtual void LateUpdate()
	{
		Zoom();
	}
#region Zoom
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

	private void Zoom()
	{
		var fingers = zoomFinger.GetFingers();

		// Get the pinch ratio of these fingers
		var pinchRatio = LeanGesture.GetPinchRatio(fingers);

		// Store
		var oldPosition = transform.localPosition;

		// Make sure the zoom value is valid
		zoom = TryClamp(zoom);

		if (pinchRatio != 1.0f)
		{
			// Store old zoom value and then modify zoom
			var oldZoom = zoom;

			zoom = TryClamp(zoom * pinchRatio);

			// Zoom relative to a point on screen?
			if (relative == true)
			{
				var screenPoint = default(Vector2);

				if (LeanGesture.TryGetScreenCenter(fingers, ref screenPoint) == true)
				{
					// Derive actual pinchRatio from the zoom delta (it may differ with clamping)
					pinchRatio = zoom / oldZoom;

					pinchRatio *= zoomSpeedScale;

					var worldPoint = screenDepth.Convert(screenPoint);

					transform.position = worldPoint + (transform.position - worldPoint) * pinchRatio;

					// Increment
					remainingTranslation += transform.localPosition - oldPosition;

					if (ignoreZ == true)
					{
						remainingTranslation.z = 0.0f;
					}
				}
			}
		}

		// Get t value
		var factor = LeanTouch.GetDampenFactor(zoomDampening, Time.deltaTime);

		// Lerp the current value to the target one
		currentZoom = Mathf.Lerp(currentZoom, zoom, factor);

		// Set the new zoom
		SetZoom(currentZoom);

		// Dampen remainingDelta
		var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

		// Shift this transform by the change in delta
		transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;

		// Update remainingDelta with the dampened value
		remainingTranslation = newRemainingTranslation;
	}

	private float TryClamp(float z)
	{
		if (clamp == true)
		{
			z = Mathf.Clamp(z, clampMin, clampMax);
		}

		return z;
	}
#endregion

	private void OnApplicationQuit()
	{
		Debug.Log("Zoom Speed Scale: " + zoomSpeedScale);
	}
	public void UpdateSpeed(float speed)
	{
		zoomSpeedScale = speed;
	}

	public void UpdateZoom(float value)
	{
		zoom = value;
		currentZoom = value;

		// Set the new zoom
		SetZoom(currentZoom);

		Debug.Log(currentZoom);
	}
}