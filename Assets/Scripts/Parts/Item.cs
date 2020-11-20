using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wootopia;

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

	public bool isCurrentItem = true;
	public List<Outline> outlines = new List<Outline>();

	// Start is called before the first frame update
	void Start()
	{
		outlines = GetComponentsInChildren<Outline>().ToList();
		foreach (var outline in outlines)
		{
			outline.SetVisibility(isCurrentItem);
		}
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