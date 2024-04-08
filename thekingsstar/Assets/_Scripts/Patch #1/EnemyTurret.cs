using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public bool Run;
    
    public GameObject[] Projectiles;
    public float StartDelay;
    public float ReloadSpeed;
    

    private void Start()
    {
        string nameSelf = gameObject.name;
        
        if (!Run)
        {
            Debug.LogWarning($"{nameSelf} Disabled! Run set to false");
            return;
        }

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
    }
}
