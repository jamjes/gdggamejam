using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    Rigidbody2D _rb;
    int speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(2, 5);
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(-speed, 0);
    }

    public void Damage()
    {
        Destroy(gameObject);
    }
}
