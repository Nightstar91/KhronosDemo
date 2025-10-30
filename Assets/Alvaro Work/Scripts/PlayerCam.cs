using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] public float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] public float lookSpeedY = 2f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80f;

    public Transform orientation;
    public Camera playerCamera;

    float rotationX;
    float rotationY;

    public InputActionAsset mouseAction;

    public InputAction m_lookAction;

    private Vector2 lookAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        m_lookAction = InputSystem.actions.FindAction("Look");
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleMouseLock();
    }


    private void OnEnable()
    {
        mouseAction.FindActionMap("PlayerMovement").Enable();
    }


    private void OnDisable()
    {
        mouseAction.FindActionMap("PlayerMovement").Disable();    
    }


    private void HandleMouseLock()
    {
        lookAmount = m_lookAction.ReadValue<Vector2>();

        rotationX -= lookAmount.x * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, lookAmount.x * lookSpeedX, 0);
    }
}
