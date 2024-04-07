using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        idle, activate, shoot, deactivate
    };
    
    public State CurrentState;
    
    [SerializeField] GameObject projectile;
    public float interval = 3;
    float timeRef;

    public float beginDelay;

    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int ActivateAnimation = Animator.StringToHash("activate");
    static readonly int AttackAnimation = Animator.StringToHash("attack");
    static readonly int DeactivateAnimation = Animator.StringToHash("deactivate");

    [SerializeField] Transform spawnPoint;

    [SerializeField] Animator anim;
    bool isAttacking = false;
    public bool run = false;

    //public int direction;


    private void OnEnable()
    {
        Projectile.OnDeathEnter += End;
        ProjectileInverse.OnDeathEnter += End;
        
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= End;
        ProjectileInverse.OnDeathEnter -= End;
    }

    public void End()
    {
        run = false;
    }

    private void Start()
    {
        CurrentState = State.idle;
    }

    public void Begin()
    {
        StartCoroutine(DelayedBegin());
    }

    IEnumerator DelayedBegin()
    {
        yield return new WaitForSeconds(beginDelay);
        run = true;
    }

    private void Update()
    {
        if (!run)
        {
            //StopAllCoroutines();
            return;
        }

        StateUpdate();
        
        timeRef += Time.deltaTime;

        if (isAttacking == false && CurrentState != State.idle)
        {
            CurrentState = State.idle;
        }

        if (timeRef >= interval)
        {
            interval = Random.Range(3, 6);
            timeRef = 0;
            StartCoroutine(ShootCoroutine());
        }
    }

    void SpawnProjectile()
    {
        GameObject bullet = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
    }

    IEnumerator ShootCoroutine()
    {   
        isAttacking = true;
        
        CurrentState = State.activate;
        yield return new WaitForSeconds(.5f);
        CurrentState = State.shoot;
        SpawnProjectile();
        yield return new WaitForSeconds(.3f);
        CurrentState = State.deactivate;
        yield return new WaitForSeconds(.5f);
        
        isAttacking = false;
    }

    void StateUpdate()
    {
        switch (CurrentState)
        {
            case State.idle:
                anim.CrossFade(IdleAnimation,0,0);
                break;
            case State.activate:
                anim.CrossFade(ActivateAnimation, 0, 0);
                break;
            case State.shoot:
                anim.CrossFade(AttackAnimation, 0, 0);
                break;
            case State.deactivate:
                anim.CrossFade(DeactivateAnimation, 0, 0);
                break;
        }
    }
}
