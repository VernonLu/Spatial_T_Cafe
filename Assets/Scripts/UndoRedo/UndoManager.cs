using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;

public class UndoManager : MonoBehaviour
{
#region Singleton
	private static UndoManager instance;
	public static UndoManager Instance
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

	[SerializeField] // Used for Debug
	private int currentStep = 0;

	// How many steps can be saved in the undo stack. 
	// When adding new step to the undo stack, the oldest step will be removed from the stack if the stack is full.
	// 0 unlimited stack
	[Tooltip("How many actions can be saved in the undo stack.\n\nWhen saving a new action, the oldest action will be removed from the stack if the stack is full.\n\n0: Unlimited stack")]
	public int maxStackCount = 0;

	[SerializeField] // Used for Debug
	private List<Action> undoStack = new List<Action>();

	private bool canSave = true;

	public bool SaveAction(ActionType type, SubPartInfo subPart = null, WoodJoint woodJoint = null)
	{
		if (!canSave) { return false; }
		Debug.Log("Save: " + type);
		// Check if the undo stack is full
		// Remove the oldest item if the stack don't have enough space
		if (maxStackCount != 0 && undoStack.Count >= maxStackCount)
		{
			int itemCount = undoStack.Count - maxStackCount + 1;
			undoStack.RemoveRange(0, itemCount);
		}

		// Delete 
		if (currentStep < undoStack.Count)
		{
			Debug.Log("Deleteing " + (undoStack.Count - currentStep) + " Actions: From " + currentStep);
			undoStack.RemoveRange(currentStep, undoStack.Count - currentStep);
		}

		// Get CurrentControlling Item from control manager
		Item item = ControlManager.Instance.currentItem;
		ItemSlot itemSlot = item.itemSlot;

		// Add new Action to undo stack
		undoStack.Add(new Action(type, item, itemSlot, subPart, woodJoint));
		++currentStep;

		return true;
	}

	public void Undo()
	{
		// Check whether currentstep is the first step in the undo stack
		if (currentStep <= 0) { return; }

		// Load previous step
		--currentStep;
		Action action = undoStack[currentStep];
		switch (action.actionType)
		{
		case ActionType.Generate:
			action.item.BackToShelf();
			break;
		case ActionType.Assemble:
			action.item.gameObject.SetActive(true);
			action.item.ResetObject();

			action.subPart.canMove = true;
			action.subPart.gameObject.SetActive(false);
			action.woodJoint.enabled = true;
			break;
		}
	}

	public void Redo()
	{
		canSave = false;
		// Check whether currentstep is the last step in the undo stack
		if (currentStep >= undoStack.Count) { return; }

		// Load next step
		Action action = undoStack[currentStep]; //	Copy a variable, not reference to it
		switch (action.actionType)
		{
		case ActionType.Generate:
			action.itemSlot.GenerateItem();
			break;
		case ActionType.Assemble:
			// action.item.isAssembled = true;
			// action.item.gameObject.SetActive(false);
			// item saved in action does't exists
			undoStack[currentStep].item = ControlManager.Instance.currentItem;
			undoStack[currentStep].item.isAssembled = true;
			undoStack[currentStep].item.gameObject.SetActive(false);

			action.subPart.canMove = false;
			action.subPart.gameObject.SetActive(true);
			if (action.woodJoint) { action.woodJoint.enabled = false; }
			break;
		}
		++currentStep;
		canSave = true;

	}
}