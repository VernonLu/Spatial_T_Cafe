using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
	private static TransitionCamera instance;
	public static TransitionCamera Instance
	{
		get { return instance; }
	}
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

	[SerializeField]
	protected bool isActive;
	[SerializeField]
	protected Transform mainCameraPivot;
	[SerializeField]
	protected Transform targetTransform;

	protected Vector3 targetPosition;
	protected Quaternion targetRotation;

	public float speed = 1.0f;
	/*
	public float moveSpeed;
	public float rotateSpeed;
	*/

	public void SetTransform(Transform target)
	{
		targetTransform = target;
		// transform.position = target.position;
		// transform.rotation = target.rotation;
		isActive = true;
	}

	public void Release()
	{
		isActive = false;
		targetTransform = null;
	}
	private void UpdateTransform()
	{
		Vector3 currentPosition = transform.position;
		Quaternion currentRotation = transform.rotation;

		Vector3 targetPosition = (null == targetTransform) ? mainCameraPivot.position : targetTransform.position;
		Quaternion targetRotation = (null == targetTransform) ? mainCameraPivot.rotation : targetTransform.rotation;

		currentPosition = Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
		currentRotation = Quaternion.Lerp(currentRotation, targetRotation, speed * Time.deltaTime);

		transform.position = currentPosition;
		transform.rotation = currentRotation;
	}
	void Start()
	{

	}

	void Update()
	{
		// if (!isActive)
		// {
		// 	transform.position = mainCameraPivot.position;
		// 	transform.rotation = mainCameraPivot.rotation;
		// }
		UpdateTransform();

	}
}