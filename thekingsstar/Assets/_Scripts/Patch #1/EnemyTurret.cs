using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public bool Run;
    
    public GameObject[] Projectiles;
    [Range(3,6)] [SerializeField] float StartDelay = 3;
    [Range(1,5)] [SerializeField] float ReloadSpeed = 5;
    public SpriteRenderer Spr;

    float timerRef;


    private void Start()
    {
        string nameSelf = gameObject.name;
        
        if (Projectiles.Length == 0)
        {
            Debug.LogError($"{nameSelf} Disabled! Projectiles list is empty");
            Run = false;
            return;
        }

        foreach(GameObject obj in Projectiles)
        {
            if (obj == null)
            {
                Debug.LogError($"{nameSelf} Disabled! Unassigned value in Projectile List");
                Run = false;
                return;
            }
            else if (obj.GetComponent<SpawnableProjectile>() == null)
            {
                Debug.LogError($"{nameSelf} Disabled! Invalid object in Projectile List");
                Run = false;
                return;
            }
        }

        //Remove after
        Init();
    }

    public void EnableTurret(float startDelay, float reloadSpeed)
    {
        StartDelay = startDelay;
        ReloadSpeed = reloadSpeed;
        Init();
    }

    void Init()
    {
        Debug.Log($"{gameObject.name} has been scheduled to start after delay!");
        StartCoroutine(DelayedStart(StartDelay));
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        Run = true;
        Debug.Log($"Delay complete. {gameObject.name} is enabled!");
        Deploy();
    }

    void Deploy()
    {
        GameObject obj = Instantiate(Projectiles[0], transform.position, Quaternion.identity);

        int direction;
        
        if (Spr.flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        obj.transform.parent = transform;

        obj.GetComponent<SpawnableProjectile>().Configure(7,1,direction, SpawnableProjectile.ProjectileType.Bomb);
    }

    void Update()
    {
        if (!Run)
        {
            return;
        }

        timerRef += Time.deltaTime;

        if (timerRef >= ReloadSpeed)
        {
            Deploy();

            timerRef = 0;
        }
    }
}
