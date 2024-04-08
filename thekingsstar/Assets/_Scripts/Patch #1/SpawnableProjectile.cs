using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableProjectile : MonoBehaviour, IDamageable
{
    public enum ProjectileType
    {
        Bullet, Bomb
    };
    
    [Header("Main Settings")]
    public ProjectileType Type;
    public Sprite StaticSprite, DynamicSprite;
    public SpriteRenderer Spr;


    [Header("Physics Settings")]
    public Rigidbody2D Rb;
    [Range(3, 7)] public int Speed = 4;
    [Range(1, 3)] public int Power = 1;

    int _direction = 1;
    float _minSpeed = 2, _maxSpeed = 7;

    public void Configure()
    {
        switch(Type)
        {
            case ProjectileType.Bullet:
                Spr.color = Color.white;
                break;
            case ProjectileType.Bomb:
                Spr.color = Color.red;
                break;
        }

        _minSpeed = Speed - 2;
        _maxSpeed = Speed + 3;
        Speed = (int)_maxSpeed;
    }

    public void Configure(int speed, int power, int direction)
    {
        Speed = speed;
        Power = power;
        _direction = direction;
        Configure();
    }

    void Update()
    {
        if (Rb.velocity.x != 0 && Spr.sprite != DynamicSprite)
        {
            Spr.sprite = DynamicSprite;
        }
        else if (Rb.velocity.x != 0 && Spr.sprite != StaticSprite)
        {
            Spr.sprite = DynamicSprite;
        }

        if (_direction == -1 && Spr.flipX == false)
        {
            Spr.flipX = true;
        }
        else if (_direction == 1 && Spr.flipX == true)
        {
            Spr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        Rb.velocity = new Vector2(_direction * Speed, Rb.velocity.y);
    }

    void IDamageable.Damage()
    {
        Debug.Log("Damage Interface Call Recieved");
    }

    public void Parry(int direction)
    {
        Debug.Log("Parry Interface Call Recieved");
    }
}
