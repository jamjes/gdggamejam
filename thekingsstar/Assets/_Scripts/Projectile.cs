using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    Rigidbody2D _rb;
    int speed;

    [SerializeField] Sprite idleSprite, motionSprite;
    SpriteRenderer _spr;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(8, 11);
        _spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_rb.velocity.x != 0)
        {
            _spr.sprite = motionSprite;
        }
        else
        {
            _spr.sprite = idleSprite;
        }
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
