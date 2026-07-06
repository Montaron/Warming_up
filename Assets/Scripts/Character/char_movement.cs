using UnityEngine;

public class char_movement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float backwardSpeedMultiplier = 0.5f;
    private CharacterController controller;

    public Vector2 inputVector { get; private set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
    }

    void Update()
    {
        if (cameraTransform == null)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(horizontal, vertical);

        // Camera forward & right (flattened)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Movement direction
        bool isMovingBackward = vertical < 0; 
        bool isStrafing = Mathf.Abs(horizontal) > 0.1f;

        Vector3 moveDirection;
        if (isMovingBackward && isStrafing)
        {
            moveDirection = camRight * horizontal;
        }
        else
            moveDirection = camForward * vertical + camRight * horizontal;


        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation;

            targetRotation = isMovingBackward && !isStrafing ? Quaternion.LookRotation(-moveDirection) : Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
        }
        // Move character
        controller.Move(moveDirection * moveSpeed * Time.deltaTime * (isMovingBackward ? backwardSpeedMultiplier : 1f));
    }
}
