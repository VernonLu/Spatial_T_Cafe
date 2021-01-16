using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{

	public int stageIndex;

	public bool isFinished = false;

	public List<Level> levels = new List<Level>();
	public List<Stage> dependencies = new List<Stage>();

	public UnityEvent finishedEvent;
	public UnityEvent cameraEnter;
	public UnityEvent cameraLeave;

	private void Start()
	{
		foreach (var level in levels)
		{
			level.LoadData();
		}
		foreach (var level in levels)
		{
			level.CheckDependencies();
		}
		UpdateFinishState();

	}

	[ContextMenu("Update Level List")]
	private void UpdateLevelList()
	{
		levels = GetComponentsInChildren<Level>().ToList();
	}

	public void UpdateFinishState()
	{
		if (isFinished)
		{
			return;
		}

		isFinished = true;
		foreach (var level in levels)
		{
			if (!level.isFinished)
			{
				isFinished = false;
			}
		}

		if (isFinished)
		{
			finishedEvent.Invoke();
		}
	}

	public void ToggleCurrentStage(bool isCurrentStage)
	{
		Debug.Log(this.gameObject.name + ": Update");
		Debug.Log(isCurrentStage);
		foreach (var level in levels)
		{
			level.TogglePackageVisibility(isCurrentStage);
		}
	}

}