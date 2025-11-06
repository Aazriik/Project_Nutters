// Libraries
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Global Variables
    // Movement Variables
    [Header("Movement")]
    public float moveSpeed = 10f;               // Player movement speed
    float horizontalMovement;                   // Horizontal movement input

    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;      // Radius for ground check
    private bool isGrounded = false;            // Is the player grounded

    [Header("Jumping")]
    public float jumpForce = 10f;               // Force applied when jumping
    public int maxJumps = 2;                    // Maximum number of jumps allowed
    private int jumpsRemaining;                 // Remaining jumps

    [Header("Gravity")]
    public float baseGravity = 2f;              // Base gravity scale
    public float maxFallSpeed = 18f;            // Maximum fall speed
    public float fallSpeedMultiplier = 2f;      // Multiplier for fall speed


    // Component Refs
    Rigidbody2D rb;                             // Reference to the player's Rigidbody2D
    Collider2D col;                             // Reference to the player's Collider2D
    SpriteRenderer sr;                          // Reference to the player's SpriteRenderer
    Animator anim;                              // Reference to the player's Animator
    GroundCheck groundCheck;                    // Reference to GroundCheck script

    // Input Action References
    

    // Start is called once at creation
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Get the Collider2D component
        col = GetComponent<Collider2D>();
        // Get the SpriteRenderer component
        sr = GetComponent<SpriteRenderer>();
        // Get the Animator component
        anim = GetComponent<Animator>();
        // Initialize GroundCheck
        groundCheck = new GroundCheck(col, LayerMask.GetMask("Ground"), groundCheckRadius);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is grounded
        isGrounded = groundCheck.CheckIsGrounded();
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        // Reset jumps if grounded
        ResetJumpsOnGround();
        // Gravity
        Gravity();
    }

    private void FixedUpdate()
    {
        
    }

    // Check if player is grounded, update ground check radius on validate
    private void OnValidate() => groundCheck?.UpdateCheckRadius(groundCheckRadius);

    //private void SpriteFlip(float hValue)
    //{
    //    // Flip sprite based on movement direction
    //    // hValue is negative for left, positive for right
    //    if (hValue != 0)
    //        sr.flipX = (hValue < 0);
    //}

    public void Move(InputAction.CallbackContext context)
    {
        // Get horizontal movement input
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Reset jumps if grounded
        if (jumpsRemaining >0)
        {        
            // If the jump action is performed and the player is grounded, apply jump force
            if (context.performed)
            {
                // Apply jump force
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                // Decrement jumps remaining
                jumpsRemaining--;
            }
            // Context canceled (jump button released)
            else if (context.canceled && rb.linearVelocity.y > 0)
            {
                // Reduce upward velocity for variable jump height
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                // Decrement jumps remaining
                jumpsRemaining--;
            }
        }
    }

    private void ResetJumpsOnGround()
    {
        if (isGrounded)
            jumpsRemaining = maxJumps;
    }

    private void Gravity()
    {
        // Apply custom gravity logic
        if (rb.linearVelocity.y < 0)
        {
            // Falling
            // Fall increasingly faster
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            // Cap fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            // Rising or stationary
            rb.gravityScale = baseGravity;
        }
    }
}
