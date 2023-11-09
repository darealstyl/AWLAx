using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public float damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming the other GameObject has a health script
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // Deal damage to the player
                player.TakeDamage(damage);
            }
        }
    }
}
