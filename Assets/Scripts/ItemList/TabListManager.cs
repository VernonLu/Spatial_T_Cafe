using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabListManager : MonoBehaviour
{
	private static TabListManager instance;
	public static TabListManager Instance
	{
		get { return instance; }
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
		TryGetComponent(out canvasGroup);
	}
	private CanvasGroup canvasGroup;

	public List<TabManager> tabList = new List<TabManager>();

	public TabManager currentTab;

	public bool isFinished = false;

	void Start()
	{
		tabList = GetComponentsInChildren<TabManager>().ToList();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void UpdateTabList()
	{
		isFinished = true;
		foreach (var tab in tabList)
		{
			tab.UpdateIsFinished();
			if (!tab.isFinished)
			{
				isFinished = false;
			}
		}
		foreach (var tab in tabList)
		{
			tab.UpdateIsLocked();
		}
		Complete();
	}

	/// <summary>
	/// Hide Tab List Panel
	/// </summary>
	public void HideListPanel()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;

	}

	/// <summary>
	/// Show Tab List Panel
	/// </summary>
	public void ShowListPanel()
	{
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
	public void Complete()
	{
		if (isFinished)
		{
			LevelManager.Instance.ShowEndGame();
		}
	}
}