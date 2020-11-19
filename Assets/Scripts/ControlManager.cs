using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public void ResetObject()
	{
		if (null == currentItem) { return; }
		currentItem.ResetObject();
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

	public void Undo()
	{
		// if (null == currentItem) { return; }
		// currentItem.Undo();

		if (currentStep <= 0) { return; }
		--currentStep;
		Action action = actionList[currentStep];
		if (action.type == ActionType.Generate)
		{
			action.item.BackToShelf();
		}
		else
		{
			action.item.ResetObject();
		}
	}
	public void Redo()
	{
		// if (null == currentItem) { return; }
		// currentItem.Redo();
		if (currentStep >= actionList.Count) { return; }
		++currentStep;
		Action action = actionList[currentStep];
		if (action.type == ActionType.Generate)
		{
			action.itemSlot.GenerateItem();
		}
		else
		{
			action.item.transform.position = action.savedTransform.position;
			action.item.transform.rotation = action.savedTransform.rotation;
			action.item.isAssembled = true;
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

	public void SaveAction(ActionType type)
	{
		actionList.Add(new Action(type, currentItem, currentItem.itemSlot, currentItem.transform));
	}

	public List<Action> actionList = new List<Action>();
	public int currentStep;
	public enum ActionType
	{
		Generate,
		Assemble
	}
	public class Action
	{
		public Action(ActionType type, Item item, ItemSlot slot, Transform savedTransform = null)
		{
			this.type = type;
			this.item = item;
			this.itemSlot = slot;
			this.savedTransform = savedTransform;
		}
		public ActionType type;
		public Item item;
		public ItemSlot itemSlot;
		public Transform savedTransform;
	}
}