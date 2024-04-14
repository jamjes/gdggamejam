using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    public delegate void PlayerAudio();
    public static event PlayerAudio OnDeathEnter;

    public enum State
    {
        idle,
        slash,
        jump,
        run,
        death
    };
    public State CurrentState;

    public PlayerAudioController AudioController;

    [SerializeField] Transform slashOrigin;
    [SerializeField] SpriteRenderer spr;
    BoxCollider2D _col;
    [SerializeField] LayerMask projectileLayer;
    Rigidbody2D _rb;
    [SerializeField] LayerMask groundLayer;
    
    //bool isAttacking = false;
    public bool IsAttacking;
    public bool IsGrounded;
    public bool IsDead = false;
    //bool run = true;
    int reference;
    int _direction = 1;
    //bool runStateMachine = true;

    public delegate void GameDelegate();
    public static event GameDelegate OnMoveEnd;


    public bool canMove, canAttack, canJump;

    private void OnEnable()
    {
        Projectile.OnDeathEnter += Death;
        ProjectileInverse.OnDeathEnter += Death;
        Door.OnGameWin += Proceed;
        GameManager.OnGameEnter += Restart;
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= Death;
        ProjectileInverse.OnDeathEnter -= Death;
        Door.OnGameWin -= Proceed;
        GameManager.OnGameEnter -= Restart;
    }

    void Restart()
    {
        if (IsDead)
        {
            IsDead = false;
        }

        CurrentState = State.idle;
    }

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        CurrentState = State.idle;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ToggleRunState();
        }

        if (IsDead || GameManager.Instance.Run == false)
        {
            return;
        }

        IsGrounded = GroundedCheck();

        if (Input.GetKeyDown(KeyCode.J))
        {
            StopAllCoroutines();
            Slash();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded)
            {
                AudioController.PlayAudio(PlayerAudioController.Sound.jump);
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //FlipPlayer(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //FlipPlayer(1);
        }
    }

    void FlipPlayer(int direction)
    {
        _direction = direction;

        if (_direction == -1)
        {
            spr.flipX = true;
        }
        else
        {
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
        RaycastHit2D slashRadius = CheckSlashRadius();
        bool sound = false;
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
                    AudioController.PlayAudio(PlayerAudioController.Sound.parry);
                    sound = true;
                    break;

                case SpawnableProjectile.ProjectileType.Bomb:
                    x.Explode();
                    StartCoroutine(SlashFail());
                    break;
            }
        }
        
        StartCoroutine(Attack());
        if (sound == false)
        {
            AudioController.PlayAudio(PlayerAudioController.Sound.slash);
        }
    }

    IEnumerator SlashFail()
    {
        yield return new WaitForSeconds(.2f);
        Damage(null);
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
        //run = false;
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
    
    public void Damage(SpawnableProjectile target)
    {
        if (!IsDead)
        {
            AudioController.PlayAudio(PlayerAudioController.Sound.death);
            IsDead = true;
            if (OnDeathEnter != null)
            {
                OnDeathEnter();
            }
        }
    }
}
