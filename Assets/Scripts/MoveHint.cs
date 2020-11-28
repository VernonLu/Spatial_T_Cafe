using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveHint : MonoBehaviour
{
	[SerializeField]
	private bool isVisible;

	public GameObject moveHint;

	void Start()
	{
		if (null == moveHint) { moveHint = gameObject; }
	}

	public void ToggleVisibility(bool visible)
	{
		isVisible = visible;
		moveHint.SetActive(isVisible);
	}
}