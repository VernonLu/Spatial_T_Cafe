using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private static CameraManager instance;
	public static CameraManager Instance
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

	[SerializeField]
	public CameraType activeCamera = CameraType.MainCamera;

	[Header("Main Camera")]
	public RotateCamera rotateCamera;
	public PanCamera panCamera;
	public ZoomCamera zoomCamera;

	public Camera mainCamera;

	[Header("Transition Camera")]
	public TransitionCamera transitionCamera;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void ToggleCamera(CameraType type)
	{

		// Disable all camera
		rotateCamera.enabled = false;
		// Active target camera
		switch (type)
		{
		case CameraType.MainCamera:
			break;
		}
	}
	public void SetRotateCamera(bool canRotate)
	{
		rotateCamera.SetRotateCamera(canRotate);
	}

	public void Reset()
	{

	}

}
public enum CameraType
{
	MainCamera,
	TransitionCamera,
}