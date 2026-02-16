using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class HeadTurnAudioThreshold : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private InputActionReference lookAction;

    [Header("FMOD")]
    [SerializeField] private EventReference headTurnEvent;
    [SerializeField] private string parameterName = "TurnSpeed";

    [Header("Turn Settings")]
    [SerializeField] private float turnThreshold = 150f; // degrees/sec to trigger sound
    [SerializeField] private float maxTurnSpeed = 1200f;  // for scaling volume
    [SerializeField] private float smoothing = 10f;

    private EventInstance headTurnInstance;
    private Quaternion lastRotation;
    private float smoothedSpeed;
    private bool isPlaying = false;

    void Start()
    {
        if (playerTransform == null)
            playerTransform = transform;

        lastRotation = playerTransform.rotation;

        // Create instance but do NOT start yet
        headTurnInstance = RuntimeManager.CreateInstance(headTurnEvent);
    }

    void Update()
    {
        UpdateAngularSpeed();
    }

    private void UpdateAngularSpeed()
    {
        Quaternion currentRotation = playerTransform.rotation;

        // Degrees rotated this frame
        float angle = Quaternion.Angle(lastRotation, currentRotation);

        // Convert to degrees/sec
        float angularSpeed = angle / Time.deltaTime;

        // Smooth to avoid jitter
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, angularSpeed, Time.deltaTime * smoothing);

        // Only play if above threshold
        if (smoothedSpeed >= turnThreshold)
        {
            float normalizedSpeed = Mathf.InverseLerp(turnThreshold, maxTurnSpeed, smoothedSpeed);
            normalizedSpeed = Mathf.Clamp01(normalizedSpeed);

            // Start event if not already playing
            if (!isPlaying)
            {
                headTurnInstance.start();
                isPlaying = true;
            }

            // Update FMOD parameter to scale volume/pitch
            headTurnInstance.setParameterByName(parameterName, normalizedSpeed);
        }
        else
        {
            // Stop event if currently playing
            if (isPlaying)
            {
                headTurnInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isPlaying = false;
            }
        }

        lastRotation = currentRotation;
    }

    private void OnDestroy()
    {
        headTurnInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        headTurnInstance.release();
    }
}