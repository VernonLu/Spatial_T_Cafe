using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wootopia
{

	[RequireComponent(typeof(CanvasGroup))]
	public class WoCanvasGroup : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;
		[ExecuteInEditMode]
		private void Reset()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
		private void Start()
		{

		}

		public void Hide()
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		public void Show()
		{
			canvasGroup.alpha = 1;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}
	}
}