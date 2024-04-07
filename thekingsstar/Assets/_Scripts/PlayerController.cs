using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D _col;
    [SerializeField] LayerMask projectileLayer;
    [SerializeField] Animator anim;
    bool isAttacking = false;
    static readonly int SlashAnimation = Animator.StringToHash("slash");
    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int DeathAnimation = Animator.StringToHash("death");
    bool run = true;

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
    }

    void Slash()
    {
        if (isAttacking)
        {
            return;
        }

        
        StopAllCoroutines();
        StartCoroutine(PlayAttackAnimation());

        RaycastHit2D slashRadius = CheckSlashRadius();

        if (slashRadius.collider != null)
        {
            slashRadius.collider.gameObject.GetComponent<Projectile>().Damage();
        }
    }

    RaycastHit2D CheckSlashRadius()
    {
        return Physics2D.CircleCast(_col.bounds.size, 1.5f, Vector2.right, 0, projectileLayer);
    }

    IEnumerator PlayAttackAnimation()
    {
        isAttacking = true;
        anim.CrossFade(SlashAnimation, 0, 0);
        yield return new WaitForSeconds(.3f);
        isAttacking = false;

    }

    void Death()
    {
        StopAllCoroutines();
        run = false;
        anim.CrossFade(DeathAnimation, 0, 0);
    }
}
