using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wallLayer;
    public bool isWallRunning;
    public float wallrunForce = 7f;
    public float wallrunGravity = 5f;
    private float maxWallRunCooldown;
    private float maxWallRunTimer;
    [SerializeField] public float wallBounceForce = 1f;
    [SerializeField] public float wallRunCooldown = 1f;
    [SerializeField] public float wallRunTimer;


    [Header("Detection")]
    Vector3 wallNormal;
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

        maxWallRunTimer = wallRunTimer;
        maxWallRunCooldown = wallRunCooldown;
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


    public void CheckWallRun()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.75f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.75f, wallLayer);

        if(onRightWall)
        {
            wallNormal = rightWallHit.normal;
        }
        if(onLeftWall)
        {
            wallNormal = leftWallHit.normal;
        }

        if((onRightWall || onLeftWall) && !isWallRunning && !pm.isGrounded && wallRunReady)
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
            wallJumpDirection= rightWallBouncer.transform.position;
            
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
}
