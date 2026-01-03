using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wallLayer;
    public bool isWallRunning;
    public float wallrunForce;
    public float maxWallRunTime;
    public float wallRunTimer;
    public int jumpCharge;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    Vector3 wallNormal;
    Vector3 yVelocity;
    Vector3 forwardDirection;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool onLeftWall;
    public bool onRightWall;

    [Header("Detection")]
    public Transform orientation;
    private FPSController pm;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        pm = GetComponent<FPSController>();
        orientation = GameObject.Find("Player").GetComponent<Transform>();
    }


    private void Start()
    {
        wallLayer = LayerMask.GetMask("Wall");
    }


    private void CheckWallRun()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallLayer);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.7f, wallLayer);

        if((onRightWall || onLeftWall) && !isWallRunning)
        {
            StartWallRun();
        }
        if((!onRightWall || !onLeftWall) && isWallRunning)
        {
            ExitWallRun();
        }
    }


    private void WallRunMovement()
    {
        
    }


    private void StartWallRun()
    {
        isWallRunning = true;
        jumpCharge = 1;
        pm.IncreaseBaseSpeed(5f); 
        yVelocity = new Vector3 (0f, 0f, 0f);

        wallNormal = onLeftWall ? leftWallHit.normal : rightWallHit.normal;
        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if(Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }
    }

    private void ExitWallRun()
    {
        isWallRunning = false;
    }

}
