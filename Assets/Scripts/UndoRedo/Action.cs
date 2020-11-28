using UnityEngine;
namespace Wootopia
{
	[System.Serializable]
	public class Action
	{
		public Action(ActionType actionType, Item item, ItemSlot itemSlot = null, SubPartInfo subPart = null, WoodJoint woodJoint = null)
		{
			this.actionType = actionType;
			this.item = item;
			this.itemSlot = itemSlot;
			this.subPart = subPart;
			this.woodJoint = woodJoint;
		}
		public ActionType actionType;
		public Item item;
		public ItemSlot itemSlot;
		public SubPartInfo subPart;
		public WoodJoint woodJoint;

	}
	public enum ActionType
	{
		Generate,
		Assemble,
	}

}