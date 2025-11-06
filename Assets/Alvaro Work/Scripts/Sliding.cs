using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private CharacterController cc;
    private FPSController pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public InputAction slideAction;
    private float horizontalInput;
    private float verticalInput;

    private bool isSliding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<FPSController>();

        startYScale = playerObj.localScale.y;
    }


    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(slideAction.IsPressed() && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(slideAction.WasReleasedThisFrame() && isSliding)
        {
            StopSlide();
        }

        if(isSliding)
        {
            SlidingMovement();
        }
        
    }


    private void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }


    private void StopSlide()
    {

    }


    private void Awake()
    {
        slideAction = InputSystem.actions.FindAction("Slide");
    }


    private void OnEnable()
    {
        slideAction.Enable();
    }

    private void OnDisable()
    {
        slideAction.Disable();
    }


    
}
