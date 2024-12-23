using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class test_player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    private float speed;

    private float facingDir = 1;
    private bool facingRight = true;


    [Header("Dash info")]
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration = 2 ;
    [SerializeField] private float dashTime;
    private float dashCooldownTimer;

    [Header("Attack info")]
    [SerializeField] private float comboTimeCounter;
    [SerializeField] private float comboTime = 3f;
    private bool isAttacking;
    private int attackState;
    
    
    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        Debug.Log("Start is called");
        
    }
    private enum MovementState { idle, running, jumping, falling }
    // Update is called once per frame
    
    
    void Update()
    {
        Movement();
        Checkinput();
        CollisionChecks();
        DashControl();
        FlipControl();
        UpdateAnimationState();


        comboTimeCounter -= Time.deltaTime;
        
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);
        Debug.Log(isGrounded);
    }


    public void AttackOver(){
        isAttacking = false;
        
        attackState++;
        Debug.Log("attackState" + attackState);

        if(attackState > 2){
            attackState = 0;
        }

        
    }

    private void DashControl()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (dashTime > 0)
        {
            Debug.Log("I'm dashing");
            dashTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;  // ÉèÖÃÀäÈ´Ê±¼ä
        }
    }


    private void Checkinput()
    {
        speed = Input.GetAxisRaw("Horizontal");
        if(Input.GetMouseButtonDown(0))
        {
            StartAttackEvent();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        }

    private void StartAttackEvent()
    {
        if (!isGrounded)
        {
            return;
        }
        if(comboTimeCounter < 0)
        {
            attackState = 0;
        }
        isAttacking = true;
        comboTimeCounter = comboTime;
    }

    private void Movement()
    {

        if (isAttacking) {
            rb.velocity = new Vector2(0, 0);
        }
        else if( dashTime >0){
           rb.velocity = new Vector2(facingDir*dashSpeed, 0);    
        }
        else{
            rb.velocity = new Vector2(speed * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void UpdateAnimationState(){
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isDashing",dashTime>0);
        anim.SetBool("isAttacking",isAttacking);
        anim.SetInteger("attackState",attackState);
        anim.SetBool("isGrounded", isGrounded);
    }


    private void Flip(){
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void FlipControl(){

        if(Input.GetKeyDown(KeyCode.R)){
           Flip();
        }


        if(rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

}
