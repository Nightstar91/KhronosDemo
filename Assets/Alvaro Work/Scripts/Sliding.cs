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
    private float originalSlideTimer;
    private float originalSlideCooldown;
    [Range(0f,4f)] public float slideTimer = 1.5f;
    [Range(0f, 4f)] public float slideCooldown = 3f;
    [Range(1, 30)] public float slideForce;


    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    public bool isSliding;
    public bool slideReady;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSliding = false;
        slideReady = true;
        startYScale = playerObj.localScale.y;
        slideForce = 3f;
        originalSlideTimer = slideTimer;
        originalSlideCooldown = slideCooldown;
    }


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<FPSController>();
        playerObj = GameObject.Find("Player").GetComponent<Transform>();
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


    public void SlideCountdown()
    {
        if (slideTimer >= 0)
        {
            slideTimer -= 1 * Time.deltaTime;
        }
        else
        {
            isSliding = false;
        }
    }


    public void HandleSlideCooldown()
    {
        if (slideCooldown >= 0 && !slideReady)
        {
            slideCooldown -= 1 * Time.deltaTime;
        }
        else
        {
            slideCooldown = originalSlideCooldown;
            slideReady = true;
        }
    }


    public void StopSlide()
    {
        slideTimer = originalSlideTimer;

        slideReady = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

 
}
