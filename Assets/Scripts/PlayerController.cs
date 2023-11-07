using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    public HealthBar healthBar;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public float maxHealth;
    public float maxRunSpeed;
    public float jumpForce = 10.0f; // Set your desired jump force
    public float dashForce;
    public float dashDuration;

    float currentHealth;
    float currentRunSpeed;

    bool isGrounded; // A flag to check if the player is grounded
    public LayerMask groundLayer; // Set this in the inspector to the layer your ground is on
    public Transform groundCheck; // Assign a child GameObject to act as the ground check position
    public float groundCheckDistance = 0.2f; // Radius of the overlap circle to determine if grounded

    float horizontal = 0.0f;
    float vertical = 0.0f;
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

            //dashInput = Input.GetKeyDown(KeyCode.Return) ? !dashInput : dashInput;
            //jumpInput = Input.GetKeyDown(KeyCode.Space) && isGrounded ? !jumpInput : jumpInput;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                dashInput = true;
            }
            else
            {
                dashInput = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpInput = true;
            }
            else
            {
                jumpInput = false;
            }

        }
    }

    void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.Raycast(rb.position, Vector2.down, groundCheckDistance, groundLayer).collider != null;
        //
        //
        Debug.DrawRay(rb.position, Vector2.down * groundCheckDistance, Color.red);
        // If a jump is requested and the player is grounded then add a vertical force
        if (jumpInput && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpInput = false; // Reset the jump input flag
        }

        // Handle dashing
        if (dashInput && dashElapsed >= dashDuration)
        {
            Vector2 inputAxes = new Vector2(horizontal, vertical).normalized; // Assuming dash only happens horizontally
            rb.velocity = inputAxes * dashForce;
            dashElapsed = 0.0f;
            TakeRecoilDamage();
            animator.SetBool("isSwimming", true);
            dashInput = false; // Reset the dash input flag immediately
        }

        // Update the dashElapsed time if a dash has been initiated
        if (dashElapsed < dashDuration)
        {
            dashElapsed += Time.fixedDeltaTime;
            if (dashElapsed >= dashDuration)
            {
                animator.SetBool("isSwimming", false);
                rb.velocity = Vector2.zero; // Stop horizontal dash force
            }
        }

        // Running logic
        if (Mathf.Abs(horizontal) > 0.2f && currentRunSpeed > 0.0f)
        {
            rb.velocity = new Vector3(horizontal * currentRunSpeed, rb.velocity.y);
            spriteRenderer.flipX = horizontal > 0; // Flipping the sprite based on direction
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void TakeRecoilDamage()
    {
        currentHealth -= 10.0f;
        healthBar.SetHealth(currentHealth);
    }
}

