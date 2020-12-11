using System.Collections;
using System.Collections.Generic;
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

	void Start()
	{

	}

	public void Init()
	{
		// Load 
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

	public void CheckDependecies()
	{

	}
}