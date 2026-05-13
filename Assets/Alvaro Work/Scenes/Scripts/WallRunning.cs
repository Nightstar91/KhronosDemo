using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using static FPSController;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wallLayer;
    public bool isWallRunning;
    public float wallrunForce = 7f;
    public float wallrunGravity = 5f;
    private float wallRunCooldownWithPenalty;
    private float maxWallRunCooldown;
    private float maxWallRunTimer;
    private const float cooldownPenalty = 0.25f;
    [SerializeField] public float wallBounceForce = 1.5f;
    [SerializeField] public float wallRunCooldown;
    [SerializeField] public float wallRunTimer;


    [Header("Detection")]
    Vector3 wallNormal;
    private Vector3 wallRunOrigin;
    public Transform leftWallBouncer;
    public Transform rightWallBouncer;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool onLeftWall;
    public bool onRightWall;
    public bool wallRunReady;

    [Header("Detection")]
    private FPSController pm;

    private void Awake()
    {
        pm = GetComponent<FPSController>();
        leftWallBouncer = GameObject.Find("LeftBounce").GetComponent<Transform>();
        rightWallBouncer = GameObject.Find("RightBounce").GetComponent<Transform>();
        wallRunOrigin = GameObject.Find("WallRunOrigin").transform.position;

        maxWallRunTimer = wallRunTimer;
        maxWallRunCooldown = wallRunCooldown;
        wallRunCooldownWithPenalty = wallRunCooldown;
    }


    private void Start()
    {
        wallLayer = LayerMask.GetMask("Wall");

        wallRunReady = true;
    }


    private void ManageWallRunCountdown()
    {
        // While wallrunning
        if(wallRunTimer >= 0 && isWallRunning)
        {
            wallRunTimer -= 1f * Time.deltaTime;
        }

        // Once Timer is done
        else
        {
            ExitWallRun();
            wallRunTimer = maxWallRunTimer;
        }
    }


    public void ManageWallRunCooldown()
    {
        // Once the player is off the wall
        if(!isWallRunning && !wallRunReady && wallRunCooldown >= 0)
        {
            wallRunCooldown -= 1f * Time.deltaTime;
        }

        else
        {
            wallRunCooldown = maxWallRunCooldown;
            wallRunReady = true;
        }
    }


    private void FixedUpdate()
    {
        if (pm.currentState == PlayerState.STATE_INAIR || pm.currentState == PlayerState.STATE_WALLRUN)
        {
            CheckWallRun();
        }
    }


    public void CheckWallRun()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 1f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 1f, wallLayer);

        if(!onRightWall && !onLeftWall && isWallRunning)
        {
            BounceOffWall(0f);
        }

        if ((onRightWall || onLeftWall) && !isWallRunning && !pm.isGrounded && wallRunReady)
        {
            //Debug.Log("SHOULD BE WALLRUNNING");
            CommenceWallRun();
        }

        return;
    }


    public void BounceOffWall(float forwardDirection)
    {
        Vector3 wallJumpDirection;
        wallJumpDirection = Vector3.zero;

        if(onLeftWall)
        {
            wallJumpDirection = leftWallBouncer.transform.position;
        }
        else if(onRightWall)
        {
            wallJumpDirection = rightWallBouncer.transform.position;
            
        }
        else
        {
            wallJumpDirection = pm.forwardOrientation;
        }

        //wallJumpDirection.z = forwardDirection;
        wallJumpDirection.y = Mathf.Sqrt(wallBounceForce * 2.0f * pm.gravity);
        pm.moveDirection = wallJumpDirection;

        ExitWallRun();
    }


    public void ExitWallRun()
    {
        isWallRunning = false;
        wallRunReady = false;
    }


    public void CommenceWallRun()
    {
        isWallRunning = true;

        Vector3 wallRunDirection;
        float wallRunSpeed;

        Vector2 movementInput = pm.moveAction.ReadValue<Vector2>();
        float movementX = movementInput.x;
        float movementY = movementInput.y;


        // if player stops moving forward or landed on ground
        if (movementY <= 0 || pm.isGrounded)
        {
            ExitWallRun();
            return;
        }

        wallRunSpeed = wallrunForce;
        wallRunDirection = pm.forwardOrientation * wallRunSpeed;
        wallRunDirection.y = wallrunGravity;

        //ManageWallRunCountdown();

        pm.characterController.Move(wallRunDirection * Time.deltaTime);

        return;
    }


    private void ResetWallrunCooldown()
    {
        wallRunCooldown = maxWallRunCooldown;
    }
}
