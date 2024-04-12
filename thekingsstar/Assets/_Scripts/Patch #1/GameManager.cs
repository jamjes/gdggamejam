using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void Main();
    public static event Main OnGameEnter;

    void Start()
    {
        if (OnGameEnter != null)
        {
            OnGameEnter();
        }
    }
}
