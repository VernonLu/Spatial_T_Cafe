using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodJoint : MonoBehaviour
{
	public string ID;
	public string targetID;
	public WoodTenon targetTenon;
	//position divided by 10 for more precision 
	//don't change in realtime
	public ToleranceDomin toleranceDomin;
	[SerializeField]
	protected bool isActive = false;

	[Serializable]
	public class ToleranceDomin
	{
		public Vector3 tolerancePositionStart;
		public Vector3 tolerancePositionEnd;
		//public Vector3 toleranceEularAngleStart;
		public Vector3 toleranceEularAngleEnd;
	}

	public Transform cameraFocusPivot;

	// Activate sub part when
	public GameObject subPart;

	public List<SubPartInfo> dependencies = new List<SubPartInfo>();

	private new Collider collider;

	void Start()
	{
		collider = GetComponent<Collider>();
	}

	private void Update()
	{
		//need optimize
		if (!CheckStatus()) { return; }
		Item currentItem = ControlManager.Instance.currentItem;
		WoodTenon currentTenon = currentItem?.tenonList.Count > 0 ? currentItem.tenonList[0] : null;
		//if(currentTenon == null)
		//      {
		//	return;
		//      }
		if (currentTenon != null && targetID == currentTenon.ID)
		{
			targetTenon = currentTenon;
		}
		if (IsTenonInCorrectPose())
		{
			// Debug.Log("Paired");
			targetTenon.transform.parent.TryGetComponent(out Item item);
			if (item.isAssembled)
			{
				return;
			}
			item.isAssembled = true;
			targetTenon.transform.parent.gameObject.SetActive(false);

			// Move Camera to target position and rotation
			MainCameraSwitch.Instance.SwitchOff();
			TransitionCamera.Instance.SetTransform(cameraFocusPivot);

			// Enable sub part control
			subPart.SetActive(true);
			// Disable currentItem
			ControlManager.Instance.currentItem?.gameObject.SetActive(false);
			LocationHintBox.Instance.HideAxisHintBox();
		}
	}

	/// <summary>
	/// Check All dependenies are assembled
	/// </summary>
	/// <returns></returns>
	private bool CheckStatus()
	{
		bool res = true;
		foreach (var subPart in dependencies)
		{
			if (subPart.canMove)
			{
				res = false;
			}
		}
		return res;
	}

	private bool IsTenonInCorrectPose()
	{
		if (targetTenon == null)
		{
			return false;
		}
		//calculate if target in tolerance position
		Vector3 startPos = Vector3.Min(toleranceDomin.tolerancePositionStart, toleranceDomin.tolerancePositionEnd) / 10; //formlize
		Vector3 endPos = Vector3.Max(toleranceDomin.tolerancePositionStart, toleranceDomin.tolerancePositionEnd) / 10; //formlize
		Vector3 deltaPos = targetTenon.transform.position - transform.position;
		bool isPositionCorrect = Vector3.Min(startPos, deltaPos) == startPos && Vector3.Max(deltaPos, endPos) == endPos;
		//calculate if target in tolerance angle
		//Vector3 startAngle = Vector3.Min(toleranceDomin.toleranceEularAngleStart, toleranceDomin.toleranceEularAngleEnd);
		Vector3 endAngle = Vector3.Max(-toleranceDomin.toleranceEularAngleEnd, toleranceDomin.toleranceEularAngleEnd);
		Vector3 deltaAngle = targetTenon.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
		deltaAngle.x = Math.Abs(deltaAngle.x);
		deltaAngle.y = Math.Abs(deltaAngle.y);
		deltaAngle.z = Math.Abs(deltaAngle.z);
		bool isAngleCorrect = /*Vector3.Min(startAngle, targetAngle) == startAngle && */ Vector3.Max(deltaAngle, endAngle) == endAngle;
		//return if target in correct pose
		//print("pos: " + isPositionCorrect);
		// print("angle: " + isAngleCorrect + ", " + deltaAngle);
		// print("targetAngle: " + targetTenon.transform.rotation.eulerAngles + "currentAngle: " + transform.rotation.eulerAngles);
		return isPositionCorrect && isAngleCorrect;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isActive) { return; }

		//// Debug.Log("Trigger Enter");
		//other.gameObject.TryGetComponent(out WoodJoint otherJoint);
		//if (null == otherJoint) { return; }
		//if (targetID != otherJoint.ID) { return; }

		//// Debug.Log("Paired");
		//other.transform.parent.TryGetComponent(out Item item);
		//item.isAssembled = true;
		//other.transform.parent.gameObject.SetActive(false);

		//// Move Camera to target position and rotation
		//MainCameraSwitch.Instance.SwitchOff();
		//TransitionCamera.Instance.SetTransform(cameraFocusPivot);

		//// Enable sub part control
		//subPart.SetActive(true);
		//// Disable currentItem
		//ControlManager.Instance.currentItem.gameObject.SetActive(false);

	}

	private void OnDrawGizmos()
	{
		//Gizmos.color = Color.red;
		//Gizmos.DrawLine(transform.position, transform.position + transform.right / 10);
		//Gizmos.color = Color.green;
		//Gizmos.DrawLine(transform.position, transform.position + transform.up / 10);
		//Gizmos.color = Color.blue;
		//Gizmos.DrawLine(transform.position, transform.position + transform.forward / 10); 
		//convert local coord to world
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.green;
		Vector3 center = (toleranceDomin.tolerancePositionStart + toleranceDomin.tolerancePositionEnd) / 2 / 10;
		Gizmos.DrawWireCube(center, (toleranceDomin.tolerancePositionStart - toleranceDomin.tolerancePositionEnd) / 10);
		if (toleranceDomin.toleranceEularAngleEnd.x != 0)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(-toleranceDomin.toleranceEularAngleEnd.x, 0, 0) * Vector3.up / 10);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(toleranceDomin.toleranceEularAngleEnd.x, 0, 0) * Vector3.up / 10);
		}
		if (toleranceDomin.toleranceEularAngleEnd.y != 0)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, -toleranceDomin.toleranceEularAngleEnd.y, 0) * Vector3.forward / 10);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, toleranceDomin.toleranceEularAngleEnd.y, 0) * Vector3.forward / 10);
		}
		if (toleranceDomin.toleranceEularAngleEnd.z != 0)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, -toleranceDomin.toleranceEularAngleEnd.z) * Vector3.right / 10);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, toleranceDomin.toleranceEularAngleEnd.z) * Vector3.right / 10);
		}
	}
}