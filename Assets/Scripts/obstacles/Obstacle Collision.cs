using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public float damage = 1;
    private float lastDamageTime = -1;
    private float damageCooldown = 0.5f; // 1 second cooldown

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - lastDamageTime < damageCooldown)
        {
            return; // Skip if we're still within the cooldown period
        }

        if (collision.CompareTag("Sprite"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null && player.dashElapsed >= player.dashDuration)
            {
                player.TakeDamage(damage);
                 // Update the last damage time
            }

            player.canRun = false;
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            knockbackDirection.y = 0.8f; // Adjust the vertical component of the knockback
            if (knockbackDirection.x < 0)
            {
                knockbackDirection.x = -1;
            }
            else
            {
                knockbackDirection.x = 1;
            }

            float knockbackForce = 10.0f;
            rb.velocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            player.knockedBack = false;
            player.runAgain();
            Debug.Log("Knockback" + knockbackDirection);
        }
        lastDamageTime = Time.time;
    }
}
