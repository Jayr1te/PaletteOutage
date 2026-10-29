using UnityEngine;

public class RoboGoombaBehaviour : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aggro = false;
        health = 1;
        damage = 1;
        speed = 20;
        direction = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AggroResponse()
    {
        Debug.Log("aggro enter");
        animator.Play("RoboGoombaAggroed");
        aggro = true;
    }

    public override void AggroLeave()
    {
        Debug.Log("aggro leave");
        animator.Play("RoboGoombaPassive");
        aggro = false;
    }
}
