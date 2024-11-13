using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour, IParryable
{
    private int direction = -1;
    private float speed = 7;
    private float parrySpeed = 10;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer[] sprs;
    Vector3 size;
    Vector2 spawnPos;

    public delegate void BulletEvent();
    public static event BulletEvent OnTutorialEnter;

    private void Awake()
    {
        size = transform.localScale;
        spawnPos = transform.position;
    }

    public void Parry()
    {
        Debug.Log("Parry");
        direction = 0;
        StartCoroutine(ParryAfterDelay());
    }

    IEnumerator ParryAfterDelay()
    {
        Color original = sprs[0].color;
        foreach(SpriteRenderer spr in sprs)
        {
            spr.color = Color.white;
        }
        
        yield return new WaitForSeconds(.1f);
        direction = 1;
        transform.localScale = new Vector3(size.x * -1, size.y, size.z);
        speed = parrySpeed;
        yield return new WaitForSeconds(.05f);
        foreach (SpriteRenderer spr in sprs)
        {
            spr.color = original;
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * direction, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float originalSpeed = speed;

        if (collision.tag == "Tutorial")
        {
            speed = 0;
            rb.velocity = Vector2.zero;
            if (OnTutorialEnter != null)
            {
                OnTutorialEnter();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.tag == "Reset")
        {
            speed = 7;
            direction = -1;
            transform.position = spawnPos;
        }
    }
}
