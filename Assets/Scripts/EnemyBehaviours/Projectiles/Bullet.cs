using UnityEngine;

public class Bullet : Projectile
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

    public override void TargetLocked(GameObject obj)
    {
        base.TargetLocked(obj);
        direction = Vector2.Normalize(target.transform.position - transform.position);
        rb.linearVelocity = direction * speed;
    }
}
