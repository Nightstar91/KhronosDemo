using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float gravity = 30f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    public Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;

    private float originalWalkSpeed;
    private const float sprintSpeed = 0.075f;
    private const float maxSpeed = 15f;

    public bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalWalkSpeed = walkSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            HandleMouseLock();
            HandleMovementInput();
            ApplyFinalMovements();
        }


    }


    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }


    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }


    private void HandleMovementInput()
    {
        GainSpeedCheck();

        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;

        if (moveAction.IsPressed())
            isMoving = true;
        else
            isMoving = false;
    }


    private void HandleMouseLock()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);


    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }


    private void GainSpeedCheck()
    {
        if (walkSpeed < maxSpeed && isMoving)
            walkSpeed += sprintSpeed;

        else if (walkSpeed >= maxSpeed && isMoving)
            walkSpeed = maxSpeed;

        else
            walkSpeed = originalWalkSpeed;

    }
}