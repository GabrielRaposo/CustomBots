using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int playerID { get; private set; }
    public float damage { get; private set; }
    public Vector2 knockback { get; private set; }

    Rigidbody2D rb;
    Collider2D coll;
    BulletPool pool;

    bool active;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer("Hitbox");
    }

    public void Init(BulletPool pool)
    {
        this.pool = pool;
        transform.tag = "Bullet";
    }

    public void ChargeSetup(int playerID, float scale)
    {
        this.playerID = playerID;
        transform.localScale = Vector3.one * scale;
        coll.enabled = false;

    }

    public void ShootSetup(int playerID, float damage, Vector2 knockback, Vector2 velocity)
    {
        this.playerID = playerID;
        this.damage = damage;
        this.knockback = knockback;

        if(rb) rb.velocity = velocity;
        coll.enabled = true;
        active = true;
    }

    public void Destroy()
    {
        active = false;
        coll.enabled = false;

        pool.Return(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Arena"))
        {
            pool.Return(gameObject);
        }
    }

    public void ReturnToLegalCustody()
    {
        transform.parent = pool.transform;
    }
}
