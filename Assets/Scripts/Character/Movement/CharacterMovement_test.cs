using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_test : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // how snappy the turning is

    public Camera mainCamera;

    CharacterController controller;
    Vector3 mouseWorldPos;
    public float zOffset = 0.1f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (GetMouseWorldPosition(out mouseWorldPos))
            {
                Vector3 direction = mouseWorldPos - transform.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = targetRotation;
                }
            }
        }
        else 
        {
            Transform cameraTransform = mainCamera.transform;
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            // Movement direction
            Vector3 moveDirection;
            moveDirection = camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal");

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation;

                targetRotation = Quaternion.LookRotation(moveDirection);

                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
            }
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        } 

    }
    bool GetMouseWorldPosition(out Vector3 hit)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // plane at y=0, facing up

        if (groundPlane.Raycast(ray, out float distance))
        {
            hit = ray.GetPoint(distance);
            return true;
        }
        hit = Vector3.zero; // must assign hit on every path, even when returning false
        return false;
    }
}