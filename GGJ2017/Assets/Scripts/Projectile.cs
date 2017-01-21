using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 Direction { get; set; }
    public float Speed = 3;
    public float DieTime = 5f;

    void Start()
    {
        Destroy(gameObject, DieTime);
    }

    void Update ()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
	}
}
