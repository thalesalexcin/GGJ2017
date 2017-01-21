using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalWave : MonoBehaviour
{
    public float Speed = 1f;

    private Rigidbody2D _Rigidbody;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
	}

    public void Send(Vector2 direction)
    {
        _Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }
}
