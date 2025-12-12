using UnityEngine;
using UnityEngine.Rendering;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public float wallrunForce;
    public float maxWallRunTime;
    public float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Detection")]
    public Transform orientation;
    private FPSController pm;
    private CharacterController cc;

    private void Start()
    {
        whatIsWall = LayerMask.GetMask("Wall");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {

    }
}
