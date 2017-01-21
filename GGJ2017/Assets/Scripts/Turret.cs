using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Projectile ProjectilePrefab;
    public float ShootDelay = 1f;

    private float _CurrentTimer;

    void Start()
    {
        _CurrentTimer = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        _CurrentTimer += Time.deltaTime;
        if(_CurrentTimer >= ShootDelay)
        {
            _Shoot();
            _CurrentTimer -= ShootDelay;
        }
	}

    private void _Shoot()
    {
        var projectile = Instantiate<Projectile>(ProjectilePrefab);

        var direction = transform.TransformDirection(Vector3.right);
        projectile.Direction = direction;
        projectile.transform.position += transform.position;
    }
}
