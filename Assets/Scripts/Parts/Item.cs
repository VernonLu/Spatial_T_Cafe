using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

	public string ID;
	public bool isAssembled;
	public bool isOnScreen;

	public Vector3 defaultPos;
	public Vector3 defaultRot;

	// public int maxStep;
	private int currentStep;
	public List<WoodJoint> jointList = new List<WoodJoint>();

	public List<SavedAction> savedActionList = new List<SavedAction>();

	public ItemSlot itemSlot;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void ResetObject()
	{
		transform.position = defaultPos;
		transform.rotation = Quaternion.Euler(defaultRot);
	}

	public void SaveStep()
	{
		if ((savedActionList.Count - 1) > currentStep)
		{
			//Delete unused saved steps
			savedActionList.RemoveRange(currentStep, savedActionList.Count);
		}
		// Sace current state
		savedActionList.Add(new SavedAction(SavedActionType.None, transform));
		++currentStep;
	}

	public void Undo()
	{
		if (currentStep <= 0) { return; }
		GoToStep(--currentStep);
	}

	public void Redo()
	{
		if (currentStep >= savedActionList.Count) { return; }
		GoToStep(++currentStep);
	}

	private void GoToStep(int step)
	{
		if (step < 0 || step >= savedActionList.Count) { return; }
		// Load Saved state
		var savedState = savedActionList[step].savedState;
		transform.position = savedState.position;
		transform.rotation = savedState.rotation;
		currentStep = step;
	}

	public void BackToShelf()
	{
		Debug.Log("Back to Shelf");
		if (null == itemSlot) { return; }
		itemSlot.RetriveObject();
		Destroy(gameObject);
	}

	public ObjectController GetObjectController()
	{
		return transform.GetComponent<ObjectController>();
	}
}