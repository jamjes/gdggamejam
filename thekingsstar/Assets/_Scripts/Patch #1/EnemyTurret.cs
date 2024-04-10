using System.Collections;
using UnityEngine;

public class EnemyTurret : MonoBehaviour, IDamageable
{
    [Header("Projectile Settings")]
    [Range(3, 7)] public int Speed;
    [Range(1, 3)] public int Power;

    [Header("Turret Settings")]
    bool _run = false;
    public GameObject[] Projectiles;
    [Range(3,6)] [SerializeField] float _startDelay = 3;
    [Range(1,5)] [SerializeField] float _reloadSpeed = 5;
    [SerializeField] int health = 3;
    
    float timerRef;

    [Header("Animation Settings")]
    [SerializeField] AnimationController _animationController;
    [SerializeField] SpriteRenderer _spr;

    private void Start()
    {
        _animationController.SetState(AnimationController.State.disabled);
        Init();
    }

    public void EnableTurret(float startDelay, float reloadSpeed)
    {
        _startDelay = startDelay;
        _reloadSpeed = reloadSpeed;
        Init();
    }

    void Init()
    {
        StartCoroutine(DelayedStart(_startDelay));
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animationController.BlendState(AnimationController.State.activate, AnimationController.State.attack, .4f);
        yield return new WaitForSeconds(.4f);
        ShootProjectile();
        _run = true;
        yield return new WaitForSeconds(.4f);
        _animationController.SetState(AnimationController.State.idle);
    }

    IEnumerator AttackAnimation()
    {
        _animationController.SetState(AnimationController.State.attack);
        yield return new WaitForSeconds(.4f);
        _animationController.SetState(AnimationController.State.idle);
    }

    IEnumerator HurtAnimation()
    {
        _run = false;
        _animationController.SetState(AnimationController.State.hurt);
        yield return new WaitForSeconds(.3f);
        if (health <= 0)
        {
            _animationController.SetState(AnimationController.State.deactivate);
            _run = false;
        }
        else
        {
            _animationController.SetState(AnimationController.State.idle);
            _run = true;
        }
    }

    void ShootProjectile()
    {
        int direction;

        if (_spr.flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        int index = Random.Range(0, Projectiles.Length);
        GameObject obj = Instantiate(Projectiles[index], transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<SpawnableProjectile>().Configure(Speed,Power,direction);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(HurtAnimation());
        }
        
        if (!_run)
        {
            return;
        }

        timerRef += Time.deltaTime;

        if (timerRef >= _reloadSpeed)
        {
            StartCoroutine(AttackAnimation());
            ShootProjectile();
            timerRef = 0;
        }
    }

    public void Damage(SpawnableProjectile target)
    {
        if(!_run)
        {
            return;
        }

        health -= target.Power;
        StartCoroutine(HurtAnimation());
        
        if (health >= 0)
        {
            Destroy(target.gameObject);
        }
    }
}
