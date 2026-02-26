using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_iso : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        transform.position = target.position + offset;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
