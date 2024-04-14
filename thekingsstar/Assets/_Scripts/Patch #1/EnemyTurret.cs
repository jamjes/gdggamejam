using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyTurret : MonoBehaviour, IDamageable
{
    [SerializeField] TurretSettingsScriptableObject _settings;
    bool _run = false;
    int health;
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
        if (_settings.StartDelay)
        {
            _settings.DelayDuration = startDelay;
        }
        _settings.ReloadSpeed = reloadSpeed;
        Init();
    }

    void Init()
    {
        health = _settings.Health;
        healthLabel.text = health.ToString();
        float delay;

        if (_settings.StartDelay)
        {
            delay = _settings.DelayDuration;
        }
        else
        {
            delay = 0;
        }
        
        StartCoroutine(DelayedStart(delay));
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

        int index = Random.Range(0, _settings.Projectiles.Length);
        GameObject obj = Instantiate(_settings.Projectiles[index], transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<SpawnableProjectile>().Configure(_settings.ProjectileSpeed, _settings.ProjectilePower, direction);

    }

    void Update()
    {
        if (!_run)
        {
            return;
        }

        timerRef += Time.deltaTime;

        if (timerRef >= _settings.ReloadSpeed)
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
