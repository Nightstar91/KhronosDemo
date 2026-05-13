using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name Reference")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string slide = "Slide";
    [SerializeField] private string pause = "Pause";

    private InputAction movementAction;
    private InputAction lookAction;
    public InputAction jumpAction;
    private InputAction slideAction;
    private InputAction pauseAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SlideTriggered { get; private set; }
    public bool PauseTriggered { get; private set; }


    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        lookAction = mapReference.FindAction(look);
        jumpAction = mapReference.FindAction(jump);
        slideAction = mapReference.FindAction(slide);
        pauseAction = mapReference.FindAction(pause);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        lookAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        lookAction.canceled += inputInfo => MovementInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        slideAction.performed += inputInfo => SlideTriggered = true;
        slideAction.canceled += inputInfo => SlideTriggered = false;

        pauseAction.performed += inputInfo => PauseTriggered = true;
        pauseAction.canceled += inputInfo => PauseTriggered = false;
    }


    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}
