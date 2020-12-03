using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CameraPath : MonoBehaviour
{
	protected LineRenderer path;

	public int Count
	{
		get
		{
			return path ? path.positionCount : 0;
		}
	}

	public Stage from;
	public Stage dest;

	void Start()
	{
		TryGetComponent(out path);
		if (null == path)
		{
			Debug.Log(gameObject.name + " Don't have a Line Renderer component");
			this.gameObject.SetActive(false);
		}
	}

}