using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour
{
	public enum MoveMethod
	{
		Translate,
		Velocity,
	}
	public enum CtrlMode
	{
		None,
		Move,
		Rotate,
	}
	public enum Axis
	{
		X,
		Y,
		Z,
		None
	}
	public MoveMethod moveMethod = MoveMethod.Translate;
	public CtrlMode currentCtrlMode;
	private Axis currentAxis;
	private Vector3 axisNormal;
	Vector3 screenPosition;
	Vector3 offset;
	Vector3 startPoint;
	int currentUnit;
	public int rotateAngle = 15;
	public RotateHintRing currentHintRing;

	private int screenDivision = 200;
	public bool isControlling
	{
		get;
		private set;
	}
	private GameObject rotateHint;

	[HideInInspector]
	public Lean.Touch.LeanFingerFilter fingerFilter;

	void Start()
	{
		currentCtrlMode = currentCtrlMode == CtrlMode.None ? CtrlMode.Move : currentCtrlMode;
		ControlManager.Instance.SwitchControlMode(currentCtrlMode);
	}

	void Update()
	{
#region Modified
		var fingers = fingerFilter.GetFingers();
		if (fingers.Count != 1)
		{
			return;
		}

		Vector3 mousePos = Lean.Touch.LeanGesture.GetLastScreenCenter(fingers);

		Vector3 camPos = Camera.main.transform.position;
		Vector3 camToObj = transform.position - camPos;
		mousePos.z = 100;
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 camCenterWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 100));
		Vector3 b = camCenterWorldPos - Camera.main.transform.position;
		float bMag = b.magnitude;
		Vector3 a = camToObj;
		Vector3 a1 = (Vector3.Dot(a, b) / (bMag * bMag)) * b;
		Vector3 a2 = a - a1;
		Vector3 objScreenPos = camCenterWorldPos + (a2 * (bMag / a1.magnitude));

		//Vector3 mousePos = Input.mousePosition;
