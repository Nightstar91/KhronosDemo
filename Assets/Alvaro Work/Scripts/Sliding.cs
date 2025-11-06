using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform playerObj;
    private CharacterController cc;
    private FPSController pm;

    [Header("Sliding")]
    public float maxSlideTime;
    [Range(1,30)] public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    public bool isSliding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSliding = false;
        startYScale = playerObj.localScale.y;
        slideForce = 3f;
    }


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<FPSController>();
        playerObj = GameObject.Find("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if(pm.slideAction.IsPressed() && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(pm.slideAction.WasReleasedThisFrame() && isSliding)
        {
            StopSlide();
        }

        if(isSliding)
        {
            SlidingMovement();
        }
        
    }


    public void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
    }


    public void SlidingMovement()
    {
        Vector3 inputDirection = playerObj.forward * verticalInput + playerObj.right * horizontalInput;

        if (!cc.isGrounded)
            inputDirection.y -= pm.gravity * Time.deltaTime;

        cc.Move(inputDirection.normalized * slideForce * Time.deltaTime);
    }


    public void StopSlide()
    {
        isSliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

 
}
