using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public HealthBar healthBar;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public float maxHealth;
    public float maxRunSpeed;
    public float dashForce;
    public float dashDuration;

    float currentHealth;
    float currentRunSpeed;

    float horizontal = 0.0f;
    float vertical = 0.0f;
    bool dashInput = false;
    float dashElapsed;


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
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        dashInput = !dashInput ? Input.GetKeyDown(KeyCode.Space) : dashInput;
    }

    void FixedUpdate()
    {
        if (dashInput)
        {
            if (dashElapsed >= dashDuration)
            {
                Vector2 inputAxes = new Vector2(horizontal, vertical).normalized;
                rb.velocity = inputAxes * dashForce;
                dashElapsed = 0.0f;
                animator.SetBool("isSwimming", true);
            }
            else
            {
                dashElapsed += Time.fixedDeltaTime;
                if (dashElapsed >= dashDuration)
                {
                    dashInput = false;
                    animator.SetBool("isSwimming", false);
                    rb.velocity = Vector2.zero;
                }
            }
        }


        if (!dashInput)
        {
            if (Mathf.Abs(horizontal) > 0.2f && currentRunSpeed > 0.0f)
            {
                transform.position += new Vector3(horizontal * currentRunSpeed, 0) * Time.fixedDeltaTime;
                spriteRenderer.flipX = horizontal > 0.2f;
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }


    }

    public void TakeDamage(float damage)
    {
        currentHealth -= 10.0f;
        healthBar.SetHealth(currentHealth);
    }
}
