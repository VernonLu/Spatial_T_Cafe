using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{

	public int stageIndex;
	private bool initialized = false;
#region isFinished
	private bool isFinished = false;
	public bool IsFinished
	{
		get
		{
			Init();
			return isFinished;
		}
	}
#endregion

#region isLocked
	private bool isLocked = true;
	public bool IsLocked
	{
		get
		{
			Init();
			return isLocked;
		}
	}
#endregion
	public List<Level> levels = new List<Level>();
	public List<Stage> preStages = new List<Stage>();

	public List<StageSwitchButton> stageSwitchButtons = new List<StageSwitchButton>();

	public UnityEvent finishedEvent;
	public UnityEvent cameraEnter;
	public UnityEvent cameraLeave;

	private void Start() { }

	public void ManualInit()
	{
		Init();
	}
	private void Init()
	{
		if (initialized) { return; }
		// Two steps (Load data & Update status) are seperated to prevent 
		foreach (var level in levels)
		{
			// Load saved data
			level.LoadData();
		}

		foreach (var level in levels)
		{
			// Update level status
			level.UpdateStatus();
		}

		// Check if all levels belonging to this stage are finished
		CheckFinished();
		// Debug.Log(this.gameObject.name);

		// Check if any pre-stage is not finished
		isLocked = preStages.Exists((Stage stage) => !stage.IsFinished);

		initialized = true;
	}

	[ContextMenu("Update Level List")]
	private void UpdateLevelList()
	{
		levels = GetComponentsInChildren<Level>().ToList();
	}

	private void CheckFinished()
	{
		if (isFinished)
		{
			return;
		}

		isFinished = levels.All((Level level) => level.IsFinished);

		if (isFinished)
		{
			finishedEvent.Invoke();
		}
	}

	public void ToggleCurrentStage(bool isCurrentStage)
	{
		print(gameObject.name + " : " + isCurrentStage);
		foreach (var level in levels)
		{
			// Debug.Log(level.gameObject.name + " isCurrentLevel " + isCurrentStage);
			level.ToggleCurrentStage(isCurrentStage);
		}
	}

}