using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wootopia;

public class StageIndicator : MonoBehaviour
{
	public WoCanvasGroup canvasGroup;

	public List<StageSwitchButton> buttons;
    void Start()
    {
        
    }

	public void SetActive(bool value)
	{
		bool buttonActive = false;
		foreach (var button in buttons)
		{
			button.SetActive(value);
			buttonActive |= button.isActive;
			Debug.Log(button.gameObject.name + " " + button.isActive);
		}
		
		if (value)
		{
			if (buttonActive)
			{
				canvasGroup.Show();
			}
			else
			{
				canvasGroup.Hide();
			}
		}
		else
		{
			canvasGroup.Hide();
		}
	}

#if UNITY_EDITOR
	[ContextMenu("Get Buttons")]
	private void GetButtons()
	{
		buttons = GetComponentsInChildren<StageSwitchButton>().ToList();
	}
#endif

}
