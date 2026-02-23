using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(WallRunning))]
public class WallrunAudio : MonoBehaviour
{
    [Header("FMOD Events")]
    [SerializeField] private EventReference wallrunLoopEvent;
    [SerializeField] private EventReference wallFootstepEvent;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.35f;

    private WallRunning wallrun;

    private EventInstance wallrunInstance;
    private bool wallrunPlaying = false;

    private float footstepTimer = 0f;

    private const string PARAM_NAME = "WallrunDirection";

    // Parameter Values
    private const float NA = 0f;
    private const float RIGHT = 1f;
    private const float LEFT = 2f;

    private void Awake()
    {
        wallrun = GetComponent<WallRunning>();
    }

    private void Update()
    {
        HandleWallrunAudio();
    }

    private void HandleWallrunAudio()
    {
        if (wallrun.isWallRunning)
        {
            float directionValue = GetDirectionValue();

            // Start loop if needed
            if (!wallrunPlaying)
            {
                wallrunInstance = RuntimeManager.CreateInstance(wallrunLoopEvent);
                wallrunInstance.start();
                wallrunPlaying = true;
            }

            // Update loop parameter
            wallrunInstance.setParameterByName(PARAM_NAME, directionValue);

            // Handle footsteps
            HandleFootsteps(directionValue);
        }
        else
        {
            if (wallrunPlaying)
            {
                // Reset parameter before stopping
                wallrunInstance.setParameterByName(PARAM_NAME, NA);

                wallrunInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                wallrunInstance.release();

                wallrunPlaying = false;
            }

            footstepTimer = 0f;
        }
    }

    private void HandleFootsteps(float directionValue)
    {
        footstepTimer += Time.deltaTime;

        if (footstepTimer >= footstepInterval)
        {
            footstepTimer = 0f;

            EventInstance footstepInstance = RuntimeManager.CreateInstance(wallFootstepEvent);
            footstepInstance.setParameterByName(PARAM_NAME, directionValue);
            footstepInstance.start();
            footstepInstance.release(); // one-shot
        }
    }

    private float GetDirectionValue()
    {
        if (wallrun.onRightWall)
            return RIGHT;

        if (wallrun.onLeftWall)
            return LEFT;

        return NA;
    }
}