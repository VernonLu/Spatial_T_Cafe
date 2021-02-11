using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wootopia
{
	[RequireComponent(typeof(Renderer))]
	public class Outline : MonoBehaviour
	{
		private Shader shader;
		private string shaderName = "Wootopia/Outline";
		// Renderer component attached to this gameobject
		protected new Renderer renderer;

		protected bool isVisible = false;

		private float outlineWidth;

		private List<Material> materials = new List<Material>();
		private List<float> defaultWidth = new List<float>();
		public void SetVisibility(bool isVisible)
		{
			this.isVisible = isVisible;

			for (int i = 0; i < materials.Count; ++i)
			{
				materials[i].SetFloat("_Outline", isVisible ? defaultWidth[i] : 0);

			}
		}

		private void Awake()
		{
			shader = Shader.Find(shaderName);
			renderer = GetComponent<Renderer>();
			materials = renderer.materials.ToList();

			foreach (var mat in materials)
			{
				if (mat.shader != Shader.Find("Wootopia/Outline") && mat.shader != Shader.Find("Wootopia/TransparentOutline"))
				{
					Debug.LogWarning(gameObject.name + " is assigned with wrong material!");

					// Disable this script when shader is not correct;
					this.enabled = false;
					return;
				}
				float width = mat.GetFloat("_Outline");
				defaultWidth.Add(width);
			}
			SetVisibility(isVisible);
		}
	}

}