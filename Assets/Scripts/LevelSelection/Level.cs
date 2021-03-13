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

	private bool isLoaded = false;

	private bool isCurrentStage = false;

	private bool isLocked = false;
	public bool IsLocked
	{
		get
		{
			LoadData();
			return isLocked;
		}
	}

	private bool isFinished = false;
	public bool IsFinished
	{
		get
		{
			LoadData();
			return isFinished;
		}
	}
	public List<Level> preLevels = new List<Level>();
	public GameObject packagingBox;
	public GameObject finishedObject;
	public new Collider collider;
	private List<Outline> outlines;

	private List<Collider> finishedObjColliders = new List<Collider>();

	void Start() {
	}

	/// <summary>
	/// Load saved data from PlayerPrefs
	/// </summary>
	public void LoadData()
	{
		if (isLoaded) { return; }
		// Check if this level is finished
		isFinished = PlayerPrefs.GetInt(levelName, 0) == 1;
		isLocked = preLevels.Any((Level level) => !level.IsFinished);
		isLoaded = true;
		finishedObjColliders = finishedObject.GetComponentsInChildren<Collider>().ToList();
	}

	public void UpdateStatus()
	{
		TogglePackageVisibility(!IsLocked && !isFinished);

		outlines = GetComponentsInChildren<Outline>().ToList();

		ToggleOutline(!IsLocked && !isFinished);

		finishedObject.transform.localScale = IsFinished && !IsLocked ? Vector3.one : Vector3.zero;


	}

	private void ToggleOutline(bool visible)
	{
		foreach (var outline in outlines)
		{
			outline.SetVisibility(visible);
		}
	}

	/// <summary>
	/// Hide / Show Package
	/// </summary>
	/// <param name="visible"></param>
	private void TogglePackageVisibility(bool visible)
	{
		// Debug.Log(gameObject.name + " Package " + visible);
		// packagingBox.SetActive(visible);
		packagingBox.transform.localScale = visible ?  Vector3.one : Vector3.zero;

		collider.enabled = !IsLocked;
		Debug.Log(this.gameObject.name + " Collider " + collider.enabled);
	}

	public void ToggleCurrentStage(bool isCurrentStage)
	{

		this.isCurrentStage = isCurrentStage;
		bool visible = !IsLocked && !IsFinished && isCurrentStage;
		TogglePackageVisibility(visible);
		// finishedObject.SetActive(IsFinished || !IsLocked);
		collider.enabled = isCurrentStage && !IsLocked;

		foreach (var colllide in finishedObjColliders)
		{
			colllide.enabled = isCurrentStage;
		}

#if UNITY_EDITOR
		Debug.Log(gameObject.name);
		Debug.Log("isCurrentStage" + isCurrentStage);
		Debug.Log(this.gameObject.name + "  Collider " + collider.enabled);
#endif
	}
}