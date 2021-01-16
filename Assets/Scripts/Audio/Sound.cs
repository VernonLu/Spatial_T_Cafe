using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Linq;

[Serializable]
public class Sound
{
	public string id;
	[HideInInspector]
	public string EventId { get { return "event:/" + id; } }
	public bool is2D;
	public bool isOneShot;
	public bool isMainLoop;
	[HideInInspector]
	private EventInstance eventInstance;
	public void SetEventInstance()
	{
		eventInstance = FMODUnity.RuntimeManager.CreateInstance(EventId);
	}
	public void Set3DAttributes(Vector3 position)
	{
		eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
	}

	public Sound Copy()
	{
		Sound copySound = new Sound();
		copySound.id = id;
		copySound.is2D = is2D;
		copySound.isOneShot = isOneShot;
		copySound.isMainLoop = isMainLoop;
		return copySound;
	}

	public void PlayOneShot(Vector3 position)
	{
		eventInstance = RuntimeManager.CreateInstance(EventId);
		if (!is2D) Set3DAttributes(position);
		eventInstance.start();
		eventInstance.release();
	}

	public void Stop()
	{
		eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public void Start()
	{
		eventInstance.start();
	}

	public void Pause()
	{
		eventInstance.setPaused(true);
	}

	public void Resume()
	{
		eventInstance.setPaused(false);
	}
}


[CreateAssetMenu(fileName = "Data_Sounds", menuName = "ImportData/Data_Sounds", order = 1)]
public class Data_Sounds : ScriptableObject
{
	[SerializeField]
	private List<Sound> _sounds = new List<Sound>();
	public Dictionary<string, Sound> sounds { get; private set; }
	public List<Sound> LoopSounds => sounds.Where(sound => !sound.Value.isOneShot).Select(s => s.Value).ToList();

	private void OnEnable()
	{
		sounds = new Dictionary<string, Sound>();
		foreach (Sound sound in _sounds)
		{
			sounds[sound.id] = sound.Copy();
		}
	}

	public void InitLoopSounds()
	{
		foreach (Sound sound in sounds.Values)
		{
			if (!sound.isOneShot)
			{
				sound.SetEventInstance();
			}
		}
	}

	public void Combine(Data_Sounds newData)
	{
		foreach (Sound sound in newData.sounds.Values)
		{
			sounds[sound.id] = sound.Copy();
		}
	}

	public Sound GetSound(string id)
	{
		return sounds[id];
	}
}
