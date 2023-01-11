using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    private float dirX = 0f;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jump = 7f;
    [SerializeField] private LayerMask jumpableGround;
    private enum MovementState {idle, running, jumping, falling}
    [SerializeField] private AudioSource jumpSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX*speed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSound.Play();
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;
        if(dirX >0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
       else if(dirX <0f)
       {
           state = MovementState.running;
           sprite.flipX = true;
       }
       else
       {
           state = MovementState.idle;
       }

       if(rb.velocity.y > .1f)
       {
           state = MovementState.jumping;
       }
       else if(rb.velocity.y < -.1f)
       {
           state = MovementState.falling;
       }
       anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
