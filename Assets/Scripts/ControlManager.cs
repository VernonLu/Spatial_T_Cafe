using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wootopia;

[System.Serializable]
public enum CtrlPanelType
{
	Object,
	World,
}
public class ControlManager : MonoBehaviour
{

#region Singleton
	private static ControlManager instance;
	public static ControlManager Instance
	{
		get
		{
			return instance;
		}
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
#endregion

	public ObjectController controller;

	public GameObject objectControlPanel;
	public GameObject worldControlPanel;

	[Header("Move")]
	public Toggle moveButton;

	public GameObject moveHint;

	[Header("Rotate")]
	public Toggle rotateButton;

	public Item currentItem;

	void Start()
	{

	}

	void Update()
	{
		if (controller && controller.currentCtrlMode == ObjectController.CtrlMode.Move) UpdateMoveHintPos();
	}

	public void SetCurrentItem(Item item)
	{
		//
		HideHighlight();

		if (null != currentItem && !currentItem.isAssembled)
		{
			currentItem.BackToShelf();
			controller = null;
		}

		currentItem = item;

		if (currentItem == null) { return; }

		controller = currentItem.GetObjectController();
		if (controller.currentCtrlMode == ObjectController.CtrlMode.None)
		{
			SwitchControlMode(ObjectController.CtrlMode.Move);
		}
		else
		{
			SwitchControlMode(controller.currentCtrlMode);
		}
		ToggleControlPanelType(CtrlPanelType.Object);
		ResetCamera();
	}

	public void ToggleControlPanelType(CtrlPanelType type)
	{
		switch (type)
		{
		case CtrlPanelType.Object:
			// SwitchControlMode(ObjectController.CtrlMode.Move);
			objectControlPanel.SetActive(true);
			worldControlPanel.SetActive(false);
			ShowControlHint();
			break;
		case CtrlPanelType.World:
			// SwitchControlMode(ObjectController.CtrlMode.None);
			// worldControlPanel.SetActive(true);
			// objectControlPanel.SetActive(false);
			objectControlPanel.SetActive(true);
			worldControlPanel.SetActive(false);
			HideControlHint();
			break;
		}
	}
	public void ToggleCtrlPanelVisibility(bool visible)
	{
		if (visible)
		{
			ShowControlHint();
		}
		else
		{
			HideControlHint();
		}
	}

	public void SwitchToMoveMode(bool isOn)
	{
		if (!isOn) { return; }
		SwitchControlMode(ObjectController.CtrlMode.Move);
	}
	public void SwitchToRotateMode(bool isOn)
	{
		if (!isOn) { return; }
		SwitchControlMode(ObjectController.CtrlMode.Rotate);
	}

	public void SwitchControlMode(ObjectController.CtrlMode mode)
	{
		if (!controller) { return; }
		controller.currentCtrlMode = mode;

		ShowControlHint();
	}

	public void HideControlHint()
	{
		if (controller)
		{
			controller.SetActiveRotateHint(false);
		}
		moveHint.SetActive(false);
	}

	public void ShowControlHint()
	{
		switch (controller.currentCtrlMode)
		{
		case ObjectController.CtrlMode.None:
			controller.SetActiveRotateHint(false);
			moveHint.SetActive(false);
			break;

		case ObjectController.CtrlMode.Move:
			controller.SetActiveRotateHint(false);
			moveHint.SetActive(true);
			break;

		case ObjectController.CtrlMode.Rotate:
			controller.SetActiveRotateHint(true);
			moveHint.SetActive(false);
			break;
		}
	}

	public void RetriveObject()
	{
		if (null == currentItem) { return; }
		HideControlHint();
		currentItem.BackToShelf();
		currentItem = null;
	}

	public void ResetObject()
	{
		if (null == currentItem) { return; }
		currentItem.ResetObject();
		ResetCamera();
	}

	public void UpdateMoveHintPos()
	{
		// Debug.Log(currentItem);
		if (currentItem)
		{
			Vector3 objPos = currentItem.GetComponent<Transform>().position;
			moveHint.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(objPos);
		}
	}

	public PanCamera panCam;
	public void ResetCamera()
	{
		if (null != currentItem)
		{
			Debug.Log("Reset");
			panCam.transform.position = currentItem.transform.position;
		}
		else
		{
			panCam.transform.position = TabListManager.Instance.currentTab.baseObject.position;
		}
	}

	public GameObject highlightObject;
	public void ShowHighlight(GameObject highlight)
	{
		if (highlightObject)
		{
			HideHighlight();
		}
		highlightObject = highlight;
		highlightObject.SetActive(true);
	}

	public void HideHighlight()
	{
		// Debug.Log(highlightObject.name);
		if (highlightObject)
		{
			highlightObject.SetActive(false);
			highlightObject = null;
		}
	}
}