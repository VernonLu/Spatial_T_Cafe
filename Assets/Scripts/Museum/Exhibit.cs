using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Wootopia
{

	public class Exhibit : MonoBehaviour
	{
		public RectTransform rect;
		public List<Text> textContainerList;
		public SequenceImagePlayer imagePlayer;
		public UnityEvent onStartPlay;
		public UnityEvent onStopPlay;
		private bool hasSwitchSoundPlayed = false;

		private LayoutElement layoutElement;
		void Start()
		{
			layoutElement = GetComponent<LayoutElement>();
		}

		public void Init(float scaler)
		{
			float scale = MuseumManager.Instance.GetItemScaler(scaler);
			rect.localScale = new Vector3(scale, scale, 1);
			layoutElement.flexibleHeight = scale;
			layoutElement.flexibleWidth = scale;
			if (imagePlayer)
			{
				// Debug.Log(scaler);
				if (Mathf.FloorToInt(scaler) == 1)
				{
					imagePlayer.Play();
				}
				else
				{
					imagePlayer.Stop();
				}
			}
		}
		public void UpdateScaler(float scaler)
		{

			float scale = MuseumManager.Instance.GetItemScaler(scaler);
			rect.localScale = new Vector3(scale, scale, 1);
			layoutElement.flexibleHeight = scale;
			layoutElement.flexibleWidth = scale;
			if (imagePlayer)
			{
				// Debug.Log(scaler);
				if (Mathf.FloorToInt(scaler) == 1)
				{
					imagePlayer.Play();
					if (!hasSwitchSoundPlayed)
					{
						onStartPlay.Invoke();
						hasSwitchSoundPlayed = true;
					}
				}
				else
				{
					imagePlayer.Stop();
					hasSwitchSoundPlayed = false;
					onStopPlay.Invoke();
				}
			}

			// Change Text alpha
			foreach (var textContainer in textContainerList)
			{
				Color textColor = textContainer.color;
				textColor.a = scaler;
				textContainer.color = textColor;
			}
		}

		[ContextMenu("Get RectTransform")]
		private void GetRectTransform()
		{
			rect = GetComponent<RectTransform>();
		}

		[ContextMenu("Get Text")]
		private void GetTextContainers()
		{
			textContainerList = GetComponentsInChildren<Text>().ToList();
		}
	}
}