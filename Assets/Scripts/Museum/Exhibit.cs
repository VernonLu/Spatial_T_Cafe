using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Wootopia
{

	public class Exhibit : MonoBehaviour
	{
		public RectTransform rect;
		public List<Text> textContainerList;
		public SequenceImagePlayer imagePlayer;

		void Start()
		{

		}

		public void UpdateScaler(float scaler)
		{

			float scale = MuseumManager.Instance.GetItemScaler(scaler);
			rect.localScale = new Vector3(scale, scale, 1);
			GetComponent<LayoutElement>().flexibleHeight = scale;
			GetComponent<LayoutElement>().flexibleWidth = scale;
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