using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    public HealthBar healthBar;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    public float maxHealth;
    public float maxRunSpeed;
    public float jumpForce = 10.0f; // Set your desired jump force
    public float dashForce;
    public float dashDuration;

    float currentHealth;
    float currentRunSpeed;

    bool isGrounded = false; // A flag to check if the player is grounded
    public LayerMask groundLayer; // Set this in the inspector to the layer your ground is on
    public Transform groundCheck; // Assign a child GameObject to act as the ground check position
    public float groundCheckDistance = 0.2f; // Radius of the overlap circle to determine if grounded

    float horizontal = 0.0f;
    float vertical = 0.0f;
    bool canRun = true;
    bool dashInput = false;
    bool jumpInput = false; // Flag to check if jump was requested
    float dashElapsed;

    public LevelTimer levelTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentRunSpeed = maxRunSpeed;
        dashElapsed = dashDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelTimer.levelStarted)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Return))
            {
                dashInput = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpInput = true;
            }


        }
    }

    void FixedUpdate()
    {
        // Handle dashing
        if (dashInput && dashElapsed >= dashDuration)
        {
            Vector2 inputAxes = new Vector2(horizontal, vertical).normalized;
            rb.velocity += inputAxes * dashForce; // Apply the dash force in the input direction
            dashElapsed = 0.0f; // Reset dash timer
            TakeRecoilDamage();
            animator.SetBool("isSwimming", true);
            dashInput = false; // Reset the dash input flag immediately
        }
        // Dashing animation started
        else if (dashElapsed < dashDuration)
        {
            dashElapsed += Time.fixedDeltaTime;
        }
        else if (dashElapsed >= dashDuration)
        {
            dashInput = false;

            if (levelTimer.levelStarted)
            {
                // Check if the player is grounded
                isGrounded = Physics2D.Raycast(rb.position, Vector2.down, groundCheckDistance, groundLayer).collider != null;
                Debug.DrawRay(rb.position, Vector2.down * groundCheckDistance, Color.red);


            }

            // If a jump is requested and the player is grounded then add a vertical force
            if (jumpInput && isGrounded)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpInput = false; // Reset the jump input flag
            }

            // float halfColliderWidth = boxCollider.size.x / 2.0f;
            // bool isCollidingRight = Physics2D.Raycast(rb.position, Vector2.right, groundCheckDistance + halfColliderWidth, groundLayer).collider != null;
            // bool isCollidingLeft = Physics2D.Raycast(rb.position, Vector2.left, groundCheckDistance + halfColliderWidth, groundLayer).collider != null;
            // Debug.DrawRay(rb.position, Vector2.right * (groundCheckDistance + halfColliderWidth), Color.red);
            // Debug.DrawRay(rb.position, Vector2.left * (groundCheckDistance + halfColliderWidth), Color.red);

            // Running logic
            if (Mathf.Abs(horizontal) > 0.2f && currentRunSpeed > 0.0f && canRun)
            {
                float targetSpeed = horizontal * currentRunSpeed;
                // Apply target speed but do not modify y velocity
                rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
                spriteRenderer.flipX = horizontal > 0; // Flipping the sprite based on direction (note: < 0 for flip when moving left)
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

        }



        // Reset swimming animation if dash is complete
        if (dashElapsed >= dashDuration && animator.GetBool("isSwimming"))
        {
            animator.SetBool("isSwimming", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canRun = isGrounded;
        }

    }



    void TakeRecoilDamage()
    {
        currentHealth -= 10.0f;
        healthBar.SetHealth(currentHealth);
    }
}

