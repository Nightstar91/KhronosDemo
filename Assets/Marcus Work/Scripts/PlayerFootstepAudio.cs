using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootstepAudio : MonoBehaviour
{
    // ===== FMOD EVENTS =====
    [Header("FMOD Events")]
    [SerializeField] private EventReference footstepEvent;  // The FMOD event for footsteps (event:/Player/Footsteps)
    [SerializeField] private EventReference landEvent;      // The FMOD event for landings (event:/Player/Land)
    [SerializeField] private string surfaceParameterName = "SurfaceTerrain";  // Name of the FMOD parameter that defines surface type

    // ===== SETTINGS =====
    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.5f;     // Time between footsteps while moving
    [SerializeField] private float minMoveSpeed = 0.1f;     // Minimum player movement speed required to trigger footsteps
    [SerializeField] private float raycastDistance = 1.2f;  // How far below the player to check for ground
    [SerializeField] private LayerMask groundMask;          // Which layers count as ground for surface detection

    // ===== INTERNAL STATE =====
    private CharacterController controller;  // Reference to the player’s CharacterController
    private Vector3 previousPosition;        // Used to measure player movement between frames
    private float stepTimer;                 // Counts down between footsteps
    private bool wasGrounded;                // Tracks if the player was grounded in the previous frame
    private int currentSurfaceIndex;         // Stores the current detected surface type (FMOD parameter value)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        previousPosition = transform.position;
        stepTimer = stepInterval;
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        float speed = Vector3.Distance(transform.position, previousPosition) / Time.deltaTime;

        DetectSurface();  // Detect which surface the player is on

        // --- LANDING SOUND ---
        if (isGrounded && !wasGrounded)
        {
            PlayLandingSound();
        }

        // --- FOOTSTEP LOOP ---
        // Only play footsteps if grounded, moving fast enough, and player is giving input
        if (isGrounded && speed > minMoveSpeed && IsPlayerWalking())
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer when player stops or is in air
            stepTimer = stepInterval;
        }

        wasGrounded = isGrounded;
        previousPosition = transform.position;
    }

    // ===== INPUT CHECK =====
    private bool IsPlayerWalking()
    {
        // Detect movement input (WASD / Arrow keys)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        // Player is "walking" if there's movement input
        return inputX != 0f || inputZ != 0f;
    }

    // ===== SURFACE DETECTION =====
    private void DetectSurface()
    {
        // Raycast slightly below player to detect what surface they’re standing on
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
        {
            string tag = hit.collider.tag;

            // Assign an index based on tag (used by FMOD parameter)
            switch (tag)
            {
                case "LabFloor":
                    currentSurfaceIndex = 0;
                    break;

                // Undefined surfaces for now
                case "UndefinedSurface1":
                    currentSurfaceIndex = 1;
                    break;

                case "UndefinedSurface2":
                    currentSurfaceIndex = 2;
                    break;

                default:
                    currentSurfaceIndex = 0; // Default to LabFloor
                    break;
            }

            Debug.DrawLine(transform.position + Vector3.up * 0.2f, hit.point, Color.green);
        }
    }

    // ===== PLAY FOOTSTEP SOUND =====
    private void PlayFootstepSound()
    {
        EventInstance stepInstance = RuntimeManager.CreateInstance(footstepEvent);
        stepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        stepInstance.setParameterByName(surfaceParameterName, (float)currentSurfaceIndex);
        stepInstance.start();
        stepInstance.release();
    }

    // ===== PLAY LANDING SOUND =====
    private void PlayLandingSound()
    {
        EventInstance landInstance = RuntimeManager.CreateInstance(landEvent);
        landInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        landInstance.setParameterByName(surfaceParameterName, (float)currentSurfaceIndex);
        landInstance.start();
        landInstance.release();
    }
}
