using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            playerController.canRun = playerController.isGrounded;
        }
    }
}