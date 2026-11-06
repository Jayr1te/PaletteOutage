using UnityEngine;

public class HornetBullet : Projectile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Startup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "FireBreakable")
        {
            Destroy(gameObject);
        } else if (other.tag == "Player")
        {
            other.SendMessage("ProjectileHit");
            Destroy(gameObject);
        }
    }

    public override void TargetLocked(GameObject obj)
    {
        base.TargetLocked(obj);
        direction = Vector2.Normalize(target.transform.position - transform.position);
        rb.linearVelocity = direction * speed;
    }
}
