using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Wootopia

{
	[RequireComponent(typeof(Toggle))]
	public class ToggleImage : MonoBehaviour
	{
		[SerializeField]
		private Toggle toggle;
		[Header("CUSTOM TRANSITION")]
		public Sprite normalSprite;
		public Sprite activeSprite;
		public Sprite disableSprite;

		[ExecuteInEditMode]
		private void Reset()
		{
			GetToggle();
		}
		private void Start()
		{
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(bool isOn)
		{
			toggle.image.sprite = isOn ? activeSprite : normalSprite;
		}

		[ContextMenu("Get Toggle")]
		private void GetToggle()
		{
			toggle = GetComponent<Toggle>();
		}
	}
}