using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PlayerController : MonoBehaviour
{
    [Tab("PlayerThings!")]
    [SerializeField] private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private bool isFiring = false;
    private bool isFacingRight = true;

    [Tab("Drag n' Drops!")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject fire;


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        //Get Axis Raw simply returns a 1 or a 0 instead of a complex float.
        horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("isRun", Mathf.Abs(horizontal));

        //This code gets the players' ground check object and allows it to jump so long as they are not in the air. 
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        //This little segment allows the player to premptively release the jump button while they're ascending in order to shorten a jump. 
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Math.Abs(rb.velocity.y) > 0 && !IsGrounded())
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(fire, transform.position, Quaternion.identity);

        }
        //Continuous calling of the flip method. 
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
   
    //This method makes a little circle around the players ground check object and the ground layer and checks to see if they are colliding.
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) 
        { 
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
