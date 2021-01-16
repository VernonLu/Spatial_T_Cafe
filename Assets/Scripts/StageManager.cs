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

	[ContextMenu("Update Stage List")]
	private void UpdateStageList()
	{
		stageList = GetComponentsInChildren<Stage>().ToList();
	}

	public void UpdateCurrentStage(Stage currentStage)
	{
		Debug.Log(this.gameObject.name + ": Update");
		foreach (var stage in stageList)
		{
			stage.ToggleCurrentStage(stage == currentStage);
		}
	}

}