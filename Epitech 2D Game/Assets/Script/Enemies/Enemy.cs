using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDying;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Player player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Die()
    {
        isDying = true;
        animator.SetTrigger("death");
        
        //rb.AddForce(new Vector2(sprite.flipX ? 250 : -250, 200));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
            player.Die(transform.position.x < other.transform.position.x);
    }

    void DestroyWithDeath()
    {
        Destroy(gameObject);
    }
}
