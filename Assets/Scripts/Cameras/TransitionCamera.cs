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
	protected Transform targetTransform;

	protected Vector3 targetPosition;
	protected Quaternion targetRotation;
	/*
	public float moveSpeed;
	public float rotateSpeed;
	*/

	public void SetTransform(Transform target)
	{
		targetTransform = target;
		transform.position = target.position;
		transform.rotation = target.rotation;
		isActive = true;
	}

	public void Release()
	{
		isActive = false;
	}
	/*
	    private void UpdateTarget()
	    {
	        if (!isActive)
	        {
	            if (null == mainCameraPivot) { return; }
	            targetPosition = mainCameraPivot.position;
	            targetRotation = mainCameraPivot.rotation;
	            return;
	        }
	        else
	        {
	            if (null == targetTransform) { return; }
	            targetPosition = mainCameraPivot.position;
	            targetRotation = mainCameraPivot.rotation;
	            return;
	        }
	    }
	*/
	void Start()
	{

	}

	void Update()
	{
		if (!isActive)
		{
			transform.position = mainCameraPivot.position;
			transform.rotation = mainCameraPivot.rotation;
		}

	}
}