using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPath : MonoBehaviour
{
	[SerializeField]
	protected LineRenderer lineRenderer;

	public int Count
	{
		get { return lineRenderer ? lineRenderer.positionCount : 0; }
	}

	public Vector3 this [int index]
	{
		get { return lineRenderer.GetPosition(index); }
	}

	public Stage from;
	public Stage dest;

	private void CheckRequiredComponent()
	{
		TryGetComponent(out lineRenderer);
		if (lineRenderer == null)
		{
			Debug.LogWarning("No Linerenderer Attached to " + gameObject.name);
			this.enabled = false;
		}
	}
	private void OnValidate()
	{
		InitDefaultVert();
	}
	private void OnEnable()
	{
		CheckRequiredComponent();
	}

	void Start()
	{
		TryGetComponent(out lineRenderer);
		if (null == lineRenderer)
		{
			Debug.Log(gameObject.name + " Don't have a Line Renderer component");
			this.gameObject.SetActive(false);
		}
	}

	[ContextMenu("Init Vertex Position")]
	public void InitDefaultVert()
	{
		if (Count < 2) { lineRenderer.positionCount = 2; }
		if (from)
		{
			lineRenderer.SetPosition(0, from.transform.position);
		}
		if (dest)
		{
			lineRenderer.SetPosition(Count - 1, dest.transform.position);
		}

	}

	[ContextMenu("Add Vertex")]
	public void AddVertex()
	{
		if (Count < 2) { lineRenderer.positionCount = 2; }
		Vector3 pos = (lineRenderer.GetPosition(Count - 1) + lineRenderer.GetPosition(Count - 2)) / 2;
		lineRenderer.positionCount += 1;
		lineRenderer.SetPosition(Count - 2, pos);
		lineRenderer.SetPosition(Count - 1, dest.transform.position);
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="from"></param>
	/// <param name="index">Vertex index</param>
	/// <returns>Vertex Position</returns>
	public Vector3 GetVertPosition(Stage from, int index)
	{
		int currentIndex = from == this.from? index: (Count - 1 - index);
		Debug.Log("Index: " + currentIndex);
		return lineRenderer.GetPosition(currentIndex);
	}

}