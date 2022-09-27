using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Déplacement horizontal")]
    [SerializeField] private float maxVelocityX;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundDeceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airDeceleration;
    [Header("Saut")]
    [SerializeField] private float gravity = 20;
    [SerializeField] private int airJumpCount = 1;
    [SerializeField] private float maxVelocityY;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private bool airControl = true;
    [Header("Collisions verticales et horizontales")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask m_WhatIsGround;
    [Header("Autre")]

    private int currentAirJumpCount;

    private Vector2 velocity = Vector2.zero;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider;
    private bool grounded = false;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentAirJumpCount = airJumpCount;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        GroundCheck();
        velocity.y -= gravity * Time.fixedDeltaTime;
        if (airControl || grounded)
            velocity.x += moveDirection.x * Time.fixedDeltaTime * (grounded ? groundAcceleration : airAcceleration);
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocityX, maxVelocityX);
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);
        float deceleration = grounded ? groundDeceleration : airDeceleration;
        if (moveDirection.x == 0)
        {
            if (Mathf.Abs(velocity.x) < deceleration * Time.fixedDeltaTime)
                velocity.x = 0;
            else
                velocity.x -= velocity.x > 0 ? deceleration * Time.fixedDeltaTime : -deceleration * Time.fixedDeltaTime;
        }
        transform.position = transform.position + (Vector3)(velocity * Time.fixedDeltaTime);
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        { 
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                Vector2 translation = colliderDistance.pointA - colliderDistance.pointB;
                transform.Translate(translation);
                if (translation.x != 0)
                    velocity.x = 0;
                if (translation.y != 0)
                    velocity.y = 0;
            }
        }

    }

    private void GroundCheck()
    {
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                currentAirJumpCount = 0;
            }
        }
    }

    private void Jump()
    {
        if (currentAirJumpCount < airJumpCount)
        {
            velocity.y = jumpVelocity;
            currentAirJumpCount += 1;
        }
    }

    public void Move(Vector2 dir, bool jump)
    {

        if (jump)
            Jump();
        moveDirection = dir;
    }
}
