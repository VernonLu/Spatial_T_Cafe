﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;

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

	public GameObject moveHint;

	public Item currentItem;

	void Start()
	{
		ToggleControlPanel(CtrlPanelType.World);
	}

	void Update()
	{
		if (controller && controller.currentCtrlMode == ObjectController.CtrlMode.Move) UpdateMoveHintPos();
	}

	public void SetCurrentItem(Item item)
	{
		if (null != currentItem && !currentItem.isAssembled)
		{
			currentItem.BackToShelf();
			controller = null;
		}
		if (null == item)
		{
			ToggleControlPanel(CtrlPanelType.World);
			currentItem = null;
			return;
		}
		currentItem = item;
		controller = currentItem.GetObjectController();
		if (controller.currentCtrlMode == ObjectController.CtrlMode.None)
		{
			SwitchControlMode(ObjectController.CtrlMode.Move);
		}
		ToggleControlPanel(CtrlPanelType.Object);
		ResetCamera();
	}

	public void ToggleControlPanel(CtrlPanelType type)
	{
		switch (type)
		{
		case CtrlPanelType.Object:
			// SwitchControlMode(ObjectController.CtrlMode.Move);
			objectControlPanel.SetActive(true);
			worldControlPanel.SetActive(false);
			ShowContorlHint();
			break;
		case CtrlPanelType.World:
			// SwitchControlMode(ObjectController.CtrlMode.None);
			worldControlPanel.SetActive(true);
			objectControlPanel.SetActive(false);
			HideControlHint();
			break;
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
		ShowContorlHint();
	}

	public void HideControlHint()
	{
		if (controller)
		{

			controller.SetActiveRotateHint(false);
		}
		moveHint.SetActive(false);
	}

	public void ShowContorlHint()
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
}