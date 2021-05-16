using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSpeedController : MonoBehaviour
{
	[Header("ZOOM")]
	[Range(0,1)]
	public float zoomMin = 0.5f;
	
	[Range(1,10)]
	public float zoomMax = 2f;
	public static float zoomSensitivity = 1;
	
	[Header("PAN")]
	[Range(0,1)]
	public float panMin = 0.5f;
	
	[Range(1,10)]
	public float panMax = 2f;
	public static float panSensitivity = 1;
	
	[Header("ROTATE")]
	[Range(0,1)]
	public float rotateMin = 0.5f;
	
	[Range(1,10)]
	public float rotateMax = 2f;
	public static float rotateSensitivity = 1;

	public Slider zoomSlider;
	public Slider panSlider;
	public Slider rotateSlider;
	
    void Start()
    {
		zoomSlider.minValue = zoomMin;
		zoomSlider.maxValue = zoomMax;
		zoomSensitivity = PlayerPrefs.GetFloat("zoomSpeed", 1);
		zoomSlider.value = zoomSensitivity;

		panSlider.minValue = panMin;
		panSlider.maxValue = panMax;
		panSensitivity = PlayerPrefs.GetFloat("panSpeed", 1);
		panSlider.value = panSensitivity;

		rotateSlider.minValue = rotateMin;
		rotateSlider.maxValue = rotateMax;
		rotateSensitivity = PlayerPrefs.GetFloat("rotateSpeed", 1);
		rotateSlider.value = rotateSensitivity;
    }

	public void UpdateZoomSensitivity(float value){
		zoomSensitivity = value;
		PlayerPrefs.SetFloat("zoomSpeed", value);
	}

	public void UpdatePanSensitivity(float value){
		panSensitivity = value;
		PlayerPrefs.SetFloat("panSpeed", value);

	}
	public void UpdateRotateSensitivity(float value){
		rotateSensitivity = value;
		PlayerPrefs.SetFloat("rotateSpeed", value);
	}
}
