using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSwitch : MonoBehaviour
{
	private static MainCameraSwitch instance;
	public static MainCameraSwitch Instance
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

	public GameObject mainCam;
	public GameObject transitionCamera;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SwitchOn()
	{
		mainCam.SetActive(true);
		transitionCamera.SetActive(false);
	}
	public void SwitchOff()
	{
		mainCam.SetActive(false);
		transitionCamera.SetActive(true);

	}
}