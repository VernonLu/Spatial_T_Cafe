using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
  [SerializeField]
  private Data_Sounds dataSounds;

  [SerializeField]
  private AudioEmitter levelBGM;

  private FMOD.Studio.Bus masterBus;
  private FMOD.Studio.Bus bgmBus;
  private FMOD.Studio.Bus sfxBus;

  private float masterVolume;
  private float bgmVolume = 1f;
  private float sfxVolume = 1f;

  public Slider sliderBGM;
  public Slider sliderSFX;

  void Start()
  {
    masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    bgmBus = FMODUnity.RuntimeManager.GetBus("bus:/BGM");
    sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    dataSounds.InitLoopSounds();

    if (levelBGM) PlaySound(levelBGM);
  }

  void Update()
  {
    UpdateBGMVolume();
    UpdateSFXVolume();
  }

  public void PlaySound(AudioEmitter emitter)
  {
    Sound sound = dataSounds.GetSound(emitter.Id);
    if (sound.isOneShot)
    {
      sound.PlayOneShot(emitter.Position);
    }
    else
    {
      PlayLoop(emitter, sound);
    }
  }

  public void PlayOneShot2D(string id)
  {
    Sound sound = dataSounds.GetSound(id);
    if (sound.isOneShot)
    {
      sound.PlayOneShot(Vector3.zero);
    }
    else
    {
      Debug.LogError("Not one shot!");
    }
  }

  private void PlayLoop(AudioEmitter emitter, Sound sound)
  {
    if (sound.isMainLoop)
    {
      StopMainLoops();
    }
    else if (!sound.is2D)
    {
      sound.Set3DAttributes(emitter.Position);
    }
    sound.Start();
    Debug.Log("Play Sound");
  }

  public void StopLoop(AudioEmitter emitter)
  {
    Sound sound = dataSounds.GetSound(emitter.Id);
    if (sound.isOneShot)
    {
      Debug.Log("Can't stop one shot!");
    }
    else
    {
      Debug.Log("Stop loop");
      sound.Stop();
    }
  }

  public void SwitchLoop(AudioEmitter emitter)
  {
    Sound sound = dataSounds.GetSound(emitter.Id);
    StopMainLoops();
    sound.Start();
  }

  public void StopMainLoops()
  {
    foreach (Sound loopSound in dataSounds.LoopSounds)
    {
      if (loopSound.isMainLoop) loopSound.Stop();
    }
  }

  public void PauseLoops()
  {
    foreach (Sound loopSound in dataSounds.LoopSounds)
    {
      loopSound.Pause();
    }
  }

  public void ResumeLoops()
  {
    foreach (Sound loopSound in dataSounds.LoopSounds)
    {
      loopSound.Resume();
    }
  }

  public void UpdateBGMVolume()
  {
    bgmVolume = (float)sliderBGM.value / 6;

    bgmBus.setVolume(bgmVolume);
  }

  public void UpdateSFXVolume()
  {
    sfxVolume = (float)sliderSFX.value / 6;

    sfxBus.setVolume(sfxVolume);
  }

  // public void SetMasterVolume(float volume)
  // {
  //   masterBus.setVolume(volume);
  // }

  // public void SetBGMVolume(float volume)
  // {
  //   bgmVolume = (float)volume / 6;
  //   Debug.Log(bgmVolume);
  //   var result = bgmBus.setVolume(val);
  //   Debug.Log("Set BGM Volume: " + result);
  //   FMODUnity.RuntimeManager.GetBus("bus:/BGM").getVolume(out float bgmVol, out float finalVol);
  //   Debug.Log(bgmVol + " " + finalVol);
  // }

  // public void SetSFXVolume(float volume)
  // {
  //   float val = (9999f / 60000f) * volume + 0.0001f;
  //   sfxBus.setVolume(Mathf.Log10(val) * 20);
  // }

  void OnDestroy()
  {
    StopMainLoops();
  }
}
