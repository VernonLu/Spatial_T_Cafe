using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineHighlight : MonoBehaviour
{
	public bool isActive = false;

	public float interval = 0.1f;

	private List<Material> materials = new List<Material>();
	private List<float> defaultWidth = new List<float>();
	private float time;
	void Start()
	{
		time = 0;
		List<Renderer> renderers = GetComponentsInChildren<Renderer>().ToList();
		foreach (var renderer in renderers)
		{
			int matCount = renderer.materials.ToList().Count;
			for (int i = 0; i < matCount; ++i)
			{
				materials.Add(renderer.materials[i]);
				defaultWidth.Add(renderer.materials[i].GetFloat("_Outline"));
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!isActive) { return; }
		time += Time.deltaTime;
		float factor = (1 - Mathf.Cos(2 * Mathf.PI * time / interval));
		for (int i = 0; i < materials.Count; ++i)
		{
			materials[i].SetFloat("_Outline", factor * defaultWidth[i]);
		}
	}

}