#endregion

		if (Input.GetMouseButtonDown(0))
		{
			if (CheckMouseOnSelf())
			{
				isControlling = true;
				CameraManager.Instance.SetRotateCamera(false);
				screenPosition = Camera.main.WorldToScreenPoint(transform.position);
				offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, screenPosition.z));
				startPoint = Input.mousePosition;
				currentUnit = 0;
				ControlManager.Instance.ToggleControlPanelType(CtrlPanelType.Object);
				LocationHintBox.Instance.ShowAxisHintBox();
				LocationHintBox.Instance.targetTransform2 = transform;
				LocationHintBox.Instance.targetPos1 = transform.position;
			}
		}

		if (isControlling && Input.GetMouseButtonUp(0))
		{
			isControlling = false;
			CameraManager.Instance.SetRotateCamera(true);
			LocationHintBox.Instance.HideAxisHintBox();
		}

		if (isControlling && currentCtrlMode == CtrlMode.Move)
		{
			Vector3 currentScreenSpace = new Vector3(mousePos.x, mousePos.y, screenPosition.z);
			Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
			switch (moveMethod)
			{
			case MoveMethod.Translate:
				transform.position = currentPosition;
				break;

			case MoveMethod.Velocity:
				GetComponent<Rigidbody>().velocity = (currentPosition - transform.position) * 10;
				break;
			}
		}

		if (isControlling && currentCtrlMode == CtrlMode.Rotate)
		{
			// mousePos.z = Camera.main.transform.position.y - transform.position.y;
			// Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
			// Vector3 camToObj = transform.position - Camera.main.transform.position;
			// float mouseAngle = Mathf.RoundToInt((Mathf.Atan2(mouseWorldPos.x - transform.position.x, mouseWorldPos.z - transform.position.z) * Mathf.Rad2Deg) / rotateAngle) * rotateAngle;

			Vector3 startPointWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(startPoint.x, startPoint.y, 100));
			Vector3 c = mouseWorldPos - startPointWorldPos;
			Vector3 d = startPointWorldPos - objScreenPos;
			Vector3 e = c - d * (Vector3.Dot(c, d) / (d.magnitude * d.magnitude));
			// Debug.DrawLine(objScreenPos, startPointWorldPos, Color.red);
			// Debug.DrawLine(startPointWorldPos, mouseWorldPos, Color.blue);
			// Debug.DrawLine(startPointWorldPos, startPointWorldPos + e, Color.green);
			// Debug.DrawLine(startPointWorldPos + e, objScreenPos, Color.yellow);

			int mouseUnit = Mathf.RoundToInt(e.magnitude / (Screen.height / screenDivision));
			int unitChange = 0;
			if (mouseUnit != currentUnit)
			{
				unitChange = mouseUnit - currentUnit;
				currentUnit = mouseUnit;
			}
			Vector3 res = Vector3.Cross(b, d).normalized * 10;
			float mag = Mathf.Sign(Vector3.Dot(camToObj, axisNormal)) * Mathf.Sign(Vector3.Dot(e, res));
			// Debug.DrawLine(startPointWorldPos, startPointWorldPos + res, Color.magenta);
			// Debug.Log(mouseUnit + " " + Mathf.Sign(Vector3.Dot(camToObj, axisNormal)) + " " + Mathf.Sign(Vector3.Dot(e, res)));
			switch (currentAxis)
			{
			case Axis.X:
				transform.Rotate(unitChange * rotateAngle * mag, 0, 0, Space.Self);
				currentHintRing.currentDir = transform.up;
				break;
			case Axis.Y:
				transform.Rotate(0, unitChange * rotateAngle * mag, 0, Space.Self);
				currentHintRing.currentDir = transform.forward;
				break;
			case Axis.Z:
				transform.Rotate(0, 0, unitChange * rotateAngle * mag, Space.Self);
				currentHintRing.currentDir = transform.right;
				break;
            }
        }
	}

	bool CheckMouseOnSelf()
	{
#region Modified
		var fingers = fingerFilter.GetFingers();
		if (fingers.Count != 1)
		{
			return false;
		}

		foreach (var finger in fingers)
		{
			if (finger.StartedOverGui)
			{
				Debug.Log("GUI");
				return false;
			}
		}

		Vector3 mousePos = Lean.Touch.LeanGesture.GetLastScreenCenter(fingers);
		//Vector3 mousePos = Input.mousePosition;
#endregion

		RaycastHit hitInfo;

		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray.origin, ray.direction * 10, out hitInfo))
		{
			if (hitInfo.collider.transform == transform || hitInfo.collider.transform.parent.parent == transform)
			{
				Debug.Log("Hit Collider Name: " + hitInfo.collider.name);
				switch (hitInfo.collider.name)
				{
				case "rotateX":
					currentAxis = Axis.X;
					axisNormal = hitInfo.collider.transform.right;
					currentHintRing = hitInfo.collider.GetComponent<RotateHintRing>();
					currentHintRing.startDir = transform.up;
					currentHintRing.rotateAxis = transform.right;
					break;
				case "rotateY":
					currentAxis = Axis.Y;
					axisNormal = hitInfo.collider.transform.up;
					currentHintRing = hitInfo.collider.GetComponent<RotateHintRing>();
					currentHintRing.startDir = transform.forward;
					currentHintRing.rotateAxis = transform.up;
					break;
				case "rotateZ":
					currentAxis = Axis.Z;
					axisNormal = hitInfo.collider.transform.forward;
					currentHintRing = hitInfo.collider.GetComponent<RotateHintRing>();
					currentHintRing.startDir = transform.right;
					currentHintRing.rotateAxis = transform.forward;
					break;
				default:
					currentAxis = Axis.None;
					currentHintRing = null;
					break;
				};
				ControlManager.Instance.SwitchControlMode(currentCtrlMode == CtrlMode.None ? CtrlMode.Move : currentCtrlMode);
				Debug.Log("Switch Control Mode: " + currentCtrlMode);
				LocationHintBox.Instance.pos1Offset = hitInfo.point - transform.position;
				LocationHintBox.Instance.pos2Offset = hitInfo.point - transform.position;
				return true;
			}
			else
			{
				ControlManager.Instance.ToggleControlPanelType(CtrlPanelType.World);
			}
		}

		currentAxis = Axis.None;
		return false;
	}

	public void SetActiveRotateHint(bool flag)
	{
		transform.Find("RotateHint").gameObject.SetActive(flag);

	}
}