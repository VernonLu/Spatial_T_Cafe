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
		// Debug.Log(sliderValue * (100 / 6) - 80);
		// mixer.SetFloat("BGM", sliderValue * (100 / 6) - 80);
		float val = (9999f / 60000f) * sliderValue + 0.0001f;
		mixer.SetFloat("BGM", Mathf.Log10(val) * 20);
	}
	public void SetSFX(float sliderValue)
	{
		float val = (9999f / 60000f) * sliderValue + 0.0001f;
		mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
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