using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    BoxCollider2D _col;
    [SerializeField] int health = 5;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameWin;

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        Destroy(collision.gameObject);

        if (health == 0 && OnGameWin != null)
        {
            OnGameWin();
            Destroy(gameObject);
        }
    }
}
