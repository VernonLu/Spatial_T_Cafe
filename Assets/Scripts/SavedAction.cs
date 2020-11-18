using UnityEngine;
[System.Serializable]
public class SavedAction
{
	public SavedAction(SavedActionType action, Transform state)
	{
		actionType = action;
		savedState = state;
	}

	public SavedActionType actionType;
	public Transform savedState;
}
public enum SavedActionType
{
	None,
	Rotate,
	Move,
}