using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{

    [Header("Target")]
    public Transform target;

    [Header("Rotation settings")]
    public float mouseSensitivity = 3f;
    public float minYAngle = -30f;
    public float maxYAngle = 70f;

    [Header("Distance")]
    public float distance = 5f;

    private float currentX = 0f;
    private float currentY = 20f;

    [Header("Collision settings")]
    public float collisionRadius = 0.3f;
    public float collisionSmooth = 10f;

    void LateUpdate()
    {
        if (target == null)
            return;

        if (Input.GetMouseButton(0)) // Left mouse button
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distance);

        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }
}
