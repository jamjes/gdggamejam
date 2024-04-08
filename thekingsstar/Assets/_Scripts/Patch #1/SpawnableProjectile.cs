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
    
    [Header("Animation Settings")]
    [SerializeField] ProjectileType _type;
    [SerializeField] Sprite _staticSprite;
    [SerializeField] Sprite _dynamicSprite;
    [SerializeField] SpriteRenderer _spr;

    [Header("Physics Settings")]
    [SerializeField] Rigidbody2D _rb;
    int _speed = 4;
    float _minSpeed = 2, _maxSpeed = 7;
    int _power = 1;
    int _direction = 0;
    
    public void Configure(int speed, int power, int direction)
    {
        //set speed equal to random betwee min and max range
        _speed = speed;
        _power = power;
        _direction = direction;

        switch (_type)
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
