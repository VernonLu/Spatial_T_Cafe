using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
	public GameObject itemPrefab;
	public int itemCount;
	public Text itemCountBadge;

	// Start is called before the first frame update
	void Start()
	{
		UpdateItemCount();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GenerateItem()
	{
		if (!itemPrefab) { return; }
		var newItem = itemPrefab.GetComponent<Item>();
		var item = Instantiate(itemPrefab, newItem.defaultPos, Quaternion.Euler(newItem.defaultRot)).GetComponent<Item>();
		itemCount--;
		ControlManager.Instance.SetCurrentItem(item);
		item.itemSlot = this;
		UpdateItemCount();

	}

	public void RetriveObject(int number = 1)
	{
		itemCount += number;
		UpdateItemCount();
	}
	private void UpdateItemCount()
	{
		itemCountBadge.text = itemCount.ToString();
		GetComponent<Button>().interactable = itemCount > 0;
	}

}