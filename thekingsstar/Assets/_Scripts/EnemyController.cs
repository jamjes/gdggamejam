using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    float interval = 3;
    float timeRef;
    Vector3 offset = new Vector3 (-1, .5f, 0);

    private void Update()
    {
        timeRef += Time.deltaTime;

        if (timeRef >= interval)
        {
            SpawnProjectile();

            interval = Random.Range(3, 7);
            timeRef = 0;
        }
    }

    void SpawnProjectile()
    {
        Instantiate(projectile, transform.position + offset, Quaternion.identity);
    }
}
