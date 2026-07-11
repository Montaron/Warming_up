using UnityEngine;
using System;

public class CharacterMovement_iso : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private CharacterController controller;
    private float currentSpeed;

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
    public void MoveCharacterForward()
    {
        controller.Move(transform.forward * currentSpeed * Time.deltaTime);
    }
    public void MoveCharacter()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

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
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void Initialize()
    {
        if (cameraTransform == null)
        {
            // Debug.LogError("Camera Transform is not assigned in char_mov_iso.");
        }
        inputVector = Vector2.zero;
        moveDirection = Vector3.zero;
        currentSpeed = moveSpeed;
        moveDirection = cameraTransform.forward;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log("char_mov -Collision with: " + hit.gameObject.name);
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
}
