using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D _col;
    [SerializeField] LayerMask projectileLayer;
    Rigidbody2D _rb;
    [SerializeField] Animator anim;
    bool isAttacking = false;
    static readonly int SlashAnimation = Animator.StringToHash("slash");
    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int DeathAnimation = Animator.StringToHash("death");
    bool run = true;
    float _direction;
    [SerializeField] LayerMask groundLayer;


    public bool canMove, canAttack, canJump;

    private void OnEnable()
    {
        Projectile.OnDeathEnter += Death;
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= Death;
    }

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!run)
        {
            return;
        }

        if (!isAttacking)
        {
            anim.CrossFade(IdleAnimation, 0, 0);
        }
        
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            Slash();
        }

        if (Input.GetKey(KeyCode.A))
        {
            _direction = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _direction = 1;
        }
        else
        {
            _direction = 0;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded())
            {
                Jump();
            }
        }
    }

    void Slash()
    {
        if (isAttacking || !canAttack)
        {
            return;
        }

        
        StopAllCoroutines();
        StartCoroutine(PlayAttackAnimation());

        RaycastHit2D slashRadius = CheckSlashRadius();

        if (slashRadius.collider != null)
        {
            slashRadius.collider.gameObject.GetComponent<Projectile>().Parry();
        }
    }

    RaycastHit2D CheckSlashRadius()
    {
        return Physics2D.CircleCast(_col.bounds.center, 2, Vector2.right, 0, projectileLayer);
    }

    IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        anim.CrossFade(SlashAnimation, 0, 0);
        yield return new WaitForSeconds(.3f);
        isAttacking = false;

    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        _rb.velocity = new Vector2(_direction * 5, _rb.velocity.y);
    }

    void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 12);
    }

    void Death()
    {
        StopAllCoroutines();
        run = false;
        anim.CrossFade(DeathAnimation, 0, 0);
        _direction = 0;
        _rb.velocity = Vector2.zero;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0, Vector2.down, .2f, groundLayer);
        return hit.collider != null;
    }
}
