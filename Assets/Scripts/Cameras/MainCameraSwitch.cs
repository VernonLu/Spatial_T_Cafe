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
	public OrbitCamera orbitCamera;

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
		ControlManager.Instance.HideControlHint();

	}
	public void SwitchToOrbitCam()
	{
		mainCamera.enabled = false;
		transitionCamera.enabled = false;
		orbitCamera.SetActive(true);
	}
}