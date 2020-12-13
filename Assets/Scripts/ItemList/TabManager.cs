using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
	[Header("Tab Status")]
	public bool isOn = false;
	public Transform activePanel;
	public Transform deactivePanel;

	[Space]
	public bool isLocked;
	public Toggle tabToggle;
	public bool checkDependencies = true;
	public List<TabManager> dependencies = new List<TabManager>();

	[Space]
	public bool isFinished = false;
	public Toggle finishToggle;

	public Text tagText;
	public string activeText;
	public string deactiveText;

	[Space]
	protected GridLayoutGroup content;
	[SerializeField]
	protected List<ItemSlot> itemSlotList = new List<ItemSlot>();
	public int itemSlotCount { get { return itemSlotList.Count; } }

	public Transform baseObject;

	protected Animator statusAnimator;

	void Start()
	{
		statusAnimator = GetComponent<Animator>();
		UpdateContentHeight();
		Toggle(isOn);
		tabToggle.isOn = isOn;
		UpdateIsLocked();
		tagText.text = deactiveText;
	}
	private void LateUpdate()
	{
		//UpdateIsFinished();
	}

	public void Toggle(bool isOn)
	{
		transform.SetParent(isOn ? activePanel : deactivePanel);
		tagText.text = isOn ? activeText : deactiveText;
		if (statusAnimator) { statusAnimator.SetBool("isActive", isOn); }
		if (isOn)
		{
			ControlManager.Instance.RetriveObject();
			TabListManager.Instance.currentTab = this;
			ControlManager.Instance.ResetCamera();
		}
		baseObject.gameObject.SetActive(isOn);
		LocationHintBox.Instance.targetTransform1 = isOn ? baseObject : LocationHintBox.Instance.targetTransform1;
	}

	private void UpdateContentHeight()
	{
		// Get content in children
		content = GetComponentInChildren<GridLayoutGroup>();
		if (!content) { return; }

		// Get item list in content
		itemSlotList = content.GetComponentsInChildren<ItemSlot>().ToList();
		var contentRect = content.GetComponent<RectTransform>();

		// Get Cell size, offset and padding 
		var cellSizeY = content.cellSize.y;
		var spacingY = content.spacing.y;
		var paddingY = content.padding.top;
		float contentHeight = (cellSizeY + spacingY) * itemSlotList.Count + paddingY;
		//Set content sizecontent
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
		contentRect.position = Vector3.zero;
	}

	// Check all the depend tab is finished
	public void UpdateIsLocked()
	{
		// Check all dependencies
		isLocked = false;
		if (checkDependencies)
		{
			foreach (var tab in dependencies)
			{
				if (!tab.isFinished)
				{
					isLocked = true;
				}
			}
		}

		// Update UI and animator
		tabToggle.interactable = !isLocked;
		if (statusAnimator) { statusAnimator.SetBool("isLocked", isLocked); }

	}
	// Check all the item in this tab is assembled
	public void UpdateIsFinished()
	{
		isFinished = true;
		foreach (var itemSlot in itemSlotList)
		{
			if (itemSlot.itemCount > 0)
			{
				isFinished = false;
			}
		}

		// Update UI and animator
		// finish.enabled = isFinished;
		if (statusAnimator) { statusAnimator.SetBool("isFinished", isFinished); }
	}
}