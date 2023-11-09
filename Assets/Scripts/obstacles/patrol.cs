using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    //idle time at the end of each patrol
    public float waitTime = 2f; 

    private bool movingToA = true;
    private float waitTimer;

    private void Start()
    {
        Turn();
    }

    void Update()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return; // Wait at the current point
        }

        if (movingToA)
        {
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToA = false;
                Turn();
                waitTimer = waitTime; // Set the timer for waiting
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToA = true;
                Turn();
                waitTimer = waitTime; // Set the timer for waiting
            }
        }
    }

    void Turn()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // Flip the sprite along the X-axis
        transform.localScale = newScale;
    }
}
