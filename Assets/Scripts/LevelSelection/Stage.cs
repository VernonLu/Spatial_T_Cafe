using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{

	public int stageIndex;

	public bool isFinished = false;

	public List<Level> levels = new List<Level>();
	public List<Stage> dependencies = new List<Stage>();

	void Start()
	{

	}

	public void CheckDependecies()
	{

	}
}