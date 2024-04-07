using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    float interval = 3;
    float timeRef;

    static readonly int IdleAnimation = Animator.StringToHash("idle");
    static readonly int AttackAnimation = Animator.StringToHash("attack");

    [SerializeField] Transform spawnPoint;

    [SerializeField] Animator anim;
    bool isAttacking = false;
    bool run = true;


    private void OnEnable()
    {
        Projectile.OnDeathEnter += End;
    }

    private void OnDisable()
    {
        Projectile.OnDeathEnter -= End;
    }

    void End()
    {
        run = false;
    }

    private void Update()
    {
        if (!run)
        {
            StopAllCoroutines();
            return;
        }
        
        timeRef += Time.deltaTime;

        if (isAttacking == false)
        {
            anim.CrossFade(IdleAnimation, 0, 0);
        }

        if (timeRef >= interval)
        {
            SpawnProjectile();

            interval = Random.Range(3, 7);
            timeRef = 0;
            StartCoroutine(ShootCoroutine());
        }
    }

    void SpawnProjectile()
    {
        Instantiate(projectile, spawnPoint.position, Quaternion.identity);
    }

    IEnumerator ShootCoroutine()
    {
        isAttacking = true;
        anim.CrossFade(AttackAnimation, 0, 0);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }
}
