using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodJoint : MonoBehaviour
{
	public string ID;
	public string targetID;
	[SerializeField]
	protected bool isActive = false;

	public Transform cameraFocusPivot;

	// Activate sub part when
	public GameObject subPart;

	public List<SubPartInfo> dependencies = new List<SubPartInfo>();

	[Header("Rotation Limitation")]
	public Vector2 XRange = new Vector2(0, 360);

	private new Collider collider;

	void Start()
	{
		collider = GetComponent<Collider>();
	}

	private void Update()
	{
		if (isActive) { return; }
		isActive = CheckStatus();
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

	private void OnTriggerEnter(Collider other)
	{
		if (!isActive) { return; }

		// Debug.Log("Trigger Enter");
		other.gameObject.TryGetComponent(out WoodJoint otherJoint);
		if (null == otherJoint) { return; }
		if (targetID != otherJoint.ID) { return; }

		// Debug.Log("Paired");
		other.transform.parent.TryGetComponent(out Item item);
		item.isAssembled = true;
		other.transform.parent.gameObject.SetActive(false);

		// Move Camera to target position and rotation
		MainCameraSwitch.Instance.SwitchOff();
		TransitionCamera.Instance.SetTransform(cameraFocusPivot);

		// Enable sub part control
		subPart.SetActive(true);
		// Disable currentItem
		ControlManager.Instance.currentItem.gameObject.SetActive(false);
		collider.enabled = false;
	}
}