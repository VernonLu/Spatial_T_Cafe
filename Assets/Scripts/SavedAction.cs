using UnityEngine;

[System.Serializable]
public class SavedAction
{
	public SavedAction(SavedActionType type, Item item, ItemSlot slot, SubPartInfo subPart = null)
	{
		this.type = type;
		this.item = item;
		this.itemSlot = slot;
		this.subPart = subPart;
	}
	public SavedActionType type;
	public Item item;
	public ItemSlot itemSlot;
	public SubPartInfo subPart;
}

public enum SavedActionType
{
	Generate,
	Assemble
}