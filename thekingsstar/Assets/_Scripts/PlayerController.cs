using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    public enum State { idle, slash, jump, run, death };
    public State CurrentState;
    public delegate void PlayerAudio();
    public static event PlayerAudio OnDeathEnter;
    
    public PlayerAudioController AudioController;
    public bool IsAttacking;
    public bool IsGrounded;
    public bool IsDead = false;
    public bool canAttack, canJump;

    [SerializeField] Transform slashOrigin;
    [SerializeField] LayerMask projectileLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] PlayerSettingsScriptableObject playerSettings;

    BoxCollider2D _col;
    Rigidbody2D _rb;

    private void OnEnable()
    {
        StartButton.OnGameBegin += Init;
    }

    private void OnDisable()
    {
        StartButton.OnGameBegin -= Init;
    }

    void Init()
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
        _rb.velocity = new Vector2(_rb.velocity.x, playerSettings.JumpForce);
    }

    bool GroundedCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0, Vector2.down, .2f, groundLayer);
        return hit.collider != null;
    }
    
    public void Damage(SpawnableProjectile target)
    {
        if (IsDead)
        {
            return;
        }

        AudioController.PlayAudio(PlayerAudioController.Sound.death);
        IsDead = true;
        
        if (OnDeathEnter != null)
        {
            OnDeathEnter();
        }
    }
}
