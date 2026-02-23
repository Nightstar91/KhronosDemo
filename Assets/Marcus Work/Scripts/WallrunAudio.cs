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

    private void LateUpdate()
    {
        // Keep 3D position updated while playing
        if (wallrunPlaying)
        {
            wallrunInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    private void HandleWallrunAudio()
    {
        if (wallrun.isWallRunning)
        {
            float directionValue = GetDirectionValue();

            if (!wallrunPlaying)
            {
                wallrunInstance = RuntimeManager.CreateInstance(wallrunLoopEvent);

                //   initial 3D attributes BEFORE starting
                wallrunInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

                wallrunInstance.start();
                wallrunPlaying = true;
            }

            wallrunInstance.setParameterByName(PARAM_NAME, directionValue);

            HandleFootsteps(directionValue);
        }
        else
        {
            if (wallrunPlaying)
            {
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

            //Set 3D position for one-shot
            footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

            footstepInstance.setParameterByName(PARAM_NAME, directionValue);

            footstepInstance.start();
            footstepInstance.release();
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