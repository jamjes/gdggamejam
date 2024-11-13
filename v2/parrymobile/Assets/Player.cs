using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int AttackAnim = Animator.StringToHash("attack-sword");
    private static readonly int AltAttackAnim = Animator.StringToHash("attack-sword-alt");
    private static readonly int IdleAnim = Animator.StringToHash("idle-sword");
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask projectileLayer;
    private bool isAttacking = false;
    private float lastAttackTime;
    private int animationIndex = 0;

    public delegate void PlayerEvents();
    public static event PlayerEvents OnMiss;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (isAttacking == true)
            {
                return;
            }

            float attackRequestTime = Time.time;
            int targetAnimation = AttackAnim;
            
            if (attackRequestTime - lastAttackTime < .2f)
            {
                switch (animationIndex)
                {
                    case 0:
                        animationIndex++;
                        targetAnimation = AltAttackAnim;
                        break;

                    case 1:
                        animationIndex--;
                        targetAnimation = AttackAnim;
                        break;
                }
            }
            else
            {
                targetAnimation = AttackAnim;
                animationIndex = 0;
            }

            StartCoroutine(Attack(targetAnimation));
        }

        if (isAttacking == false)
        {
            animator.CrossFade(IdleAnim, 0, 0);
        }
    }
    
    IEnumerator Attack(int animation)
    {
        isAttacking = true;
        animator.CrossFade(animation, 0, 0);

        yield return new WaitForSecondsRealtime(.1f);

        Collider2D[] bullets = Physics2D.OverlapCircleAll(attackPoint.position, 1f, projectileLayer);
        
        if (bullets.Length != 0)
        {
            foreach (Collider2D bullet in bullets)
            {
                bullet.GetComponent<Bullet>().Parry();
            }
        }
        else
        {
            if (OnMiss != null)
            {
                OnMiss();
            }
        }
        
        

        yield return new WaitForSecondsRealtime(.3f);
        
        isAttacking = false;
        lastAttackTime = Time.time;
    }
}
