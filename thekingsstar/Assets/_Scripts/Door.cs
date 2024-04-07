using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    BoxCollider2D _col;
    [SerializeField] int health = 5;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameWin;
    public TextMeshProUGUI healthLabel;

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        healthLabel.text = health.ToString();
        Destroy(collision.gameObject);

        if (health == 0 && OnGameWin != null)
        {
            OnGameWin();
            Destroy(gameObject);
        }
    }
}
