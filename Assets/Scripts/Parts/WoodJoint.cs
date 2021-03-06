﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodJoint : MonoBehaviour
{
	[Header("PROPERTY")]
	public string ID;
	public string targetID;
	//position divided by 10 for more precision 
	//don't change in realtime
	public ToleranceDomin toleranceDomin;

	[Serializable]
	public class ToleranceDomin
	{
		public Vector3 tolerancePositionStart;
		public Vector3 tolerancePositionEnd;
		public Vector3 toleranceEularAngleStart;
		public Vector3 toleranceEularAngleEnd;
	}

	[Header("DEPENDENCY")]
	public WoodTenon targetTenon;
	public Transform cameraFocusPivot;

	// Activate sub part when part is assembled
	public GameObject subPart;

	public List<SubPartRotate> requiredSubPartList = new List<SubPartRotate>();

	[Header("DEBUG")]
	[SerializeField]
	protected bool isActive = false;
	[SerializeField]
	private bool showPositionDebugInfo = false;
	[SerializeField]
	private bool showAngleDebugInfo = false;

	void Start() { }

	private void Update()
	{
		//need optimize
		if (!CheckStatus()) { return; }
		Item currentItem = ControlManager.Instance.currentItem;

		if (currentItem?.tenonList.Count > 0)
		{
			foreach (var tenon in currentItem.tenonList)
			{
				if (tenon != null && targetID == tenon.ID)
				{
					targetTenon = tenon;
					if (!IsTenonInCorrectPose()) { continue; }
					// Debug.Log("Paired");
					targetTenon.transform.parent.TryGetComponent(out Item item);
					if (item.isAssembled) { return; }
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
					break;
				}
			}
		}
	}

	/// <summary>
	/// Check All dependenies are assembled
	/// </summary>
	/// <returns></returns>
	private bool CheckStatus()
	{
		bool res = true;

		foreach (var subPart in requiredSubPartList)
		{
			if (!subPart.IsDone)
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
		Vector3 startAngle = Vector3.Min(toleranceDomin.toleranceEularAngleStart, toleranceDomin.toleranceEularAngleEnd);
		Vector3 endAngle = Vector3.Max(toleranceDomin.toleranceEularAngleStart, toleranceDomin.toleranceEularAngleEnd);
		Vector3 deltaAngle = targetTenon.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
		deltaAngle.x = deltaAngle.x > 180 ? deltaAngle.x - 360 : deltaAngle.x;
		deltaAngle.y = deltaAngle.y > 180 ? deltaAngle.y - 360 : deltaAngle.y;
		deltaAngle.z = deltaAngle.z > 180 ? deltaAngle.z - 360 : deltaAngle.z;
		bool isAngleCorrect = Vector3.Min(startAngle, deltaAngle) == startAngle && Vector3.Max(deltaAngle, endAngle) == endAngle;
		//return if target in correct pose
		if (showPositionDebugInfo) { print("pos: " + isPositionCorrect); }
		if (showAngleDebugInfo) { print("angle: " + isAngleCorrect + ", " + deltaAngle); }
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
	// 	private void OnDrawGizmos()
	// 	{
	// 		Gizmos.matrix = transform.localToWorldMatrix;
	// #region draw box
	// 		Gizmos.color = Color.green;
	// 		Vector3 center = (toleranceDomin.tolerancePositionStart + toleranceDomin.tolerancePositionEnd) / 2 / 10;
	// 		Vector3 size = (toleranceDomin.tolerancePositionStart - toleranceDomin.tolerancePositionEnd) / 10;
	// 		center = transform.rotation * center;
	// 		Gizmos.DrawWireCube(center, size);
	// #endregion
	// #region draw angle
	// 		Gizmos.color = Color.red;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(toleranceDomin.toleranceEularAngleStart.x, 0, 0) * Vector3.up);
	// 		Gizmos.color = Color.green;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, toleranceDomin.toleranceEularAngleStart.y, 0) * Vector3.forward);
	// 		Gizmos.color = Color.blue;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, toleranceDomin.toleranceEularAngleStart.z) * Vector3.right);
	// 		Gizmos.color = Color.red;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(toleranceDomin.toleranceEularAngleEnd.x, 0, 0) * Vector3.up);
	// 		Gizmos.color = Color.green;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, toleranceDomin.toleranceEularAngleEnd.y, 0) * Vector3.forward);
	// 		Gizmos.color = Color.blue;
	// 		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, toleranceDomin.toleranceEularAngleEnd.z) * Vector3.right);
	// #endregion
	// 	}
	private void OnDrawGizmos()
	{
#region draw box
		Gizmos.color = Color.green;
		Vector3 center = (toleranceDomin.tolerancePositionStart + toleranceDomin.tolerancePositionEnd) / 2 / 10;
		Vector3 size = (toleranceDomin.tolerancePositionStart - toleranceDomin.tolerancePositionEnd) / 10;
		//center = transform.rotation * center;
		center += transform.position;
		Gizmos.DrawWireCube(center, size);
#endregion
		Gizmos.matrix = transform.localToWorldMatrix;
#region draw angle
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(toleranceDomin.toleranceEularAngleStart.x, 0, 0) * Vector3.up);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, toleranceDomin.toleranceEularAngleStart.y, 0) * Vector3.forward);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, toleranceDomin.toleranceEularAngleStart.z) * Vector3.right);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(toleranceDomin.toleranceEularAngleEnd.x, 0, 0) * Vector3.up);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, toleranceDomin.toleranceEularAngleEnd.y, 0) * Vector3.forward);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, toleranceDomin.toleranceEularAngleEnd.z) * Vector3.right);

#if UNITY_EDITOR
		/*Init variables*/
		Vector3 arcCenter = transform.position;
		Vector3 arcNormal = Vector3.zero;
		Vector3 arcFrom = Vector3.zero;
		float arcAngle = 0;
		float arcRadius = 1f;

		/* x */
		UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
		arcNormal = transform.right;
		arcFrom = Quaternion.AngleAxis(toleranceDomin.toleranceEularAngleStart.x, transform.right) * transform.up;
		arcAngle = toleranceDomin.toleranceEularAngleEnd.x - toleranceDomin.toleranceEularAngleStart.x;
		UnityEditor.Handles.DrawSolidArc(arcCenter, arcNormal, arcFrom, arcAngle, arcRadius);

		/* y */
		UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
		arcNormal = transform.up;
		arcFrom = Quaternion.AngleAxis(toleranceDomin.toleranceEularAngleStart.y, transform.up) * transform.forward;
		arcAngle = toleranceDomin.toleranceEularAngleEnd.y - toleranceDomin.toleranceEularAngleStart.y;
		UnityEditor.Handles.DrawSolidArc(arcCenter, arcNormal, arcFrom, arcAngle, arcRadius);

		/* z */
		UnityEditor.Handles.color = new Color(0, 0, 1, 0.3f);
		arcNormal = transform.forward;
		arcFrom = Quaternion.AngleAxis(toleranceDomin.toleranceEularAngleStart.z, transform.forward) * transform.right;
		arcAngle = toleranceDomin.toleranceEularAngleEnd.z - toleranceDomin.toleranceEularAngleStart.z;
		UnityEditor.Handles.DrawSolidArc(arcCenter, arcNormal, arcFrom, arcAngle, arcRadius);
#endif

#endregion
	}

}