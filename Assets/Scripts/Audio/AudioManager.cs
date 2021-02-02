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

  void Start()
  {
    masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
		dataSounds.InitLoopSounds();

    if(levelBGM) PlaySound(levelBGM);
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

  private void StopMainLoops()
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
}
