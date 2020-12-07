using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraPathManager : MonoBehaviour
{
	private static CameraPathManager instance;
	public static CameraPathManager Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}
	public List<CameraPath> pathList = new List<CameraPath>();

	private void Start()
	{
		pathList = GetComponentsInChildren<CameraPath>().ToList();
	}

	public void AddPath(CameraPath path)
	{
		pathList.Add(path);
	}

	public CameraPath FindPath(Stage from, Stage dest)
	{
		foreach (var path in pathList)
		{
			if ((path.from == from && path.dest == dest) || path.from == dest && path.dest == from)
			{
				return path;
			}
		}
		return null;
	}

}