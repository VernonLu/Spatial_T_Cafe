using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace Wootopia
{

	public class MuseumManager : MonoBehaviour
	{
		public RectTransform rect;
		public ScrollRect scrollRect;
		public RectTransform content;
		public Canvas canvas;

		public float itemScaler = 2.7f;

		public float itemScaleRange = 555;
		public float itemMaxScaledRange = 150;
		public AnimationCurve itemScaleCurve;
		public float itemSpacing = 50;
		public float itemNormalWidth = 150;
		public float itemScaledWidth = 600;

		private float contentWidth;
		private List<Exhibit> exhibitList = new List<Exhibit>();


		private static MuseumManager instance;
		public static MuseumManager Instance
		{
			get
			{
				return instance;
			}
		}
		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				instance = this;
			}
		}

		void Start()
		{
			exhibitList = GetComponentsInChildren<Exhibit>().ToList();
			InitContentWidth();
			exhibitList[0].UpdateScaler(1);
		}

		private void Update()
		{
			
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0, 1, 0, 0.3f);
			Gizmos.DrawCube(transform.position, new Vector3(itemScaleRange* canvas.scaleFactor, GetComponent<RectTransform>().sizeDelta.y, 1));
			Gizmos.color = new Color(0, 0, 1, 0.3f);
			Gizmos.DrawCube(transform.position, new Vector3(itemMaxScaledRange * canvas.scaleFactor, GetComponent<RectTransform>().sizeDelta.y, 1));
		}
		

		private void InitContentWidth()
		{

			contentWidth = (exhibitList.Count + 3) * (itemNormalWidth + itemSpacing) + itemScaledWidth;

			Vector2 size = content.sizeDelta;
			size.x = contentWidth;
			content.sizeDelta = size;
			content.anchoredPosition = Vector2.zero;
		}

		public void OnValueChanged(Vector2 pos)
		{
			float currentX = pos.x * contentWidth;
			// Debug.Log(currentX);
			float x = transform.position.x;
			float y = transform.position.y;
			float z = 10f;

			float scaleRange = itemScaleRange * canvas.scaleFactor / 2;
			float scaledMaxRange = itemMaxScaledRange * canvas.scaleFactor / 2;

			Debug.DrawLine(new Vector3(x, y - 100, -z), new Vector3(x, y + 100, -z));
			foreach (var exhibit in exhibitList)
			{
				x = exhibit.transform.position.x;
				y = exhibit.transform.position.y;
				z = 10f;

				Debug.DrawLine(new Vector3(x, y - 100, -z), new Vector3(x, y + 100, -z));

				float distance = Mathf.Abs(exhibit.transform.position.x - transform.position.x);
				// float percentage = Mathf.Clamp01((itemScaleRange * canvas.scaleFactor - distance * 2) / (itemScaleRange * canvas.scaleFactor));
				float percentage = Mathf.Clamp01((scaleRange - distance) / (scaleRange - scaledMaxRange));
				exhibit.UpdateScaler(percentage);

			}
		}
		public void OnEndDrag()
		{

			// Get the nearest index
			int index = 0;
			float shortestDistance = float.MaxValue;
			
			foreach (var exhibit in exhibitList)
			{
				float distance = Mathf.Abs(exhibit.transform.position.x - transform.position.x);
				if (distance < shortestDistance)
				{
					index = exhibitList.IndexOf(exhibit);
					shortestDistance = distance;
				}
			}


		}

		public float GetItemScaler(float key)
		{
			return itemScaleCurve.Evaluate(key);
		}
	}
}
