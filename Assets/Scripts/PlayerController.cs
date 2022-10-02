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
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask m_WhatIsGround;
    [Header("Autre")]

    private int currentAirJumpCount;

    private Vector2 velocity = Vector2.zero;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider;
    private bool grounded = false;
    private Vector2 lastPosition;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentAirJumpCount = airJumpCount;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        lastPosition = transform.position;
        GroundCheck();
        velocity.y -= grounded ? 0 : gravity * Time.fixedDeltaTime;
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
    }

    private void GroundCheck()
    {
        grounded = false;
        if (velocity.y > 0)
            return;
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, m_WhatIsGround);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.transform.position, groundCheck.size, 0, m_WhatIsGround);
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
        if (grounded || currentAirJumpCount < airJumpCount)
        {
            velocity.y = jumpVelocity;
            if (!grounded)
                currentAirJumpCount += 1;
        }
    }

    public void Move(Vector2 dir, bool jump)
    {

        if (jump)
            Jump();
        moveDirection = dir;
    }

    private void LateUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        foreach (Collider2D hit in hits)
        { 
            if (hit == boxCollider)
                continue;
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
            if (colliderDistance.isOverlapped)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Plateform"))
                {
                    if (lastPosition.y - transform.position.y + boxCollider.bounds.min.y < hit.bounds.max.y || velocity.y > 0)
                        continue;
                }
                Vector2 translation = colliderDistance.pointA - colliderDistance.pointB;
                if (hit.gameObject.layer == LayerMask.NameToLayer("Slope"))
                {
                    translation.y = Mathf.Abs(translation.x) < 0.001 ? translation.y : Mathf.Abs(translation.magnitude / Mathf.Sin(Mathf.Atan(translation.y / translation.x)));
                    translation.x = 0;
                }
                transform.Translate(translation);
                if (Mathf.Abs(translation.x) >= 0.01f)
                {
                    velocity.x = 0;
                }
                if (Mathf.Abs(translation.y) >= 0.01f)
                    velocity.y = 0;
            }
        }
    }
}
