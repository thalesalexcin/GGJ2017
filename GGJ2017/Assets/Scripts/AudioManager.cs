using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioType
{
    EndLevel = 0,
    Die,
    Roll,
    Jump,
    TurretShot    
}

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> Audios;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Stop(EAudioType type)
    {
        Audios[(int)type].Stop();
    }

    public void Play(EAudioType type)
    {
        Audios[(int)type].Play();
    }

    public void Mute(EAudioType type, bool mute)
    {
        Audios[(int)type].mute = mute;
    }
}
