using System.Collections;
using TMPro;
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

    public delegate void Turret();
    public static event Turret OnTurretDeath;

    [SerializeField] TextMeshProUGUI healthLabel;
    
    float timerRef;

    [Header("Animation Settings")]
    [SerializeField] AnimationController _animationController;
    [SerializeField] SpriteRenderer _spr;
    public PlayerAudioController AudioController;

    private void OnEnable()
    {
        PlayerController.OnDeathEnter += ForceEnd;
        GameManager.OnGameEnter += Init;
    }

    private void OnDisable()
    {
        PlayerController.OnDeathEnter -= ForceEnd;
        GameManager.OnGameEnter -= Init;
    }

    private void Start()
    {
        _animationController.SetState(AnimationController.State.disabled);
    }

    public void EnableTurret(float startDelay, float reloadSpeed)
    {
        _startDelay = startDelay;
        _reloadSpeed = reloadSpeed;
        Init();
    }

    void Init()
    {
        health = 5;
        healthLabel.text = health.ToString();
        StartCoroutine(DelayedStart(_startDelay));
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioController.PlayAudioContinuous(PlayerAudioController.Sound.idle);
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
        if (health != 0) AudioController.PlayAudio(PlayerAudioController.Sound.damage);
        yield return new WaitForSeconds(.3f);
        if (health <= 0)
        {
            _animationController.SetState(AnimationController.State.deactivate);
            AudioController.StopAudioContinuous();
            AudioController.PlayAudio(PlayerAudioController.Sound.deactivate);

            if (OnTurretDeath != null)
            {
                OnTurretDeath();
            }

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
        healthLabel.text = health.ToString();
        StartCoroutine(HurtAnimation());
        
        if (health >= 0)
        {
            Destroy(target.gameObject);
        }
    }

    void ForceEnd()
    {
        _run = false;
    }
}
