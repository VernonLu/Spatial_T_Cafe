using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Wootopia;
public class Level : MonoBehaviour
{
	public string levelName;

	public bool isActive = false;
	public List<Level> dependencies = new List<Level>();
	public bool isFinished;
	public GameObject packagingBox;
	public GameObject finishedObject;

	private List<Outline> outlines;

	[Space]
	public UnityEvent selectEvent;

	[Space]
	public UnityEvent finishEvent;

	void Start()
	{
		// PlayerPrefs.SetInt(levelName, 0);

	}

	public void Init()
	{

		TogglePackageVisibility(isActive && !isFinished);

		finishedObject.SetActive(isActive && isFinished);
		GetComponent<Collider>().enabled = isActive && !isFinished;
		outlines = GetComponentsInChildren<Outline>().ToList();

		if (isActive)
		{
			ShowOutline();
		}
		else
		{
			HideOutline();
		}
	}
	public void ShowOutline()
	{
		foreach (var outline in outlines)
		{
			outline.SetVisibility(true);
		}
	}
	public void HideOutline()
	{
		foreach (var outline in outlines)
		{
			outline.SetVisibility(false);
		}
	}
	public void LoadLevel()
	{
		SceneManager.LoadScene(levelName);
	}

	public void LoadData()
	{
		// PlayerPrefs.SetInt(levelName, 0);
		isFinished = PlayerPrefs.GetInt(levelName, 0) == 1;
	}
	public void CheckDependencies()
	{
		// Debug.Log(this.name);
		isActive = true;
		foreach (var level in dependencies)
		{
			if (!level.isFinished)
			{
				// Debug.Log(level.name + " " + isFinished);
				isActive = false;
			}
		}
		Init();
	}

	/// <summary>
	/// Hide / Show Package
	/// </summary>
	/// <param name="visible"></param>
	public void TogglePackageVisibility(bool visible)
	{
		Debug.Log(this.gameObject.name + ": Update");
		packagingBox.SetActive(visible);
	}
}