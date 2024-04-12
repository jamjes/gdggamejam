using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    public Button mainButton;
    public Image spr;

    public delegate void Main();
    public static event Main OnGameEnter;

    private void OnEnable()
    {
        PlayerController.OnDeathEnter += EnableButton;
        EnemyTurret.OnTurretDeath += EnableButton;
    }

    private void OnDisable()
    {
        PlayerController.OnDeathEnter -= EnableButton;
        EnemyTurret.OnTurretDeath -= EnableButton;
    }

    void EnableButton()
    {
        spr.color = Color.white;
        mainButton.enabled = true;
    }

    public void PlayGame()
    {
        spr.color = Color.grey;
        mainButton.enabled = false;

        if (OnGameEnter != null)
        {
            OnGameEnter();
        }
    }


}
