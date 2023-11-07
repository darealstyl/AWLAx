using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetX;
    public float offsetY;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
    }
}
