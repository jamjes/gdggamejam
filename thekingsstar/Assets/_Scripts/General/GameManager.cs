using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public delegate void Main();
    public static event Main OnPauseEnter;
    public static event Main OnPauseExit;
    public static GameManager Instance;
    public bool Run { private set; get; }
    public float TimeReference { private set; get; }
    private void OnEnable()
    {
        StartButton.OnGameBegin += BeginTimer;
        PlayerController.OnDeathEnter += ForceStop;
        EnemyTurret.OnTurretDeath += ForceStop;
    }

    void ForceStop()
    {
        Run = false;
    }

    private void OnDisable()
    {
        StartButton.OnGameBegin -= BeginTimer;
        PlayerController.OnDeathEnter -= ForceStop;
        EnemyTurret.OnTurretDeath -= ForceStop;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleRunState()
    {
        if (FindObjectOfType<PlayerController>().IsDead)
        {
            return;
        }

        Run = !Run;

        if (!Run && OnPauseEnter != null)
        {
            OnPauseEnter();
        }
        else if (Run && OnPauseExit != null)
        {
            OnPauseExit();
        }
    }

    void BeginTimer()
    {
        Run = true;
        TimeReference = 0;
    }

    void Update()
    {
        if (!Run)
        {
            return;
        }

        TimeReference += Time.deltaTime;
    }
}
