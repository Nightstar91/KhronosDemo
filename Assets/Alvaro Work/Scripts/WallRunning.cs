using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wallLayer;
    private const float wallBounceForce = 1f;
    public bool isWallRunning;
    public float wallrunForce = 0.05f;
    public float wallrunGravity = 5f;
    private float maxWallRunCooldown = 1f;
    private float maxWallRunTime = 2f;
    [SerializeField] public float wallRunCooldown;
    [SerializeField] public float wallRunTimer;


    [Header("Detection")]
    Vector3 wallNormal;
    public Transform leftWallRunDetector;
    public Transform rightWallRunDetector;
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
        leftWallRunDetector = GameObject.Find("LeftRaycast").GetComponent<Transform>();
        rightWallRunDetector = GameObject.Find("RightRaycast").GetComponent<Transform>();

        wallRunCooldown = maxWallRunCooldown;
        wallRunTimer = maxWallRunTime;
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
            wallRunTimer = maxWallRunTime;
        }
    }


    private void ManageWallRunCooldown()
    {
        // Once the player is off the wall
        if(wallRunCooldown >= 0 && !isWallRunning)
        {
            wallRunReady = false;
            wallRunCooldown -= 1f * Time.deltaTime;
        }

        else if (isWallRunning )
        {
            wallRunTimer = maxWallRunCooldown;
        }
    }


    public void CheckWallRun()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.7f, wallLayer);

        if(onRightWall)
        {
            wallNormal = rightWallHit.normal;
        }
        if(onLeftWall)
        {
            wallNormal = leftWallHit.normal;
        }

        if((onRightWall || onLeftWall) && !isWallRunning && !pm.isGrounded)
        {
            Debug.Log("SHOULD BE WALLRUNNING");
            CommenceWallRun();
        }

        return;
    }


    public void BounceOffWall()
    {
        Vector3 wallJumpDirection;
        wallJumpDirection = Vector3.zero;

        

        pm.characterController.Move(wallJumpDirection);
    }


    public void ExitWallRun()
    {
        isWallRunning = false;
        //ManageWallRunCooldown();
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

        pm.characterController.Move(wallRunDirection);

        return;
    }
}
