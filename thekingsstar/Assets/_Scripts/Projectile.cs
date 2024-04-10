using UnityEngine;

public class Projectile : MonoBehaviour//, IDamageable
{
    Rigidbody2D _rb;
    float speed;
    int _direction = -1;

    public enum Type
    {
        bullet,
        bomb
    };

    public Type ProjectileType;

    [SerializeField] Sprite idleSprite, motionSprite;
    SpriteRenderer _spr;

    public delegate void GameDelegate();
    public static event GameDelegate OnDeathEnter;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(8, 13);
        _spr = GetComponent<SpriteRenderer>();

        int randomInt = Random.Range(0, 2);

        if (randomInt == 0)
        {
            ProjectileType = Type.bullet;
        }
        else
        {
            ProjectileType = Type.bomb;
            _spr.color = Color.red;
        }

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
        _rb.velocity = new Vector2(_direction * speed, 0);
    }

    public void Damage()
    {
        _direction = 0;
        speed = 0;
        OnDeathEnter();
        Destroy(gameObject);
    }

    public void Parry(int direction)
    {
        speed *= 1.5f;
        _direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && OnDeathEnter != null)
        {
            OnDeathEnter();
            Destroy(gameObject);
        }
    }
}
