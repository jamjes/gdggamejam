using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    private static readonly int IdleAnim = Animator.StringToHash("enemy-idle");
    private static readonly int ShootAnim = Animator.StringToHash("enemy-shoot");
    private static readonly int DeathAnim = Animator.StringToHash("enemy-death");
    public Bullet bullet;

    public delegate void EnemyEvent();
    public static event EnemyEvent OnShoot;
    public bool isDead;
    public LayerMask projectileLayer;

    public enum EnemyState
    {idle, shoot, death};

    public EnemyState CurrentState;

    void Awake()
    {
        CurrentState = EnemyState.idle;
        animator.CrossFade(IdleAnim, 0, 0);
        Shoot();
    }

    private void OnEnable()
    {
        Bullet.OnBulletReset += Shoot;
    }

    private void OnDisable()
    {
        Bullet.OnBulletReset -= Shoot;
    }

    private void Shoot()
    {
        if (isDead == false)
        {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(.7f);
        CurrentState = EnemyState.shoot;
        animator.CrossFade(ShootAnim, 0, 0);
        yield return new WaitForSeconds(.3f);
        bullet.Fire();
        if (OnShoot != null)
        {
            OnShoot();
        }
        yield return new WaitForSeconds(.6f);
        CurrentState = EnemyState.idle;
        animator.CrossFade(IdleAnim, 0, 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            collision.gameObject.SetActive(false);
            isDead = true;
            CurrentState = EnemyState.death;
            animator.CrossFade(DeathAnim, 0, 0);
        }
        else
        {
            Debug.Log("Unrecogniseed");
        }

        
    }


}
