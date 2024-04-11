using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
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
    [SerializeField] LayerMask groundLayer;


    /*[SerializeField] Animator anim;
    static readonly int SlashAnimation = Animator.StringToHash("attack");
    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int DeathAnimation = Animator.StringToHash("hurt");
    static readonly int JumpAnimation = Animator.StringToHash("jump");
    static readonly int RunAnimation = Animator.StringToHash("run");*/


    bool isAttacking = false;
    public bool IsAttacking;
    public bool IsGrounded;
    public bool IsDead;
    bool run = true;
    int reference;
    int _direction = 1;
    bool runStateMachine = true;

    public delegate void GameDelegate();
    public static event GameDelegate OnMoveEnd;


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

        IsGrounded = GroundedCheck();

        if (Input.GetKeyDown(KeyCode.J))
        {
            StopAllCoroutines();
            Slash();
            /*StartCoroutine(SetStateFor(State.slash, .3f));*/
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded)
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

        /*if (runStateMachine)
        {
            if (_rb.velocity.y != 0)
            {
                UpdateSuperState(State.jump);
            }
            else if (_rb.velocity.y == 0)
            {
                UpdateSuperState(State.idle);
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(SetStateFor(State.slash, .3f));
        }

        StateAnimUpdate();*/
    }

    void FlipPlayer(int direction)
    {
        _direction = direction;

        if (_direction == -1)
        {
            slashOrigin.position = new Vector2(-1.3f, transform.position.y);
            spr.flipX = true;
        }
        else
        {
            slashOrigin.position = new Vector2(1.3f, transform.position.y);
            spr.flipX = false;
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
        bool condition = true;
        RaycastHit2D slashRadius = CheckSlashRadius();

        if (slashRadius.collider != null)
        {
            SpawnableProjectile x = slashRadius.collider.GetComponent<SpawnableProjectile>();

            if (x == null)
            {
                return;
            }

            switch(x.Type)
            {
                case SpawnableProjectile.ProjectileType.Bullet:
                    x.Parry();
                    break;

                case SpawnableProjectile.ProjectileType.Bomb:
                    x.Explode();
                    Death();
                    condition = false;
                    Damage(null);
                    break;
            }
        }

        if (condition)
        {
            StartCoroutine(Attack());
        }
    }

    RaycastHit2D CheckSlashRadius()
    {
        return Physics2D.CircleCast(slashOrigin.transform.position, .75f, Vector2.zero, 0, projectileLayer);
    }

    IEnumerator Attack()
    {
        IsAttacking = true;
        yield return new WaitForSeconds(.3f);
        IsAttacking = false;
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

    bool GroundedCheck()
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

    /*void UpdateSuperState(State target)
    {
        if (CurrentState == target)
        {
            return;
        }

        CurrentState = target;
    }*/

    /*void StateAnimUpdate()
    {
        switch (CurrentState)
        {
            case State.idle:
                anim.CrossFade(IdleAnimation, 0, 0);
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
            case State.slash:
                anim.CrossFade(SlashAnimation, 0, 0);
                break;
        }
    }*/

    /*IEnumerator SetStateFor(State target, float duration)
    {
        runStateMachine = false;
        State originalState = CurrentState;
        CurrentState = target;
        yield return new WaitForSeconds(duration);
        CurrentState = originalState;
        runStateMachine = true;
    }*/

    public void Damage(SpawnableProjectile target)
    {
        if (!IsDead)
        {
            IsDead = true;
        }

        /*if (target.Type == SpawnableProjectile.ProjectileType.Bomb)
        {
            run = false;
            UpdateSuperState(State.death);
        }*/
    }
}
