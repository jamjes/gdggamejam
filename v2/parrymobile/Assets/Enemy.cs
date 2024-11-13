using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    private static readonly int IdleAnim = Animator.StringToHash("enemy-idle");
    private static readonly int ShootAnim = Animator.StringToHash("enemy-shoot");
    private static readonly int DeathAnim = Animator.StringToHash("enemy-death");
    public Bullet bullet;

    public enum EnemyState
    {idle, shoot, death};

    public EnemyState CurrentState;

    void Awake()
    {
        CurrentState = EnemyState.idle;
        animator.CrossFade(IdleAnim, 0, 0);
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(1f);
        CurrentState = EnemyState.shoot;
        animator.CrossFade(ShootAnim, 0, 0);
        bullet.Fire();
    }


}
