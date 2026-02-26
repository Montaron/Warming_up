using UnityEngine;
using System;
using JetBrains.Annotations;

public class char_mov_iso : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private CharacterController controller;
    private float currentSpeed;

    public Vector2 inputVector { get; private set; }
    public bool isAttacking { get; private set; }
    public bool attackFinished { get; private set; }

    public void OnAttackFinished()
    {
        Debug.Log("Attack animation finished, invoking event.");
        attackFinished = true;
        isAttacking = false;
    }
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (cameraTransform == null)
            return;
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            attackFinished = false;
            isAttacking = true;
        }
        
        
        if (!isAttacking)
        {
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
            bool isStrafing = Mathf.Abs(horizontal) > 0.1f;

            Vector3 moveDirection = camForward * vertical + camRight * horizontal;

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation;

                targetRotation = Quaternion.LookRotation(moveDirection);

                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
            }
            // Move character
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

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
