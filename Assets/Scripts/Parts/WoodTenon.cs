using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTenon : MonoBehaviour
{
	public string ID;

	public enum DebugType
	{
		None,
		Always,
		OnSelected,
	}

	[Header("DEBUG")]
	public DebugType debugType = DebugType.Always;

	public float rayLength = 1;
	private void OnDrawGizmos()
	{

		if (debugType != DebugType.Always) { return; }
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.up);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.forward);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.right);
	}

	private void OnDrawGizmosSelected()
	{
		if (debugType != DebugType.OnSelected) { return; }
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.up);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.forward);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + rayLength * transform.right);
	}
	//[SerializeField]
	//private Vector3 positionOffset;
	//[SerializeField]
	//private Vector3 angleOffset;

	//public Vector3 TenonPosition {
	//    get
	//    {
	//        return transform.position + positionOffset;
	//    }
	//}
	//public Vector3 TenonAngle
	//{
	//    get
	//    {
	//        return transform.rotation.eulerAngles + angleOffset;
	//    }
	//}

	//private void OnDrawGizmos()
	//{
	//    Gizmos.matrix = transform.localToWorldMatrix;
	//    Vector3 center = positionOffset;
	//    Gizmos.color = Color.red;
	//    Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.right / 10 + center);
	//    Gizmos.color = Color.green;
	//    Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.up / 10 + center);
	//    Gizmos.color = Color.blue;
	//    Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.forward / 10 + center);
	//}
}