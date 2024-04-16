using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void Main();
    public static event Main OnGameEnter;
    public static event Main OnPauseEnter;
    public static event Main OnPauseExit;
    
    public static GameManager Instance;
    public bool Run { private set; get; }

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

        Run = true;
    }

    void Start()
    {
        if (OnGameEnter != null)
        {
            OnGameEnter();
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
}
