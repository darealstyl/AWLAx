using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private float dropThreshold = -1.0f; // Threshold value for teleportation
    [SerializeField] private Vector2 teleportPosition = new Vector2(-6.75f, 0.1f); // Target position for teleportation
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        // Get the Rigidbody2D component from the player
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player's y position is below the threshold
        if (transform.position.y < dropThreshold)
        {
            // Teleport the player to the specified position
            rb.position = teleportPosition;
            // Reset the player's velocity to zero
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
