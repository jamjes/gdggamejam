using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController[] allEnemies;

    private void Start()
    {
        allEnemies[0].Begin();
        allEnemies[1].Begin();
    }
}
