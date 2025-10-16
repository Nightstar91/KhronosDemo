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
    [SerializeField] private LayerMask groundMask;          // Which layers count as “ground” for surface detection

    // ===== INTERNAL STATE =====
    private CharacterController controller;  // Reference to the player’s CharacterController
    private Vector3 previousPosition;        // Used to measure player movement between frames
    private float stepTimer;                 // Counts down between footsteps
    private bool wasGrounded;                // Tracks if the player was grounded in the previous frame
    private int currentSurfaceIndex;         // Stores the current detected surface type (FMOD parameter value)

    // ===== INITIALIZATION =====
    void Start()
    {
        controller = GetComponent<CharacterController>();  // Grab CharacterController for movement/ground checks
        previousPosition = transform.position;             // Store initial position
        stepTimer = stepInterval;                          // Initialize the step timer
    }

    // ===== MAIN UPDATE LOOP =====
    void Update()
    {
        bool isGrounded = controller.isGrounded;  // Check if player is standing on ground
        float speed = Vector3.Distance(transform.position, previousPosition) / Time.deltaTime;  // Calculate current move speed

        DetectSurface();  // Update surface type by raycasting downward

        // --- LANDING SOUND ---
        // If the player just became grounded this frame (was in air last frame), play a landing sound
        if (isGrounded && !wasGrounded)
        {
            PlayLandingSound();
        }

        // --- FOOTSTEP LOOP ---
        // If grounded and moving faster than minMoveSpeed, count down the timer
        if (isGrounded && speed > minMoveSpeed)
        {
            stepTimer -= Time.deltaTime;

            // When timer hits zero, play a footstep and reset timer
            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer when player stops or is in air (so footsteps don’t carry over)
            stepTimer = stepInterval;
        }

        // Store current state for next frame comparison
        wasGrounded = isGrounded;
        previousPosition = transform.position;
    }

    // ===== GROUND / SURFACE DETECTION =====
    private void DetectSurface()
    {
        // Raycast slightly below player to detect what surface they’re standing on
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
        {
            string tag = hit.collider.tag;  // Get the tag of the object we hit

            // Assign an index based on tag (used by FMOD parameter)
            switch (tag)
            {
                case "LabFloor":
                    currentSurfaceIndex = 0;
                    break;

                // Undefined surfaces for now (you can fill these in later)
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

            // Visualize the ray in the Scene view for debugging
            Debug.DrawLine(transform.position + Vector3.up * 0.2f, hit.point, Color.green);
            // Debug.Log($"Surface: {tag} | Index: {currentSurfaceIndex}");
        }
    }

    // ===== PLAY FOOTSTEP SOUND =====
    private void PlayFootstepSound()
    {
        // Create a new FMOD event instance for the footstep
        EventInstance stepInstance = RuntimeManager.CreateInstance(footstepEvent);

        // Attach 3D position and parameter data
        stepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        stepInstance.setParameterByName(surfaceParameterName, (float)currentSurfaceIndex);

        // Play and release immediately (since it’s a one-shot)
        stepInstance.start();
        stepInstance.release();
    }

    // ===== PLAY LANDING SOUND =====
    private void PlayLandingSound()
    {
        // Create a new FMOD event instance for the landing
        EventInstance landInstance = RuntimeManager.CreateInstance(landEvent);

        // Attach 3D position and parameter data
        landInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        landInstance.setParameterByName(surfaceParameterName, (float)currentSurfaceIndex);

        // Play and release immediately (since it’s a one-shot)
        landInstance.start();
        landInstance.release();
    }
}