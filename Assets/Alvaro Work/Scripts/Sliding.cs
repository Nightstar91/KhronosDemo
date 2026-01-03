using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform playerObj;
    private CharacterController cc;
    private FPSController pm;
    private Transform playerOrientation;
    private Quaternion startPlayerRotation;

    [Header("Sliding")]
    private float originalSlideTimer;
    private float originalSlideCooldown;
    private float originalSlideForce;
    float slopeAlignment;
    public float angle;
    private float currentSlideSpeed; // Track current speed (AI)
    [Range(5f, 15f)] public float baseSlideSpeed = 10f; // Base sliding speed (AI)
    [Range(0f, 50f)] public float maxSlideSpeed = 30f; // Maximum slide speed (AI)
    [Range(0f, 100f)] public float downhillAcceleration = 5f; // Acceleration going downhill (AI)
    [Range(0f, 100f)] public float uphillDeceleration = 8f; // Deceleration going uphill (AI)
    [Range(0f, 4f)] public float slideTimer = 1.5f;
    [Range(0f, 4f)] public float slideCooldown = 1.8f;
    [SerializeField] public float slideForce;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    public bool isSliding;
    public bool slideReady;

    void Start()
    {
        isSliding = false;
        slideReady = true;
        startYScale = playerObj.localScale.y;
        playerOrientation = GameObject.Find("SlopeOrientation").GetComponent<Transform>();
        slideForce = 1f;
        angle = 0f;
        slideForce = originalSlideForce;
        originalSlideTimer = slideTimer;
        originalSlideCooldown = slideCooldown;
        currentSlideSpeed = baseSlideSpeed; // (AI)
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
        currentSlideSpeed = baseSlideSpeed; // Initialize speed when starting slide (AI)
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
    }

    public void SlidingMovement()
    {
        Vector3 moveDirection;

        if (OnSlope(out Vector3 slopeDirection, out float slopeAngle))
        {
            // Calculate angle and adjust speed based on slope
            CalculateAngle();
            ApplySlopeAcceleration(slopeAngle);

            // Move in the direction of the slope
            moveDirection = slopeDirection * currentSlideSpeed;
        }
        else
        {
            // Flat ground - use input direction
            moveDirection = (playerObj.forward * verticalInput + playerObj.right * horizontalInput) * currentSlideSpeed;

            if (currentSlideSpeed > baseSlideSpeed)
            {
                // Decay over time
                currentSlideSpeed = Mathf.Lerp(currentSlideSpeed, baseSlideSpeed, Time.deltaTime * 1f);
            }

            
        }
        moveDirection.y -= pm.gravity * Time.deltaTime;

        pm.SetVelocity(moveDirection);

        cc.Move(moveDirection * Time.deltaTime);
    }


    private void ApplySlopeAcceleration(float slopeAngle)
    {
        // Determine if going downhill or uphill based on slope angle
        if (slopeAngle > 3f) // On a noticeable slope
        {
            // Check if moving downhill (slope direction points down relative to player forward)
            Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, GetSlopeNormal()).normalized;
            float slopeAlignment = Vector3.Dot(playerObj.forward, slopeDir);

            // Accelerating downhill
            if (slopeAlignment > 0.1f)
            {
                float accelerationAmount = (slopeAngle / 45f) * downhillAcceleration;
                currentSlideSpeed += accelerationAmount * Time.deltaTime;
            }
            // Decelerating on uphill
            else if (slopeAlignment < -0.1f) 
            {
                float decelerationAmount = (slopeAngle / 45f) * uphillDeceleration;
                currentSlideSpeed -= decelerationAmount * Time.deltaTime;
            }

            currentSlideSpeed = Mathf.Clamp(currentSlideSpeed, baseSlideSpeed * 0.5f, maxSlideSpeed);

            Debug.Log($"Slope: {slopeAngle:F1}° | Speed: {currentSlideSpeed:F1} | Alignment: {slopeAlignment:F2}");
        }
    }


    public void SlideCountdown()
    {
        if (slideTimer >= 0 && angle == 0)
        {
            slideTimer -= 1 * Time.deltaTime;
        }
        else if (slideTimer >= 0 && angle > 3f)
        {
            slideTimer = 1.5f;
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
        slideForce = originalSlideForce;
        currentSlideSpeed = baseSlideSpeed; 
        slideReady = false;
        angle = 0;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        playerOrientation.rotation = startPlayerRotation;
    }


    private bool OnSlope(out Vector3 slopeDirection, out float slopeAngle)
    {
        slopeDirection = Vector3.zero;
        slopeAngle = 0;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle < 3f) 
            {
                return false;
            }

            slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            angle = slopeAngle; 
            return true;
        }

        return false;
    }


    // Helper method to get slope normal
    private Vector3 GetSlopeNormal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            return hit.normal;
        }
        return Vector3.up;
    }


    public void CalculateAngle()
    {
        startPlayerRotation = playerOrientation.transform.rotation;

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, -(transform.up), out hit, 1, pm.groundLayer))
        {
            playerOrientation.transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
        }
    }
}