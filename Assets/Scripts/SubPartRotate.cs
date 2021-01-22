using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Wootopia;

public class SubPartRotate : MonoBehaviour
{
	public enum RotateAxis
	{
		X,
		Y,
		Z
	}

	private enum SubPartStatus
	{
		Moving,
		Rotating,
		Done
	}

	[Header("STATUS")]
	private bool isControlling;
	private SubPartStatus currStatus;

	[SerializeField]
	public bool IsDone
	{
		get { return currStatus == SubPartStatus.Done; }
	}

	[Header("MOVE")]
	public bool canMove;
	[SerializeField]
	public Vector3 offset;
	private Vector3 localPos;
	private Vector3 screenPosition;
	private float cameraOffset;
	private Vector3 mouseStartPoint;
	private Vector3 mouseWorldPoint;

	[Header("ROTATE")]

	public bool canRotate;
	public RotateAxis rotateAxis;
	public Vector3 rotateOffset;
	public Vector3 localAngle;
	public int rotateAngle = 15;
	private GameObject rotateHint;
	private Vector3 axisNormal;

	[Header("DEPENDENCY")]
	public List<WoodJoint> conflictJointList = new List<WoodJoint>();

	[Header("AUDIO")]
	public AudioClip enabledClip;
	public AudioClip finishedClip;
	private new AudioSource audio;
	private int currentUnit;
	[Header("EVENTS")]
	public UnityEvent onEnable;
	public UnityEvent onFinish;

	private void OnEnable()
	{
		onEnable.Invoke();
	}
	void Start()
	{
		currStatus = SubPartStatus.Rotating;
		audio = GameObject.Find("AudioSource").GetComponent<AudioSource>();

		if (audio && enabledClip) { audio.PlayOneShot(enabledClip); }

		localPos = transform.localPosition;
		localAngle = transform.localEulerAngles;
		rotateHint = transform.Find("RotateHint").gameObject;
		if (canMove)
		{
			currStatus = SubPartStatus.Moving;
			transform.localPosition = localPos + offset;

		}
		if (canRotate)
		{
			transform.localEulerAngles = localAngle + rotateOffset;
		}
	}

