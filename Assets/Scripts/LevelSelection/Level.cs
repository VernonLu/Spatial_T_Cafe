using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wootopia;
public class Level : MonoBehaviour
{
	public bool isFinished;
	public GameObject packagingBox;
	public GameObject finishedObject;
	public string levelName;
	public List<Outline> outlines;

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