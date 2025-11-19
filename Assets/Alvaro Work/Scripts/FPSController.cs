using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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
    public PlayerState previousState = PlayerState.STATE_IDLE;

    public bool CanMove { get; private set; } = true;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction pauseAction;
    public InputAction slideAction;

    [Header("Movement Parameters")]
    [SerializeField] public float walkSpeed = 3f;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float sprintSpeed = 0.09f;
    [SerializeField] float decelerateSpeed = 0.75f;

    [Header("Jumping Parameters")]
    [SerializeField] public float gravity = 30f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float jumpHeight = 1.5f;

    [Header("Look Parameters")]
    [SerializeField, Range(0.1f, 1)] public float lookSpeedX = 0.2f;
    [SerializeField, Range(0.1f, 1)] public float lookSpeedY = 0.2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    public Camera playerCamera;
    public GameObject playerCameraHolder;
    public CharacterController characterController;
    [SerializeField] public GameObject orientation;

    public Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0f;

    private const float originalWalkSpeed = 0f;

    public LayerMask groundLayer;
    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isInAir = false;

    // DELETE ALL INSTANCE OF PLAYER HUD LATER, FOR REFACTORING PLAYER MOVEMENT TO USE INPUTACTION FOR PAUSING
    public PlayerHud playerHud;
    private Sliding slide;
    private CameraEffect cameraEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCameraHolder = GameObject.Find("Cam Holder");
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        groundLayer = LayerMask.GetMask("Ground");

        Vector3 horizontalVelocity = characterController.velocity;

        lookSpeedX = PlayerPrefs.GetFloat("Sensitivity", 2);
        lookSpeedY = PlayerPrefs.GetFloat("Sensitivity", 2);
        playerCamera.fieldOfView = PlayerPrefs.GetFloat("Fov", 50);
    }


    void Update()
    {
        
        switch (currentState)
        {
            case PlayerState.STATE_IDLE:
                HandleMouseLock();
                slide.HandleSlideCooldown();

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

                slide.HandleSlideCooldown();

                if (!isMoving)
                {
                    currentState = PlayerState.STATE_IDLE;
                }

                if(isInAir)
                {
                    currentState = PlayerState.STATE_INAIR;
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

                //Condtional to transition to the slide state
                if (slideAction.IsPressed() && slide.slideReady)
                {
                    currentState = PlayerState.STATE_SLIDE;
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
                slide.HandleSlideCooldown();

                if (isGrounded && isInAir)
                {
                    isInAir = false;
                    currentState = PlayerState.STATE_RUNNING;
                }

                if (pauseAction.WasPressedThisFrame() && !playerHud.isPaused)
                {
                    playerHud.isPaused = true;
                    previousState = currentState;
                    currentState = PlayerState.STATE_PAUSE;
                }
                break;

            case PlayerState.STATE_SLIDE:

                if(slideAction.IsPressed())
                {
                    slide.StartSlide();
                }

                if (slide.isSliding)
                {
                    cameraEffect.StartSwayCamera(10f);
                    cameraEffect.ShakeCamera();
                    slide.SlidingMovement();
                    slide.SlideCountdown();
                }

                if (slideAction.WasReleasedThisFrame() || !slide.isSliding)
                {
                    slide.StopSlide();

                    currentState = PlayerState.STATE_RUNNING;
                }

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

        // To make sure gravity is applied constantly
        ApplyFinalMovements();
    }



    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        pauseAction = InputSystem.actions.FindAction("Pause");
        slideAction = InputSystem.actions.FindAction("Slide");

        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerHud = GameObject.Find("HudController").GetComponent<PlayerHud>();
        slide = GetComponent<Sliding>();
        cameraEffect = GameObject.Find("Cam Holder").GetComponent<CameraEffect>();

    }


    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        pauseAction.Enable();
        slideAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        pauseAction.Disable();
        slideAction.Disable();
    }


    private void HandleMovementInput()
    {
        if (moveAction.IsPressed())
            isMoving = true;
        else
            isMoving = false;

        GainSpeedCheck();

        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }


    private void HandleMouseLock()
    {
        rotationX -= lookAction.ReadValue<Vector2>().y * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, lookAction.ReadValue<Vector2>().x * lookSpeedX, 0);

        // rotate the player object
        transform.Rotate(0f, lookAction.ReadValue<Vector2>().x * lookSpeedX, 0f);
    }

    /*private void HandleMouseLock()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

        // rotate the player object
        transform.Rotate(0f, Input.GetAxis("Mouse X") * lookSpeedX, 0f);
    }*/


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
            walkSpeed = Mathf.SmoothDamp(walkSpeed, maxSpeed, ref sprintSpeed, 0.5f);

        else if (walkSpeed > maxSpeed && isMoving)
            walkSpeed = maxSpeed;

        else
            walkSpeed = originalWalkSpeed;
    }


    private void SpeedDecelerate()
    {
            
        
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

}