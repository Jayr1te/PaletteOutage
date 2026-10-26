using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public float startHP = 3.0f;
    private float HP;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        HP = startHP;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = new Color(1,HP / startHP,HP / startHP);
        if (HP <= 0.0f)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        print("a");

        if (other.CompareTag("PlayerFireAttack"))
        {
            HP -= 1.0f;
        }
    }
}
