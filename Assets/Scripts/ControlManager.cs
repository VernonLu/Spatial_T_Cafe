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

	public CtrlMode currentCtrlMode = CtrlMode.None;

	void Start() { }

	void Update()
	{
		if (controller && controller.currentCtrlMode == CtrlMode.Move) UpdateMoveHintPos();
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

		CtrlMode lockedMode = controller.GetLockedMode();
		UpdateLockedControlMode(lockedMode);

		currentCtrlMode = controller.currentCtrlMode;

		// Set control mode to Move when it's not assigned
		if (currentCtrlMode == CtrlMode.None)
		{
			currentCtrlMode = CtrlMode.Move;
		}

		// Change control mode when it conflicts with locked mode
		if (currentCtrlMode == lockedMode)
		{
			switch (currentCtrlMode)
			{
			case CtrlMode.Move:
				currentCtrlMode = CtrlMode.Rotate;
				break;
			case CtrlMode.Rotate:
				currentCtrlMode = CtrlMode.Move;
				break;
			}
		}
		Debug.Log("Init Control Mode: " + currentCtrlMode);

		// Update Object control mode
		SwitchControlMode(currentCtrlMode);

		// Update control UI
		switch (currentCtrlMode)
		{
		case CtrlMode.Move:
			moveButton.isOn = true;
			break;
		case CtrlMode.Rotate:
			rotateButton.isOn = true;
			break;
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

	private void UpdateLockedControlMode(CtrlMode controlMode)
	{
		switch (controlMode)
		{
		case CtrlMode.None:
			moveButton.interactable = true;
			rotateButton.interactable = true;
			break;
		case CtrlMode.Move:
			moveButton.interactable = false;
			rotateButton.interactable = true;
			break;

		case CtrlMode.Rotate:
			moveButton.interactable = true;
			rotateButton.interactable = false;
			break;

		}
	}
#region UI -> Object
	public void SwitchToMoveMode(bool isOn)
	{
		if (!isOn) { return; }
		SwitchControlMode(CtrlMode.Move);
	}
	public void SwitchToRotateMode(bool isOn)
	{
		if (!isOn) { return; }
		SwitchControlMode(CtrlMode.Rotate);
	}
	public void SwitchControlMode(CtrlMode mode)
	{
		if (!controller) { return; }
		controller.currentCtrlMode = mode;

		ShowControlHint();
	}
#endregion

#region Control Hint
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
		case CtrlMode.None:
			controller.SetActiveRotateHint(false);
			moveHint.SetActive(false);
			break;

		case CtrlMode.Move:
			controller.SetActiveRotateHint(false);
			moveHint.SetActive(true);
			break;

		case CtrlMode.Rotate:
			controller.SetActiveRotateHint(true);
			moveHint.SetActive(false);
			break;
		}
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
#endregion

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