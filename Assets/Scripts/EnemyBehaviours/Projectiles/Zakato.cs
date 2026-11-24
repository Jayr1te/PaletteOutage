using System.Collections;
using UnityEngine;

public class Zakato : Projectile
{
    [SerializeField]
    private GameObject spawnBullet;

    public float explodeDelay;

    void Awake()
    {
        Startup();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explodeDelay);

        rb.linearVelocity = Vector2.zero;
        animator.Play("ZakatoExplode");

        // this creates a vector from the zakato to the target (player) and gets its angle in radians
        // the angle is made negative because it works, idk, Unity was being weird
        Vector2 middle = Vector2.Normalize(target.transform.position - transform.position);
        float m_angle = -Vector2.SignedAngle(middle, Vector2.right) * Mathf.Deg2Rad;
        // this creates two more vectors at pi/6 radians (30 degrees) to the left and right of the first vector
        // it uses a favourite of mine, ternary expressions, to do condensed conditionals of 'if (m_angle < 0) ... else ... '
        Vector2 left = new Vector2(
            Mathf.Cos(m_angle < 0 ? m_angle - (Mathf.PI / 6) : m_angle + (Mathf.PI / 6)),
            Mathf.Sin(m_angle < 0 ? m_angle - (Mathf.PI / 6) : m_angle + (Mathf.PI / 6))
        );
        Vector2 right = new Vector2(
            Mathf.Cos(m_angle < 0 ? m_angle + (Mathf.PI / 6) : m_angle - (Mathf.PI / 6)),
            Mathf.Sin(m_angle < 0 ? m_angle + (Mathf.PI / 6) : m_angle - (Mathf.PI / 6))
        );

        GameObject m = Instantiate(spawnBullet, transform.position, Quaternion.identity);
        GameObject l = Instantiate(spawnBullet, transform.position, Quaternion.identity);
        GameObject r = Instantiate(spawnBullet, transform.position, Quaternion.identity);

        m.SendMessage("ReceiveDirection", middle);
        l.SendMessage("ReceiveDirection", left);
        r.SendMessage("ReceiveDirection", right);
    }

    public void DoneExploding()
    {
        Destroy(gameObject);
    }

    public override void TargetLocked(GameObject obj)
    {
        base.TargetLocked(obj);
        direction = Vector2.Normalize(target.transform.position - transform.position);
        rb.linearVelocity = direction * speed;
    }
}
