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
    public event Action OnHitObstacle;
    public bool LockDirection { get; set; }
    public bool LockMovement { get; set; }
    public bool ForceForward { get; set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        Initialise();
    }

    Vector2 GetMovInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector2(horizontal, vertical);
    }

    void MoveCharacter()
    {

        inputVector = GetMovInput();
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Movement direction
        if (!LockDirection)
        {
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
        }
        if (!LockMovement && ForceForward)
        {
            controller.Move(transform.forward * currentSpeed * Time.deltaTime);
        }
        else if (!LockMovement)
        {
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        MoveCharacter();
    }

    void Initialise()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned in char_mov_iso.");
        }
        LockMovement = false;
        LockDirection = false;
        LockMovement = false;
        currentSpeed = moveSpeed;
        moveDirection = cameraTransform.forward;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("char_mov -Collision with: " + hit.gameObject.name);
        OnHitObstacle?.Invoke();
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
