using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wootopia
{
	[RequireComponent(typeof(Renderer))]
	public class Outline : MonoBehaviour
	{
		private Shader shader;
		private string shaderName = "WoodJoint/Outline";
		// Renderer component attached to this gameobject
		protected new Renderer renderer;

		protected bool isVisible = false;

		private float outlineWidth;

		public void SetVisibility(bool isVisible)
		{
			this.isVisible = isVisible;
			renderer.material.SetFloat("_Outline", isVisible ? outlineWidth : 0);
		}

		private void Awake()
		{
			shader = Shader.Find(shaderName);
			renderer = GetComponent<Renderer>();
			if (renderer.material.shader == shader)
			{
				outlineWidth = renderer.material.GetFloat("_Outline");
				SetVisibility(isVisible);
			}
			else
			{
				Debug.LogWarning(gameObject.name + " is assigned with wrong material!");

				// Disable this script when shader is not correct;
				this.enabled = false;
				return;
			}
		}
	}

}