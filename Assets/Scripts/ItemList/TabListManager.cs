using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	}

	public List<TabManager> tabList = new List<TabManager>();
	// Start is called before the first frame update
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
		// Check finish status of all tabs before checking lock status
		foreach (var tab in tabList)
		{
			tab.UpdateIsFinished();
		}
		foreach (var tab in tabList)
		{
			tab.UpdateIsLocked();
		}
	}

	public TabManager currentTab;

}