using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        idle,
        slash,
        jump,
        run,
        death
    };

    public State CurrentState;
    [SerializeField] Transform slashOrigin;
    [SerializeField] SpriteRenderer spr;
    BoxCollider2D _col;
    [SerializeField] LayerMask projectileLayer;
    Rigidbody2D _rb;
    [SerializeField] Animator anim;
    bool isAttacking = false;
    static readonly int SlashAnimation = Animator.StringToHash("slash");
    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int DeathAnimation = Animator.StringToHash("death");
    static readonly int JumpAnimation = Animator.StringToHash("jump");
    static readonly int RunAnimation = Animator.StringToHash("run");
    bool run = true;
    int reference;
    int _direction = 1;
    [SerializeField] LayerMask groundLayer;

    public delegate void GameDelegate();
    public static event GameDelegate OnMoveEnd;

    Vector3 slashDirection = Vector2.right;


    public bool canMove, canAttack, canJump;

    private void OnEnable()
    {
        Projectile.OnDeathEnter += Death;
        ProjectileInverse.OnDeathEnter += Death;
        Door.OnGameWin += Proceed;
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= Death;
        ProjectileInverse.OnDeathEnter -= Death;
        Door.OnGameWin -= Proceed;
    }

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        CurrentState = State.idle;
    }

    private void Update()
    {
        if (!run)
        {
            return;
        }

        StateUpdate();

        if (!isAttacking && IsGrounded())
        {
            if (_rb.velocity.x == 0)
            {
                CurrentState = State.idle;
            }
            else
            {
                CurrentState = State.run;
            }

            
        }
        else if (!isAttacking && !IsGrounded())
        {
            CurrentState = State.jump;
        }
        
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            Slash();
            CurrentState = State.slash;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded())
            {
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            FlipPlayer(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            FlipPlayer(1);
        }
    }

    void FlipPlayer(int direction)
    {
        _direction = direction;

        if (_direction == -1)
        {
            slashOrigin.position = new Vector2(-1.3f, 0);
            spr.flipX = true;
            slashDirection = Vector2.left;
        }
        else
        {
            slashOrigin.position = new Vector2(1.3f, 0);
            spr.flipX = false;
            slashDirection = Vector2.right;
        }
        
        
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        _rb.velocity = new Vector2(4, _rb.velocity.y);
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
            Projectile obj = slashRadius.collider.gameObject.GetComponent<Projectile>();

            if (obj != null)
            {
                if (obj.ProjectileType == Projectile.Type.bullet)
                {
                    obj.Parry(_direction);
                }
                else
                {
                    obj.Damage();
                }
            }
            else
            {
                ProjectileInverse obj2 = slashRadius.collider.gameObject.GetComponent<ProjectileInverse>();

                if (obj2.ProjectileType == ProjectileInverse.Type.bullet)
                {
                    obj2.Parry(_direction);
                }
                else
                {
                    obj2.Damage();
                }
            }
        }
    }

    RaycastHit2D CheckSlashRadius()
    {
        //slashOrigin.
        return Physics2D.CircleCast(transform.position, 2f, Vector2.zero, 0, projectileLayer);
    }

    IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(.3f);
        isAttacking = false;

    }

    void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 9);
    }

    void Death()
    {
        StopAllCoroutines();
        run = false;
        CurrentState = State.death;
        _direction = 0;
        _rb.velocity = Vector2.zero;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0, Vector2.down, .2f, groundLayer);
        return hit.collider != null;
    }

    void Proceed()
    {
        reference ++;

        if (reference == 1)
        {
            StartCoroutine(MoveToNextSegment(10));
        }
        else if (reference == 2)
        {
            StartCoroutine(MoveToNextSegment(33));
        }
        else if (reference == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    IEnumerator MoveToNextSegment(float targetPosition)
    {
        canMove = true;
        canAttack = false;
        canJump = false;
        yield return new WaitUntil(() => transform.position.x >= (targetPosition / 2));

        if (OnMoveEnd != null)
        {
            OnMoveEnd();
        }

        yield return new WaitUntil(() => transform.position.x >= targetPosition);

        canMove = false;
        canAttack = true;
        canJump = true;
    }

    void StateUpdate()
    {
        switch (CurrentState)
        {
            case State.idle:
                anim.CrossFade(IdleAnimation, 0, 0);
                break;
            case State.slash:
                anim.CrossFade(SlashAnimation, 0, 0);
                break;
            case State.jump:
                anim.CrossFade(JumpAnimation, 0, 0);
                break;
            case State.run:
                anim.CrossFade(RunAnimation, 0, 0);
                break;
            case State.death:
                anim.CrossFade(DeathAnimation, 0, 0);
                break;
        }
    }
}
