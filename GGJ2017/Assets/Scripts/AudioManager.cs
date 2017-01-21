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
    TurretShot,
    Wave
}

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> Audios;
    public List<AudioSource> Waves;

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
        if (type != EAudioType.Wave)
            Audios[(int)type].Play();
        else
            _PlayRandomWave();
    }

    private void _PlayRandomWave()
    {
        var index = UnityEngine.Random.Range(0, 6);
        Waves[index].Play();
    }

    public void PlayOneShot(EAudioType type)
    {
        var clip = Audios[(int)type].clip;
        Audios[(int)type].PlayOneShot(clip);
    }

    public void Mute(EAudioType type, bool mute)
    {
        Audios[(int)type].mute = mute;
    }
}
