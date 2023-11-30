using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{

    public ParticleSystem dustEffect;

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
    public float dashCooldown = 0.5f;

    [SerializeField] private float regenRate = 0.08f; // Add this to control the regen rate

    float currentHealth;
    float currentRunSpeed;

    public bool isGrounded = false; // A flag to check if the player is grounded
    public LayerMask groundLayer; // Set this in the inspector to the layer your ground is on
    public Transform groundCheck; // Assign a child GameObject to act as the ground check position
    public float groundCheckDistance = 0.2f; // Radius of the overlap circle to determine if grounded

    float horizontal = 0.0f;
    float vertical = 0.0f;
    public bool canRun = true;
    bool dashInput = false;
    bool jumpInput = false; // Flag to check if jump was requested
    public float dashElapsed;
    float dashCooldownElapsed;

    public LevelTimer levelTimer;
    private bool hasDashed;
    public bool death;

    public float rotationClamp = 15.0f;

    private float previousHorizontal = 0.0f;
    public bool knockedBack;

    bool isJumping;
    public float maxJumpTime = 0.5f; // Set your desired max jump time
    private float jumpTimeCounter;
    public float fallMultiplier = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentRunSpeed = maxRunSpeed;
        dashElapsed = dashDuration;
        dashCooldownElapsed = dashCooldown;
        death = false;
        knockedBack = false;
        StaticManager.hasDoneTutorial = true;
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
            /*
                        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                        {
                            dustEffect.Play();
                            jumpInput = true;
                        }*/

            //Debug.Log("Velocity.x: " + rb.velocity.x);
            //Debug.Log("Velocity.y: " + rb.velocity.y);

            float halfColliderWidth = boxCollider.size.x / 2.9f;
            if (!knockedBack)
            {
                isGrounded = Physics2D.Raycast(rb.position, Vector2.down, groundCheckDistance, groundLayer).collider != null
                || Physics2D.Raycast(rb.position + new Vector2(halfColliderWidth, 0), Vector2.down, groundCheckDistance, groundLayer).collider != null
                || Physics2D.Raycast(rb.position + new Vector2(-halfColliderWidth, 0), Vector2.down, groundCheckDistance, groundLayer).collider != null;
                Debug.DrawRay(rb.position, Vector2.down * groundCheckDistance, Color.red);
                Debug.DrawRay(rb.position + new Vector2(halfColliderWidth, 0), Vector2.down * groundCheckDistance, Color.red);
                Debug.DrawRay(rb.position + new Vector2(-halfColliderWidth, 0), Vector2.down * groundCheckDistance, Color.red);
            }
            //Debug.Log("isGrounded: " + isGrounded);

            if (isGrounded && ((horizontal > 0 && previousHorizontal <= 0) || (horizontal < 0 && previousHorizontal >= 0)))
            {
                dustEffect.Play();
            }
            previousHorizontal = horizontal;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && dashElapsed >= dashDuration)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = Vector2.up * jumpForce;
            dustEffect.Play();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping && dashElapsed >= dashDuration)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                Debug.Log("jumpTimeCounter: " + jumpTimeCounter);
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && dashElapsed >= dashDuration)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 10);
        }

        if (levelTimer.levelStarted && levelTimer.GetCurrentTime() <= 0f)
        {
            Die();
        }

    }

    void FixedUpdate()
    {
        if (!knockedBack)
        {
            // Handle dashing
            if (dashInput && dashElapsed >= dashDuration && dashCooldownElapsed >= dashCooldown && ((horizontal > 0.5f || horizontal < -0.5f) || (vertical < -0.5f || vertical > 0.5f)))
            {
                Vector2 inputAxes = new Vector2(horizontal, vertical).normalized;
                rb.velocity += inputAxes * dashForce; // Apply the dash force in the input direction
                dashElapsed = 0.0f; // Reset dash timer
                TakeRecoilDamage();
                animator.SetBool("isSwimming", true);
                dashInput = false; // Reset the dash input flag immediately
                boxCollider.enabled = false;
            }
            // Dashing animation started
            else if (dashElapsed < dashDuration)
            {
                dashElapsed += Time.fixedDeltaTime;
                if (dashElapsed >= dashDuration)
                {
                    dashInput = false;
                    animator.SetBool("isSwimming", false);
                    dashCooldownElapsed = 0.0f;
                }
            }
            else if (dashElapsed >= dashDuration)
            {

                if (dashCooldownElapsed < dashCooldown)
                {
                    dashCooldownElapsed += Time.fixedDeltaTime;
                }
                else
                {
                    boxCollider.enabled = true;
                }



                // If a jump is requested and the player is grounded then add a vertical force
                if (jumpInput && isGrounded)
                {
                    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                    jumpInput = false; // Reset the jump input flag
                }

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
        }

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
            Die();
        }

        rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x, -25f, 25f),
                Mathf.Clamp(rb.velocity.y, -25f, 25f));

        if (death)
        {
            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

        if (isGrounded)
        {
            // Smoothly interpolate the rotation back to 0 when grounded
            float currentZRotation = transform.eulerAngles.z;
            // Use Mathf.LerpAngle to interpolate between the current rotation and 0
            float newZRotation = Mathf.LerpAngle(currentZRotation, 0, Time.fixedDeltaTime * 20.0f); // Adjust the multiplier to change the smoothing speed
            transform.eulerAngles = new Vector3(0, 0, newZRotation);
        }
        else
        {
            RotatePlayerBasedOnVelocity();
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Clamp the falling velocity
        if (rb.velocity.y < -30)
        {
            rb.velocity = new Vector2(rb.velocity.x, -30);
        }

    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canRun = isGrounded;
            //rb.velocity.x = 0;
        }

        // Reset swimming animation if dash is complete

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            canRun = isGrounded;

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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, -1.0f, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
    void RotatePlayerBasedOnVelocity()
    {
        // Calculate rotation amount based on y velocity
        float rotation = rb.velocity.y * 5.0f; // Change the multiplier to adjust the sensitivity

        // If the player is moving left (negative x velocity), invert the rotation angle
        if (rb.velocity.x < 0)
        {
            rotation *= -1.0f;
        }

        rotation = Mathf.Clamp(rotation, -rotationClamp, rotationClamp); // Clamp the rotation between +/- rotationClamp degrees

        // Apply the rotation
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    public void runAgain()
    {
        Invoke("knocked", 0.6f);
    }

    void knocked()
    {
        knockedBack = false;
        Debug.Log("knockedBack" + knockedBack);
    }

    void Die()
    {
        death = true;
        levelTimer.levelStarted = false;
        rb.velocity = new Vector2(0, 0);
    }

}

