using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
	public AudioMixer mixer;
	public Slider bgmSlider;
	public Slider sfxSlider;

	void Start()
	{

	}
	public void SetBGM(float sliderValue)
	{
		Debug.Log(sliderValue * (100 / 6) - 80);
		mixer.SetFloat("BGM", sliderValue * (100 / 6) - 80);
		// mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
	}
	public void SetSFX(float sliderValue)
	{
		Debug.Log(sliderValue * (100 / 6) - 80);
		mixer.SetFloat("SFX", sliderValue * (100 / 6) - 80);
		// mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
	}

	private void Save(string tag, float value)
	{

	}

	private float Load(string tag)
	{
		return PlayerPrefs.GetFloat(tag, 5);

	}
}