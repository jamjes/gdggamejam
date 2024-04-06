using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D _col;
    [SerializeField] LayerMask projectileLayer;

    private void Start()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Slash();
        }
    }

    void Slash()
    {
        RaycastHit2D slashRadius = CheckSlashRadius();

        if (slashRadius.collider != null)
        {
            slashRadius.collider.gameObject.GetComponent<Projectile>().Damage();
        }
    }

    RaycastHit2D CheckSlashRadius()
    {
        return Physics2D.CircleCast(_col.bounds.size, 2, Vector2.right, 0, projectileLayer);
    }
}
