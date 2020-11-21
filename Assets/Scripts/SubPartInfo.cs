using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPartInfo : MonoBehaviour
{
	[SerializeField]
	public Vector3 offset;
	public bool canMove;
	private Vector3 localPos;
	private bool isControlling;
	private Vector3 screenPosition;
	private float cameraOffset;
	private Vector3 mouseStartPoint;
	private Vector3 mouseWorldPoint;

	public WoodJoint joint2Disable;

	public List<WoodJoint> jointList = new List<WoodJoint>();

	void Start()
	{
		localPos = transform.localPosition;
		if (canMove) transform.localPosition = localPos + offset;
	}

	void Update()
	{
		if (canMove && Input.GetMouseButtonDown(0))
		{
			if (CheckMouseOnSelf())
			{
				screenPosition = Camera.main.WorldToScreenPoint(transform.position);
				cameraOffset = Vector3.Magnitude(transform.position - Camera.main.transform.position);
				mouseStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				isControlling = true;
			}
		}

		if (isControlling && Input.GetMouseButtonUp(0))
		{
			isControlling = false;
		}

		if (isControlling)
		{
			mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
			Vector3 pointOnLine = Vector3.Project(mouseWorldPoint - mouseStartPoint, offset);
			float percentage = Vector3.Magnitude(pointOnLine) / Vector3.Magnitude(offset);
			// Debug.Log(percentage + " " + Vector3.Dot(pointOnLine, offset) + " " + offset);
			transform.localPosition = Vector3.Lerp(localPos + offset, localPos, percentage * -Mathf.Sign(Vector3.Dot(pointOnLine, offset)));
			Debug.DrawLine(mouseStartPoint, mouseWorldPoint, Color.yellow);
			Debug.DrawLine(mouseStartPoint, mouseStartPoint + pointOnLine, Color.blue);
			Debug.DrawLine(localPos, localPos + offset, Color.magenta);
			Debug.DrawLine(mouseStartPoint, mouseStartPoint + new Vector3(0, 100, 0), Color.red);
			Debug.DrawLine(mouseWorldPoint, mouseWorldPoint + new Vector3(0, 100, 0), Color.green);

			if (transform.localPosition == localPos)
			{
				CompleteMove();
			}
		}
	}
	public void CompleteMove()
	{
		canMove = false;
		isControlling = false;
		foreach (var joint in jointList)
		{
			joint.enabled = true;
		}
		if (joint2Disable)
		{
			joint2Disable.enabled = false;
		}
		MainCameraSwitch.Instance.SwitchOn();
		TransitionCamera.Instance.Release();
		TabListManager.Instance.UpdateTabList();

		ControlManager.Instance.SetCurrentItem(null);
		CameraManager.Instance.SetRotateCamera(true);

		ControlManager.Instance.SaveAction(SavedActionType.Assemble);
	}

	bool CheckMouseOnSelf()
	{
		Vector3 mousePos = Input.mousePosition;

		RaycastHit hitInfo;

		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
		{
			if (hitInfo.collider.transform == transform)
			{
				// Debug.Log(hitInfo.collider.name);
				return true;
			}
		}
		return false;
	}
}