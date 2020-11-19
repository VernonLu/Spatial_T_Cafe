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
	public GameObject transitionCam;

	public Camera mainCamera;
	public Camera transitionCamera;

	private void Start()
	{
		SwitchOn();
	}

	public void SwitchOn()
	{
		// mainCam.SetActive(true);
		mainCamera.enabled = true;
		// transitionCam.SetActive(false);
		transitionCamera.enabled = false;
		TabListManager.Instance.ShowListPanel();
	}
	public void SwitchOff()
	{
		// mainCam.SetActive(false);
		mainCamera.enabled = false;
		// transitionCam.SetActive(true);
		transitionCamera.enabled = true;
		TabListManager.Instance.HideListPanel();

	}
}