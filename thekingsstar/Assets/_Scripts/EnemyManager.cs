using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController[] allEnemies;
    int reference = 0;

    private void OnEnable()
    {
        Door.OnGameWin += OnStageEnd;
    }

    private void OnDisable()
    {
        Door.OnGameWin += OnStageEnd;
    }

    private void Start()
    {
        allEnemies[0].Begin();
    }

    void OnStageEnd()
    {
        reference += 17;

        switch(reference)
        {
            case 17:

                allEnemies[0].gameObject.SetActive(false);

                allEnemies[1].Begin();
                allEnemies[2].Begin();

                break;

            case 34:

                allEnemies[1].gameObject.SetActive(false);
                allEnemies[2].gameObject.SetActive(false);

                allEnemies[3].Begin();
                allEnemies[4].Begin();

                break;
        }
    }
}
