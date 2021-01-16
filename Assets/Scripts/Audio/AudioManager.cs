using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
  [SerializeField]
  private Data_Sounds gameSounds;
  [SerializeField]
  private Data_Sounds uiSounds;

  private Data_Sounds dataSounds;

  [SerializeField]
  private AudioEmitter levelBGM;

  void Start()
  {
    dataSounds = new Data_Sounds();
    dataSounds.Combine(gameSounds);
    dataSounds.Combine(uiSounds);
		dataSounds.InitLoopSounds();

    if(levelBGM) PlaySound(levelBGM);
    PauseLoops();
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
}
