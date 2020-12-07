using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSwitchButton : MonoBehaviour
{
	public Stage stage;
	void Start()
	{

	}

	// Update is called once per frame
	void LateUpdate()
	{
		// transform.LookAt(stage.transform);
		transform.LookAt(stage.transform, Vector3.up);
	}
}