using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SignalWavePooling : MonoBehaviour
{
    public static SignalWavePooling Current;
    public SignalWave Prefab;
    private List<SignalWave> _pooledObjects;

    public int initialAmount = 20;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Current = this;
    }

    void Start()
    {
        _pooledObjects = new List<SignalWave>();
        for (int i = 0; i < initialAmount; i++)
        {
            _Instantiate();
        }
    }

    private SignalWave _Instantiate()
    {
        var instantiated = Instantiate<SignalWave>(Prefab);
        instantiated.gameObject.SetActive(false);
        instantiated.transform.parent = gameObject.transform;
        _pooledObjects.Add(instantiated);

        return instantiated;
    }

    public SignalWave GetPooled()
    {
        var inactive = _pooledObjects.FirstOrDefault(a => !a.gameObject.activeInHierarchy);

        if (inactive == null)
            inactive = _Instantiate();

        return inactive;
    }

    public void DestroyAll()
    {
        foreach (var child in _pooledObjects)
            child.DestroySignal();
    }
}
