using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceShot : Weapon
{
    public int MaxBounces;
    int currBounces = 0;
    public float speed;
    Vector2 lastVel;
    protected override void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bod.AddForce(transform.right * speed);
        Invoke("Disable", 3f);
    }

    private void Update()
    {
        lastVel = bod.velocity; //for obtaining bouncing normal vectors
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //limit the set of bounces
        currBounces++;
        if(currBounces == MaxBounces)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            float speed = lastVel.magnitude;
            Vector2 dir = Vector2.Reflect(lastVel.normalized, collision.contacts[0].normal);
            bod.velocity = dir * Mathf.Max(speed, 0f);
            Sound.PlaySound("bounce"); 
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy1>().Deactivate();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("BossWeakSpot"))
        {
            collision.gameObject.GetComponent<DeathParticles>().Explode();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
