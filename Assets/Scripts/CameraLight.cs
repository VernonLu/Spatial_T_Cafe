using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLight : MonoBehaviour
{
	public Camera Cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		Cam = Camera.current ?? Cam;
		if (null == Cam)
		{
			return;
		}
		transform.position = Cam.transform.position;
		transform.rotation = Cam.transform.rotation;
    }
}
