using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Direction;
    public SpriteRenderer Spr;
    public Animator Anim;
    bool canAttack = true;

    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int SlashAnimation = Animator.StringToHash("attack");

    private void Start()
    {
        Anim.CrossFade(IdleAnimation, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            StopAllCoroutines();
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Direction = -1;
            Spr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Direction = 1;
            Spr.flipX = false;
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        Anim.CrossFade(SlashAnimation,0,0);
        yield return new WaitForSeconds(.25f);
        canAttack = true;
        yield return new WaitForSeconds(.25f);
        Anim.CrossFade(IdleAnimation,0,0);
        
    }
}
