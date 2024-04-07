using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    Rigidbody2D _rb;
    float speed;
    int _direction = -1;

    [SerializeField] Sprite idleSprite, motionSprite;
    SpriteRenderer _spr;

    public delegate void GameDelegate();
    public static event GameDelegate OnDeathEnter;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(8, 13);
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
        _rb.velocity = new Vector2(_direction * speed, 0);
    }

    public void Damage()
    {
        
    }

    public void Parry()
    {
        speed *= 1.5f;
        _direction = 1;
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
