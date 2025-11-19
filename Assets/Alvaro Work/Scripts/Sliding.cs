using Unity.Cinemachine;
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
    public float angle;
    private float speedAcceleration;
    private const float speedCap = 30f;
    private Vector3 slideVelocity;
    [Range(0f,4f)] public float slideTimer = 1.5f;
    [Range(0f, 4f)] public float slideCooldown = 3;
    [Range(1, 30)] public float slideForce;


    public float slideYScale;
    private float startYScale;
    private const float slopeDamp = 2f;

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
        playerOrientation = GameObject.Find("SlopeOrientation").GetComponent<Transform>();
        slideForce = 1f;
        angle = 0f;
        slideForce = originalSlideForce;
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

        if (OnSlope(out Vector3 slopeDirection))
        {
            slideVelocity += slopeDirection * speedAcceleration * Time.deltaTime;
            slideVelocity = Vector3.ClampMagnitude(slideVelocity, speedAcceleration);
            cc.Move(slideVelocity * Time.deltaTime);
        }
        else
        {
            cc.Move(inputDirection.normalized * Time.deltaTime);
        }
    }


    public void SlideCountdown()
    {
        if (slideTimer >= 0)
        {
            slideTimer -= 1 * Time.deltaTime;
        }
        //else if(slideTimer >= 0 && angle)
        //{

        //}

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

        slideReady = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        playerOrientation.rotation = startPlayerRotation;
    }


    private bool OnSlope(out Vector3 slopeDirection)
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2))
        {
            slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            return Vector3.Angle(hit.normal, Vector3.up) > 0f;
        }

        slopeDirection = Vector3.zero;
        return false;
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


    private void SlopeBasedAcceleration()
    {
        angle = Quaternion.Angle(transform.rotation, playerOrientation.rotation);

        if(angle != 0)
        {
            slideForce += angle / slopeDamp;
            slideForce = Mathf.Clamp(slideForce, 0, 100);
            Debug.Log(string.Format("slideforce = {0}", slideForce));
        }
    }
}
