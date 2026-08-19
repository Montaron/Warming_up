using UnityEngine;
using System;

public class CharacterMovement_iso : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private CharacterController controller;
    public float currentSpeed;

    public Vector2 inputVector { get; private set; }
    public Vector3 moveDirection { get; private set; }
    public event Action<Collider> OnHitObstacle;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        Initialize();
    }

    public void SetInput(Vector2 input)
    {
        inputVector = input;
    }

    public Vector3 GetMouseDirection()
    {
        if (GetMouseWorldPosition(out Vector3 mouseWorldPos))
        {
            Vector3 direction = mouseWorldPos - transform.position;
            direction.y = 0f;
            direction.Normalize();
            return direction;
        }
        return Vector3.zero;
    }
    public void MoveCharacterForward()
    {
        controller.Move(transform.forward * currentSpeed * Time.deltaTime);
    }
    public void MoveCharacterTo(Vector3 direction)
    {
        controller.Move(direction * currentSpeed * Time.deltaTime);
    }
    public void OrientCharacter()
    {
        if (GetMouseWorldPosition(out Vector3 mouseWorldPos))
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
    public void MoveCharacter()
    {
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Movement direction
        moveDirection = camForward * inputVector.y + camRight * inputVector.x;

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
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
    void Initialize()
    {
        if (mainCamera == null)
        {
            // Debug.LogError("Camera Transform is not assigned in char_mov_iso.");
        }
        inputVector = Vector2.zero;
        moveDirection = Vector3.zero;
        currentSpeed = moveSpeed;
        moveDirection = mainCamera.transform.forward;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        OnHitObstacle?.Invoke(hit.collider);
    }
    
    public void ModifySpeed(float multiplier)
    {
        currentSpeed = moveSpeed * multiplier;
    }
    public void ResetSpeed()
    {
        currentSpeed = moveSpeed;
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
