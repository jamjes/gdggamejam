using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour, IParryable
{
    private int direction = -1;
    [SerializeField] private float normalSpeed = 10;
    [SerializeField] private float parrySpeed = 20;
    private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer[] sprs;
    Vector3 size;
    Vector2 spawnPos;

    public delegate void BulletEvent();
    public static event BulletEvent OnTutorialEnter;
    public static event BulletEvent OnBulletReset;

    private void Awake()
    {
        size = transform.localScale;
        spawnPos = transform.position;
        this.gameObject.SetActive(false);
    }

    public void Fire()
    {
        this.gameObject.SetActive(true);
        speed = normalSpeed;
    }

    public void Parry()
    {
        direction = 0;
        StartCoroutine(ParryAfterDelay());
    }

    IEnumerator ParryAfterDelay()
    {
        Color original = sprs[0].color;
        foreach(SpriteRenderer spr in sprs)
        {
            spr.color = Color.red;
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
            speed = 0;
            direction = -1;
            transform.position = spawnPos;
            
            if (OnBulletReset != null)
            {
                OnBulletReset();
            }

            this.gameObject.SetActive(false);
        }
    }
}