	void Update()
	{
		if ((canMove || canRotate) && Input.GetMouseButtonDown(0))
		{
			if (CheckMouseOnSelf())
			{
				screenPosition = Camera.main.WorldToScreenPoint(transform.position);
				cameraOffset = Vector3.Magnitude(transform.position - Camera.main.transform.position);
				mouseStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				isControlling = true;
				currentUnit = 0;
			}
		}

		if (isControlling && Input.GetMouseButtonUp(0))
		{
			isControlling = false;
			if (currStatus == SubPartStatus.Moving)
			{
				if (transform.localPosition != localPos)
				{
					transform.localPosition = localPos + offset;
				}
			}
			if (currStatus == SubPartStatus.Rotating)
			{
				if (transform.localEulerAngles != localAngle)
				{
					transform.localEulerAngles = localAngle + rotateOffset;
				}
			}
		}

		if (isControlling)
		{
			if (currStatus == SubPartStatus.Rotating)
			{
				mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				Vector3 camCenterWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cameraOffset));
				Vector3 b = camCenterWorldPos - Camera.main.transform.position;
				float bMag = b.magnitude;
				Vector3 a = transform.position - Camera.main.transform.position;
				Vector3 a1 = (Vector3.Dot(a, b) / (bMag * bMag)) * b;
				Vector3 a2 = a - a1;
				Vector3 objScreenPos = camCenterWorldPos + (a2 * (bMag / a1.magnitude));
				Vector3 c = mouseWorldPoint - mouseStartPoint;
				Vector3 d = mouseStartPoint - objScreenPos;
				Vector3 e = c - d * (Vector3.Dot(c, d) / (d.magnitude * d.magnitude));
				Debug.DrawLine(objScreenPos, transform.position, Color.red);
				Debug.DrawLine(mouseStartPoint, mouseWorldPoint, Color.blue);
				Debug.DrawLine(mouseStartPoint, mouseStartPoint + e, Color.green);
				Debug.DrawLine(mouseStartPoint + e, objScreenPos, Color.yellow);
				int mouseUnit = Mathf.RoundToInt(e.magnitude / (Screen.height * 0.0005f));
				Debug.Log(e.magnitude);
				int unitChange = 0;
				if (mouseUnit != currentUnit)
				{
					unitChange = mouseUnit - currentUnit;
					currentUnit = mouseUnit;
				}
				Debug.Log(unitChange + " " + currentUnit);
				Vector3 res = Vector3.Cross(b, d).normalized * 10;
				float mag = Mathf.Sign(Vector3.Dot((transform.position - Camera.main.transform.position), axisNormal)) * Mathf.Sign(Vector3.Dot(e, res));
				switch (rotateAxis)
				{
				case RotateAxis.X:
					transform.Rotate(unitChange * rotateAngle * mag, 0, 0, Space.Self);
					break;
				case RotateAxis.Y:
					transform.Rotate(0, unitChange * rotateAngle * mag, 0, Space.Self);
					break;
				case RotateAxis.Z:
					Debug.Log("Z" + unitChange);
					transform.Rotate(0, 0, unitChange * rotateAngle * mag, Space.Self);
					break;
				}
				if (transform.localEulerAngles == localAngle)
				{
					Done();
				}
			}
			else if (currStatus == SubPartStatus.Moving)
			{
				mouseWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraOffset));
				Vector3 pointOnLine = Vector3.Project(mouseWorldPoint - mouseStartPoint, offset);
				float percentage = Vector3.Magnitude(pointOnLine) / Vector3.Magnitude(offset);
				transform.localPosition = Vector3.Lerp(localPos + offset, localPos, percentage * -Mathf.Sign(Vector3.Dot(pointOnLine, offset)));

				if (transform.localPosition == localPos)
				{
					CompleteMove();
				}
			}
		}
	}

	public void CompleteMove()
	{
		canMove = false;
		if (canRotate)
		{
			currStatus = SubPartStatus.Rotating;
			switch (rotateAxis)
			{
			case RotateAxis.X:
				rotateHint.transform.Find("rotateX").gameObject.SetActive(true);
				axisNormal = transform.right;
				break;
			case RotateAxis.Y:
				rotateHint.transform.Find("rotateY").gameObject.SetActive(true);
				axisNormal = transform.up;
				break;
			case RotateAxis.Z:
				rotateHint.transform.Find("rotateZ").gameObject.SetActive(true);
				axisNormal = transform.forward;
				break;
			}
		}
		else
		{
			Done();
		}
	}

	public void Done()
	{
		if (currStatus == SubPartStatus.Done) { return; }
		if (audio && finishedClip) { audio.PlayOneShot(finishedClip); }
		currStatus = SubPartStatus.Done;
		isControlling = false;

		foreach (var joint in conflictJointList)
		{
			joint.enabled = false;
		}

		MainCameraSwitch.Instance.SwitchOn();
		TransitionCamera.Instance.Release();
		TabListManager.Instance.UpdateTabList();

		// UndoManager.Instance.SaveAction(ActionType.Assemble, this, joint2Disable);

		ControlManager.Instance.SetCurrentItem(null);
		CameraManager.Instance.SetRotateCamera(true);
		onFinish.Invoke();
	}

	bool CheckMouseOnSelf()
	{
		Vector3 mousePos = Input.mousePosition;

		RaycastHit hitInfo;

		Ray ray = Camera.main.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
		{
			if (hitInfo.collider.transform == transform || hitInfo.collider.transform.parent == transform || hitInfo.collider.transform.parent.parent == transform)
			{
				return true;
			}
		}
		return false;
	}
}