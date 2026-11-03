using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private Vector2 direction;

    protected Rigidbody2D rb;

    public int speed;
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.Normalize(target.transform.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }
}
