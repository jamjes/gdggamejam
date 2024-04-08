using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableProjectile : MonoBehaviour
{
    public enum ProjectileType
    {
        Bullet, Bomb
    };
    
    [Header("Main Settings")]
    [SerializeField] Sprite _staticSprite, _dynamicSprite;
    public ProjectileType Type { private set; get; }
    [SerializeField] SpriteRenderer _spr;


    [Header("Physics Settings")]
    [SerializeField] Rigidbody2D _rb;
    [Range(3, 7)] int _speed = 4;
    [Range(1, 3)] int _power = 1;

    int _direction = 0;
    float _minSpeed = 2, _maxSpeed = 7;

    public void Configure()
    {
        switch(Type)
        {
            case ProjectileType.Bullet:
                _spr.color = Color.white;
                break;
            case ProjectileType.Bomb:
                _spr.color = Color.red;
                break;
        }

        _minSpeed = _speed - 2;
        _maxSpeed = _speed + 3;
    }

    public void Configure(int speed, int power, int direction, ProjectileType type)
    {
        _speed = speed;
        _power = power;
        _direction = direction;
        Type = type;
        Configure();
    }

    void Update()
    {
        if (_rb.velocity.x != 0 && _spr.sprite != _dynamicSprite)
        {
            _spr.sprite = _dynamicSprite;
        }
        else if (_rb.velocity.x == 0 && _spr.sprite != _staticSprite)
        {
            _spr.sprite = _staticSprite;
        }

        if (_direction == -1 && _spr.flipX == false)
        {
            _spr.flipX = true;
        }
        else if (_direction == 1 && _spr.flipX == true)
        {
            _spr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_direction * _speed, _rb.velocity.y);
    }
}
