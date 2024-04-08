using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Range(3, 7)] public int Speed;
    [Range(1, 3)] public int Power;

    [Header("Turret Settings")]
    bool _run = false;
    public GameObject[] Projectiles;
    [Range(3,6)] [SerializeField] float _startDelay = 3;
    [Range(1,5)] [SerializeField] float _reloadSpeed = 5;
    [SerializeField] SpriteRenderer _spr;
    float timerRef;


    private void Start()
    {
        string nameSelf = gameObject.name;
        
        if (Projectiles.Length == 0)
        {
            Debug.LogError($"{nameSelf} Disabled! Projectiles list is empty");
            _run = false;
            return;
        }

        foreach(GameObject obj in Projectiles)
        {
            if (obj == null)
            {
                Debug.LogError($"{nameSelf} Disabled! Unassigned value in Projectile List");
                _run = false;
                return;
            }
            else if (obj.GetComponent<SpawnableProjectile>() == null)
            {
                Debug.LogError($"{nameSelf} Disabled! Invalid object in Projectile List");
                _run = false;
                return;
            }
        }

        //Remove after
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
        Debug.Log($"{gameObject.name} has been scheduled to start after delay!");
        StartCoroutine(DelayedStart(_startDelay));
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        _run = true;
        Debug.Log($"Delay complete. {gameObject.name} is enabled!");
        Deploy();
    }

    void Deploy()
    {
        //Randomly switch between projectiles by random int between 0 and 1.
        GameObject obj = Instantiate(Projectiles[0], transform.position, Quaternion.identity);

        int direction;
        
        if (_spr.flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

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
            Deploy();

            timerRef = 0;
        }
    }
}
