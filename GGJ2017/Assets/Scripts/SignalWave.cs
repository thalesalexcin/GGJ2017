using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalWave : MonoBehaviour
{
    public float Speed = 1f;
    public int NumberOfBounces = 3;

    public EInputType InputType { get; set; }
    public int Id { get; set; }

    private Rigidbody2D _Rigidbody;
    private int _TimesBounced;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _TimesBounced = 0;
	}

    public void Send(Vector2 direction, EInputType input, int id)
    {
        InputType = input;
        Id = id;
        _Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Receptor"))
        {
            //call character functions
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D()
    {
        _TimesBounced++;
        if (_TimesBounced > NumberOfBounces)
            Destroy(gameObject);
    }
}
