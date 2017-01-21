using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _Rigidbody;

    public void Shoot(Vector3 direction, float speed, float dieTime)
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);

        if(dieTime > 0)
            Destroy(gameObject, dieTime);
    }
}
