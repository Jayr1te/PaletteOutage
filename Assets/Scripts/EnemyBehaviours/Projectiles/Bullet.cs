using UnityEngine;

public class Bullet : Projectile
{
    void Awake()
    {
        Startup();
    }

    public override void TargetLocked(GameObject obj)
    {
        base.TargetLocked(obj);
        direction = Vector2.Normalize(target.transform.position - transform.position);
        rb.linearVelocity = direction * speed;
    }
}
