using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
	/// <summary>The camera that will be used during calculations.
	/// None = MainCamera.</summary>
	public Camera Camera;

	public GameObject Target;

	/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
	public LeanScreenDepth screenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

	/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
	public LeanFingerFilter zoomFinger = new LeanFingerFilter(true);

	public float dampening = 10.0f;

	public float zoomSpeedScale = 1f;

	[HideInInspector]
	[SerializeField]
	private Vector3 remainingDelta;
	protected virtual void Update()
	{
		var fingers = zoomFinger.GetFingers();
		if (fingers.Count > 1)
		{

			var scale = LeanGesture.GetPinchScale(fingers);

			scale = (scale - 1.0f);

			Vector3 vector = Vector3.forward * scale;

			var finalTransform = Target != null ? Target.transform : transform;

			vector = finalTransform.TransformVector(vector);

			vector *= Time.deltaTime;

			remainingDelta += (vector * zoomSpeedScale);

			var factor = LeanTouch.GetDampenFactor(dampening, Time.deltaTime);
			var newDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

			finalTransform.position += remainingDelta - newDelta;

			remainingDelta = newDelta;

		}

	}
	private void OnApplicationQuit()
	{
		Debug.Log("Zoom Speed Scale: " + zoomSpeedScale);
	}
	public void UpdateSpeed(float speed)
	{
		zoomSpeedScale = speed;
	}

}