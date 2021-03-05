using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrbitCamera : MonoBehaviour
{
	public enum Status
	{
		Disabled,
		Moving,
		Rotating,
	}

	public Camera Cam;

	public Status status = Status.Disabled;

	public Camera followCamera;

	//	相机初始位置信息
	private float defaultZoom;

	private float defaultRadius;

	private float defaultY;

	//	像机参数
	[Header("CAMERA PROPERTIES")]

	public float moveDuration;

	public AnimationCurve speedCurve;

	[Tooltip("Unit : degree per second")]
	public float rotateSpeed = 45f;

	public Transform pivot;

	private Vector3 moveStartPos;
	private Vector3 moveEndPos;

	private float moveStartZoom;

	private Quaternion moveStartRotation;
	private Quaternion moveEndRotation;
	
	private float timer;

	public UnityEvent onStartMove;

	public UnityEvent onStartRotate;

	private bool finished = false;
	public UnityEvent onFinishRotate;



	void Start()
	{
		if (status == Status.Disabled)
		{
			Cam.enabled = false;
		}
		
		Cam = Cam ?? GetComponent<Camera>();
		if (null == Cam)
		{
			Debug.LogError("Orbit Camera isn't assigned");
			this.enabled = false;
		}

		defaultZoom = Cam.fieldOfView;

		defaultY = Cam.transform.position.y;

		Vector3 pos = Cam.transform.position;
		pos.y = 0;
		defaultRadius = Vector3.Magnitude(pos);
		//	S
	}


	void LateUpdate()
	{
		switch (status)
		{
			case Status.Disabled:
				//	关闭状态下像机始终与当前像机同步
				// followCamera = Camera.current;
				if (followCamera)
				{
					transform.position = followCamera.transform.position;
					transform.rotation = followCamera.transform.rotation;
					Cam.fieldOfView = followCamera.fieldOfView;
				}
				break;
			case Status.Moving:
				//	将相机移至旋转轨道, 并注视pivot
				timer += Time.deltaTime;
				float factor = Mathf.Clamp(speedCurve.Evaluate(timer / moveDuration), 0, 1);
				Cam.transform.position = Vector3.Lerp(moveStartPos, moveEndPos, factor);
				//	Cam.transform.LookAt(pivot);
				Cam.transform.rotation = Quaternion.Lerp(moveStartRotation, moveEndRotation, factor);	
				Cam.fieldOfView = Mathf.Lerp(moveStartZoom, defaultZoom, factor);
				if (factor >= 1)
				{
					timer = 0;
					status = Status.Rotating;
					onStartRotate.Invoke();
				}
				break;

			case Status.Rotating:
				//	像机沿轨道旋转
				timer += Time.deltaTime;
				Cam.transform.RotateAround(pivot.position, Vector3.up, rotateSpeed * Time.deltaTime);
				if (!finished && timer >= 360f / rotateSpeed)
				{
					onFinishRotate.Invoke();
					finished = true;
				}
				break;
		}

	}

	public void SetActive(bool value)
	{
		if (status == Status.Disabled)
		{
			if (value)
			{
				status = Status.Moving;
				moveStartZoom = Cam.fieldOfView;

				Vector3 currentPos = moveStartPos = Cam.transform.position;

				currentPos.y = 0;

				float k = defaultRadius / currentPos.magnitude;
				moveEndPos = k * currentPos;
				moveEndPos.y = defaultY;

				Vector3 lookAtVector = pivot.position - moveEndPos;

				moveStartRotation = Cam.transform.rotation;
				moveEndRotation = Quaternion.LookRotation(lookAtVector);
				timer = 0;
				Cam.enabled = true;
				onStartMove.Invoke();
			}
		}
		else
		{
			if (!value)
			{
				status = Status.Disabled;
				Cam.enabled = false;
			}
		}
	}

	void OnDrawGizmos()
	{
		Vector3 center = pivot.position;
		center.y = defaultY;

		Gizmos.color = new Color(1, 1, 1, 0.01f);
		// Gizmos.DrawWireSphere(center, defaultRadius);
		UnityEditor.Handles.DrawSolidArc(center,Vector3.up,Vector3.forward,360, defaultRadius);
	}
}
