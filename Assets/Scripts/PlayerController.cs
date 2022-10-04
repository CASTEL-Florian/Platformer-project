using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Déplacement horizontal")]
    [Space]
    [SerializeField] private float maxVelocityX;
    [SerializeField] private float acceleration;
    [SerializeField] private float sprintMaxVelocityX;
    [SerializeField] private float sprintAcceleration;
    [SerializeField] private bool allowTurnBoost = true;
    [SerializeField] private float turnBoostFactor;
    [SerializeField] private float turnBoostDuration;
    [SerializeField] private bool allowDash = true;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    
    [Header("Saut")]
    [Space]
    [SerializeField] private float gravity = 20;
    [SerializeField] private int airJumpCount = 1;
    [SerializeField] private float maxVelocityY;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private Vector2 wallJumpVelocity;
    [SerializeField] private float jumpReleaseMultiplier = 2f;
    [SerializeField] private float bufferJumpMaxAllowedTime = .2f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private bool airSprintControl = true;
    
    [Header("Collisions verticales et horizontales")]
    [Space]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private BoxCollider2D wallCheck;
    [SerializeField] private Transform wallCheckTransform;
    [SerializeField] private float maxFallingSpeedOnWalls = 2f;
    
    [Header("Autre")]
    [Space]
    [SerializeField] private float groundDeceleration;
    [SerializeField] private float airDeceleration;
    [SerializeField] private float trampolineBounceVelocity;

    private Vector2 velocity = Vector2.zero;
    private Vector2 moveDirection;
    
    private BoxCollider2D boxCollider;
    private Vector2 groundPosition;
    private Vector2 lastPosition;
    private int groundObjectId;
    private bool grounded = false;
    private bool onWall = false;
    
    private int currentAirJumpCount;
    private bool jump = false;
    private bool jumpButtonReleased;
    private float bufferedJumpTime = 0f;
    
    private bool sprinting = false;
    
    private float turnBoostStopTime = 0f;
    
    private float dashDirection = 0f;
    private float dashStopTime = 0f;
    private float dashCooldownStopTime = 0f;

    private bool hasBounced = false;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentAirJumpCount = airJumpCount;
    }
    private void FixedUpdate()
    {
        bool onTrampoline = false;
        lastPosition = transform.position;
        GroundCheck(ref onTrampoline);

        if (onTrampoline)
        {
            velocity.y = trampolineBounceVelocity;
            hasBounced = true;
        }
        else
        {
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
                velocity.y /= jumpReleaseMultiplier;
            jumpButtonReleased = false;
        }

        float speed = sprinting ? sprintAcceleration : acceleration;
        float maxSpeedX = sprinting ? sprintMaxVelocityX : maxVelocityX;
        float deceleration = grounded ? groundDeceleration : airDeceleration;

        if (airControl || grounded)
            velocity.x += moveDirection.x * speed * Time.fixedDeltaTime;

        if (IsTurnBoostActive())
            velocity.x += moveDirection.x * Mathf.Abs(velocity.x) * turnBoostFactor * Time.fixedDeltaTime;
        
        wallCheckTransform.rotation = Quaternion.Euler(0,0,velocity.x < 0 ? 180 : 0);

        WallCheck();
        velocity.y -= gravity * Time.fixedDeltaTime;
        
        if(onWall)
            velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeedOnWalls, maxVelocityY);
        else if(hasBounced)
            velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, velocity.y);
        else
            velocity.y = Mathf.Clamp(velocity.y, -maxVelocityY, maxVelocityY);

        if (moveDirection.x == 0)
        {
            if (Mathf.Abs(velocity.x) < deceleration * Time.fixedDeltaTime)
                velocity.x = 0;
            else
                velocity.x -= velocity.x > 0 ? deceleration * Time.fixedDeltaTime : -deceleration * Time.fixedDeltaTime;
        }

        if (IsDashing())
        {
            velocity.x = dashDirection * dashSpeed;
            velocity.y = 0;
        }
        else
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeedX, maxSpeedX);
        
        transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
    }

    private void GroundCheck(ref bool onTrampoline)
    {
        grounded = false;

        if (velocity.y > 0)
        {
            groundObjectId = GetInstanceID();
            return;
        }
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.transform.position, groundCheck.size, 0, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                onTrampoline = colliders[i].CompareTag("Trampoline");

                if (Time.time < bufferedJumpTime)
                    jump = true;

                grounded = true;
                hasBounced = false;
                currentAirJumpCount = 0;
                
                if (groundObjectId == colliders[i].GetInstanceID())
                {
                    transform.Translate(colliders[i].transform.position - (Vector3)groundPosition);
                }
                groundObjectId = colliders[i].GetInstanceID();
                groundPosition = colliders[i].transform.position;
            }
        }
        
        if(!grounded)
            groundObjectId = GetInstanceID();
    }

    private void WallCheck()
    {
        onWall = false;
        if (velocity.x * moveDirection.x <= 0)
            return;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(wallCheck.transform.position, wallCheck.size, 0, whatIsWall);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                onWall = true;
                if (Time.time < bufferedJumpTime)
                    jump = true;
            }
        }
    }

    private void Jump()
    {
        if (!grounded)
            bufferedJumpTime = Time.time + bufferJumpMaxAllowedTime;
        else if (onWall)
        {
            jump = true;
        }    
        else if (currentAirJumpCount < airJumpCount)
        {
            jump = true;
            currentAirJumpCount += 1;
        }
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

    public void Move(Vector2 dir, bool jump, bool sprint, bool dash) // CHANGE
    {
        if (jump)
            Jump();

        if (allowTurnBoost && ((velocity.x < 0 && dir.x > 0) || (velocity.x > 0 && dir.x < 0)))
            turnBoostStopTime = Time.time + turnBoostDuration;

        moveDirection = dir;
        sprinting = sprint && (sprinting || airSprintControl || (!airSprintControl && grounded));

        if (allowDash && CanDash() && dash && dir.x != 0)
        {
            dashDirection = dir.x > 0 ? 1 : -1;
            dashStopTime = Time.time + dashDuration;
            dashCooldownStopTime = Time.time + dashCooldown;
        }
    }
    
    private bool IsTurnBoostActive() // CHANGE
    {
        return Time.time < turnBoostStopTime;
    }
    
    private bool IsDashing() // CHANGE
    {
        return Time.time < dashStopTime;
    }
    
    private bool CanDash() // CHANGE
    {
        return Time.time >= dashCooldownStopTime;
    }
}
