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

    [SerializeField] private float regenRate = 0.08f; // Add this to control the regen rate

    float currentHealth;
    float currentRunSpeed;

    bool isGrounded = false; // A flag to check if the player is grounded
    public LayerMask groundLayer; // Set this in the inspector to the layer your ground is on
    public Transform groundCheck; // Assign a child GameObject to act as the ground check position
    public float groundCheckDistance = 0.2f; // Radius of the overlap circle to determine if grounded

    float horizontal = 0.0f;
    float vertical = 0.0f;
    bool dashInput = false;
    bool jumpInput = false; // Flag to check if jump was requested
    float dashElapsed;

    public LevelTimer levelTimer;
    private bool hasDashed;
    public bool death;

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
        death = false;
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


            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpInput = true;
            }

            
        }

    }

    void FixedUpdate()
    {
        if (levelTimer.levelStarted)
        {
            // Check if the player is grounded
            isGrounded = Physics2D.Raycast(rb.position, Vector2.down, groundCheckDistance, groundLayer).collider != null;
            Debug.DrawRay(rb.position, Vector2.down * groundCheckDistance, Color.red);
        }

        if (isGrounded)
        {
            hasDashed = false;
        }

        // If a jump is requested and the player is grounded then add a vertical force
        if (jumpInput && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpInput = false; // Reset the jump input flag
        }

        // Handle dashing
        if (dashInput && dashElapsed >= dashDuration && !hasDashed)
        {
            Vector2 inputAxes = new Vector2(horizontal, vertical).normalized;
            rb.velocity += inputAxes * dashForce; // Apply the dash force in the input direction
            dashElapsed = 0.0f; // Reset dash timer
            TakeRecoilDamage();
            animator.SetBool("isSwimming", true);
            dashInput = false; // Reset the dash input flag immediately
            hasDashed = true;
        }
        else if (dashElapsed < dashDuration)
        {
            dashElapsed += Time.fixedDeltaTime;
        }
        else
        {
            // Ensure the dashElapsed timer does not exceed the dashDuration + threshold to avoid small deltaTime additions
            dashElapsed = Mathf.Min(dashElapsed, dashDuration + 0.01f);

            // Running logic
            if (Mathf.Abs(horizontal) > 0.2f && currentRunSpeed > 0.0f)
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

        if (dashInput && !isGrounded)
        {
            dashInput = false;
        }

        // Reset swimming animation if dash is complete
        if (dashElapsed >= dashDuration && animator.GetBool("isSwimming"))
        {
            animator.SetBool("isSwimming", false);
        }
        if (!death)
        {
            regenHealth();
        }
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            death = true;
            levelTimer.levelStarted = false;
            rb.velocity = new Vector2(0, 0);
        }
    }

    void regenHealth()
    {
        currentHealth += regenRate;
        currentHealth = Mathf.Clamp(currentHealth, -1.0f, maxHealth);
        healthBar.SetHealth(currentHealth);
    }


    void TakeRecoilDamage()
    {
        currentHealth -= 10.0f;
        currentHealth = Mathf.Clamp(currentHealth, -1.0f, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}

