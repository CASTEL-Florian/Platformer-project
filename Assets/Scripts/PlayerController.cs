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
    [SerializeField] private float maxFallingSpeedOnWalls = 2f;
    [SerializeField] private AnimationCurve velocityOnSlopeMultiplier;
    [Header("Saut")]
    [SerializeField] private float gravity = 20;
    [SerializeField] private int airJumpCount = 1;
    [SerializeField] private float maxVelocityY;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private Vector2 wallJumpVelocity;
    [SerializeField] private float jumpReleaseMultiplier = 2f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private float coyoteTimeThreshold = 0.1f;
    [Header("Collisions verticales et horizontales")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private LayerMask m_WhatIsWalls;
    [SerializeField] private BoxCollider2D wallCheck;
    [SerializeField] private Transform wallCheckTransform;
    [Header("Autre")]

    private int currentAirJumpCount;
    private bool jump = false;

    private Vector2 velocity = Vector2.zero;
    private Vector2 moveDirection;
    private BoxCollider2D boxCollider;
    private bool grounded = false;
    private int groundObjectId;
    private Vector2 groundPosition;
    private bool onWall = false;
    private Vector2 lastPosition;
    private bool jumpButtonReleased;
    private float timeSinceLeftGround = 0;
    private float slopeAngle = 0;
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
        if (!grounded)
            timeSinceLeftGround += Time.fixedDeltaTime;
        if (jump)
        {
            jump = false;
            if (onWall)
            {
                velocity = wallJumpVelocity;
                if (wallCheck.transform.rotation.eulerAngles.z < 90)
                    velocity.x = -velocity.x;
            }
            else
            {
                velocity.y = jumpVelocity;
            }
        }
        if (velocity.y > 0 && jumpButtonReleased)
        {
            velocity.y /= jumpReleaseMultiplier;
        }
        jumpButtonReleased = false;
        float acceleration = grounded ? groundAcceleration : airAcceleration;
        if (airControl || grounded)
            velocity.x += moveDirection.x * Time.fixedDeltaTime * acceleration;
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocityX, maxVelocityX);
        if (grounded)
        {
            if (slopeAngle * moveDirection.x > 0)
            {
                float slopeMaxVelocity = velocityOnSlopeMultiplier.Evaluate(Mathf.Abs(slopeAngle / 90)) * maxVelocityX;
                velocity.x = Mathf.Clamp(velocity.x, -slopeMaxVelocity, slopeMaxVelocity);
            }
        }
        wallCheckTransform.rotation = Quaternion.Euler(0,0,velocity.x < 0 ? 180 : 0);

        WallCheck();
        velocity.y -= gravity * Time.fixedDeltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);
        if(onWall)
            velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeedOnWalls, maxVelocityY);

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
        slopeAngle = 0;
        if (velocity.y > 0)
        {
            groundObjectId = GetInstanceID();
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.transform.position, groundCheck.size, 0, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                currentAirJumpCount = 0;
                if (colliders[i].tag == "Slope")
                {
                    slopeAngle = colliders[i].transform.rotation.eulerAngles.z;
                    if (slopeAngle > 90)
                        slopeAngle -= 360;
                }
                if (groundObjectId == colliders[i].GetInstanceID())
                {
                    transform.Translate(colliders[i].transform.position - (Vector3)groundPosition);
                }
                groundObjectId = colliders[i].GetInstanceID();
                groundPosition = colliders[i].transform.position;
            }
        }
        if (!grounded)
            groundObjectId = GetInstanceID();
        else
            timeSinceLeftGround = 0;
    }

    private void WallCheck()
    {
        onWall = false;
        if (velocity.x * moveDirection.x <= 0)
            return;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(wallCheck.transform.position, wallCheck.size, 0, m_WhatIsWalls);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                onWall = true;
            }
        }
    }

    private void Jump()
    {
        if (grounded || onWall || timeSinceLeftGround < coyoteTimeThreshold)
        {
            jump = true;
            timeSinceLeftGround = coyoteTimeThreshold;
        }    
        else if (currentAirJumpCount < airJumpCount)
        {
            jump = true;
            currentAirJumpCount += 1;
        }
    }

    public void Move(Vector2 dir, bool jump)
    {

        if (jump)
            Jump();
        moveDirection = dir;
    }

    public void JumpButtonReleased()
    {
        jumpButtonReleased = true;
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
                    if (!grounded && lastPosition.y - transform.position.y + boxCollider.bounds.min.y < hit.bounds.max.y || velocity.y > 0 || moveDirection.y < 0)
                        continue;
                }
                Vector2 translation = colliderDistance.pointA - colliderDistance.pointB;
                if (hit.gameObject.layer == LayerMask.NameToLayer("Slope"))
                {
                    translation.y = Mathf.Abs(translation.x) < 0.01 ? translation.y : Mathf.Abs(translation.magnitude / Mathf.Sin(Mathf.Atan(translation.y / translation.x)));
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
