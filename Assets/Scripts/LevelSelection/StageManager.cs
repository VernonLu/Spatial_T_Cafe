using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
#region Singleton
	private static StageManager instance;
	public static StageManager Instance
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

	[SerializeField]
	private List<Stage> stageList;
	private void Start()
	{
		if (null == stageList)
		{
			UpdateStageList();
		}
	}
	public void ManualInit()
	{
		if (null == stageList)
		{
			UpdateStageList();
		}
		foreach (var stage in stageList)
		{
			stage.ManualInit();
		}
	}

	[ContextMenu("Update Stage List")]
	private void UpdateStageList()
	{
		stageList = GetComponentsInChildren<Stage>().ToList();
	}

	public void UpdateCurrentStage(Stage currentStage)
	{
		foreach (var stage in stageList)
		{
			// Debug.Log(stage.gameObject.name + ": Update");
			stage.ToggleCurrentStage(stage == currentStage);
		}
	}

	public Stage GetStageByIndex(int index)
	{
		return stageList.Find(stage => stage.stageIndex == index);
	}

}