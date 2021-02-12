using System.Collections.Generic;
using UnityEngine;
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
  private float bgmVolume = 0.5f;
  private float sfxVolume = 0.2f;

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
    var bgmResult = bgmBus.setVolume(bgmVolume);
    // Debug.Log("BGM result: " + bgmResult);
    var sfxResult = sfxBus.setVolume(sfxVolume);
    // Debug.Log("SFX result: " + sfxResult);
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

  public void SetMasterVolume(float volume)
  {
    masterBus.setVolume(volume);
  }

  public void SetBGMVolume(float volume)
  {
    float val = volume / 6;
    var result = FMODUnity.RuntimeManager.GetBus("bus:/").setVolume(val);
    Debug.Log("Set BGM Volume: " + result);
    FMODUnity.RuntimeManager.GetBus("bus:/").getVolume(out float bgmVol, out float finalVol);
    Debug.Log(bgmVol + " " + finalVol);
  }

  public void SetSFXVolume(float volume)
  {
    float val = (9999f / 60000f) * volume + 0.0001f;
    sfxBus.setVolume(Mathf.Log10(val) * 20);
  }
}
