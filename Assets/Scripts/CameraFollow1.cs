using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Camera boundary variables
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp the camera's position
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        smoothedPosition.z = -10f;

        transform.position = smoothedPosition;
    }
}
