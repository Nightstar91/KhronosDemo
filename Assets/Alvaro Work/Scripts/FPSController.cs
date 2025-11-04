using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    public enum PlayerState
    {
        STATE_IDLE,
        STATE_RUNNING,
        STATE_JUMP,
        STATE_INAIR,
        STATE_SLIDE,
        STATE_WALLRUN,
        STATE_PAUSE,
        STATE_DEAD
    }
    
    public PlayerState currentState = PlayerState.STATE_IDLE;
    public PlayerState previousState;

    public bool canMove { get; private set; } = true;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction pauseAction;

    [Header("Movement Parameters")]
    [SerializeField] public float walkSpeed = 3f;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float sprintSpeed = 0.09f;

    [Header("Jumping Parameters")]
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float downwardVelocity;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] public float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] public float lookSpeedY = 2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    public Camera playerCamera;
    public CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;

    private float originalWalkSpeed;

    public LayerMask groundLayer;
    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isInAir = false;

    // DELETE ALL INSTANCE OF PLAYER HUD LATER, FOR REFACTORING PLAYER MOVEMENT TO USE INPUTACTION FOR PAUSING
    public PlayerHud playerHud;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalWalkSpeed = walkSpeed;
        groundLayer = LayerMask.GetMask("Ground");

        Vector3 horizontalVelocity = characterController.velocity;

        playerHud = GameObject.Find("HudController").GetComponent<PlayerHud>();

        lookSpeedX = PlayerPrefs.GetFloat("Sensitivity", 2);
        lookSpeedY = PlayerPrefs.GetFloat("Sensitivity", 2);
        playerCamera.fieldOfView = PlayerPrefs.GetFloat("Fov", 50);
    }


    // Updatwasddawdgfe is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.STATE_IDLE:
                HandleMouseLock();

                // Once moving go to running state to apply velocity limit
                if (moveAction.triggered)
                {
                    currentState = PlayerState.STATE_RUNNING;
                }

                // Conditional to get to the jump state
                if (jumpAction.IsPressed() && isGrounded)
                {
                    currentState = PlayerState.STATE_JUMP;
                }

                // Conditional to transition to the pause state
                if (pauseAction.WasPressedThisFrame() && !playerHud.isPaused)
                {
                    playerHud.isPaused = true;
                    previousState = currentState;
                    currentState = PlayerState.STATE_PAUSE;
                }

                break;

            case PlayerState.STATE_RUNNING:
                HandleMouseLock();
                HandleMovementInput();

                if (!isMoving)
                {
                    currentState = PlayerState.STATE_IDLE;
                }

                // Conditional for jumping to got to jump state 
                if (jumpAction.IsPressed() && isGrounded)
                {
                    currentState = PlayerState.STATE_JUMP;
                }

                // Conditional to transition to the pause state
                if (pauseAction.WasPressedThisFrame() && !playerHud.isPaused)
                {
                    playerHud.isPaused = true;
                    previousState = currentState;
                    currentState = PlayerState.STATE_PAUSE;
                }

                break;

            // TODO: See if can make it so that air movement can be lessen
            // TODO: how do you implement jumping in state??
            case PlayerState.STATE_JUMP:

                Jump();

                isInAir = true;
                currentState = PlayerState.STATE_INAIR;

                break;

            case PlayerState.STATE_INAIR:
                HandleMouseLock();
                HandleMovementInput(); // change to air movement

                if (isGrounded && isInAir)
                {
                    isInAir = false;
                    currentState = PlayerState.STATE_RUNNING;
                }
                break;

            case PlayerState.STATE_SLIDE:

                break;

            case PlayerState.STATE_WALLRUN:

                break;

            case PlayerState.STATE_PAUSE:
                if(playerHud.isPaused)
                {
                    playerHud.PauseGame();

                    if(pauseAction.WasPressedThisFrame())
                    {
                        playerHud.isPaused = false;
                    }
                }
                else
                {
                    playerHud.ResumeGame();
                    currentState = previousState;
                    previousState = PlayerState.STATE_IDLE;
                }
                    break;  

            case PlayerState.STATE_DEAD:

                break;
        }

        ApplyFinalMovements();
    }



    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        pauseAction = InputSystem.actions.FindAction("Pause");

        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        pauseAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        pauseAction.Disable();
    }


    private void HandleMovementInput()
    {
        GainSpeedCheck();

        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;

        // Limit velocity to max speed
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

        isGrounded = CheckIfGrounded();
    }


    private void GainSpeedCheck()
    {
        if (walkSpeed < maxSpeed && isMoving)
            walkSpeed = Mathf.SmoothDamp(walkSpeed, maxSpeed,ref sprintSpeed, 0.5f);

        else if (walkSpeed > maxSpeed && isMoving)
            walkSpeed = maxSpeed;

        else
            walkSpeed = originalWalkSpeed;

    }


    public bool CheckIfGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out hit, 1.1f, groundLayer))
            return true;
        else
            return false;
    }

    public void Jump()
    {
        moveDirection.y = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
    }


    public void Slide()
    {

    }
}