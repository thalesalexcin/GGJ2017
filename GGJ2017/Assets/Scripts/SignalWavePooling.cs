using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalWavePooling : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void FakeInstantiate(SignalWave prefab)
    {

    }

    public void FakeDestroy(SignalWave signalWave)
    {

    }
}
