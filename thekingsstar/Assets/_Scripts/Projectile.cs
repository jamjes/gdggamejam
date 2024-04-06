using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(-2, 0);
    }

    public void Damage()
    {
        Debug.Log("Interface Event Triggered!");
        Destroy(gameObject);
    }
}
