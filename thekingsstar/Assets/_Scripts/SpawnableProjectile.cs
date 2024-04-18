using System.Collections;
using UnityEngine;

public class SpawnableProjectile : MonoBehaviour
{
    public enum ProjectileType { Bullet, Bomb };
    public ProjectileType Type;
    public int Power { private set; get; } = 1;
    public void Configure(int speed, int power, int direction)
    {
        float minSpeed = speed - 1;
        float maxSpeed = speed + 2;
        _speed = Random.Range(minSpeed, maxSpeed);
        
        Power = power;
        _direction = direction;
    }

    [SerializeField] Sprite _staticSprite;
    [SerializeField] Sprite _dynamicSprite;
    [SerializeField] SpriteRenderer _spr;
    [SerializeField] Rigidbody2D _rb;

    bool _run = true;
    float _speed;
    int _direction = 0;
    bool parried;

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

    void Freeze()
    {
        _run = false;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    void UnFreeze()
    {
        _run = true;
    }

    void FixedUpdate()
    {
        if (_run) _rb.velocity = new Vector2(_direction * _speed, _rb.velocity.y);
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

    private void OnEnable()
    {
        GameManager.OnPauseEnter += Freeze;
        GameManager.OnPauseExit += UnFreeze;
        EnemyTurret.OnTurretDeath += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnPauseEnter -= Freeze;
        GameManager.OnPauseExit -= UnFreeze;
        EnemyTurret.OnTurretDeath -= SelfDestruct;
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
