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
		isFinished = PlayerPrefs.GetInt(levelName, 0) == 1;
		packagingBox.SetActive(!isFinished);
		finishedObject.SetActive(isFinished);
		GetComponent<Collider>().enabled = !isFinished;
		outlines = GetComponentsInChildren<Outline>().ToList();
		ShowOutline();
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
}