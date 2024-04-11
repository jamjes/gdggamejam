using System.Collections;
using UnityEngine;

public class SpawnableProjectile : MonoBehaviour
{
    public enum ProjectileType
    {
        Bullet, Bomb
    };
    
    [Header("Animation Settings")]
    [SerializeField] Sprite _staticSprite;
    [SerializeField] Sprite _dynamicSprite;
    [SerializeField] SpriteRenderer _spr;
    public ProjectileType Type;

    [Header("Physics Settings")]
    [SerializeField] Rigidbody2D _rb;
    float _speed;
    public int Power { private set; get; } = 1;
    int _direction = 0;
    bool parried;

    public void Configure(int speed, int power, int direction)
    {
        float minSpeed = speed - 1;
        float maxSpeed = speed + 2;
        _speed = Random.Range(minSpeed, maxSpeed);
        
        Power = power;
        _direction = direction;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Killzone"))
        {
            Destroy(gameObject);
        }

        EnemyTurret asTurret = collision.GetComponent<EnemyTurret>();

        if (asTurret != null && parried)
        {
            asTurret.Damage(this);
        }

        PlayerController asPlayer = collision.GetComponent<PlayerController>();

        if (asPlayer != null && Type == ProjectileType.Bomb)
        {
            if (asPlayer.IsDead)
            {
                return;
            }

            asPlayer.Damage(this);
            Explode();
        }
    }

    public void Parry()
    {
        StartCoroutine(AnimatedParry());
        parried = true;
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    IEnumerator AnimatedParry()
    {
        yield return new WaitForSeconds(.1f);
        _speed *= -1.5f;
    }
}
