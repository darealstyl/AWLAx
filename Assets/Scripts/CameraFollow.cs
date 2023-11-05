using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }
}
