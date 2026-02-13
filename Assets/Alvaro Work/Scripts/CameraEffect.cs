using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraEffect : MonoBehaviour
{
    [Header("Camera Bob Parameters")]
    [Range(0, 0.1f)] public float amount = 0.005f;
    [Range(1f, 30f)] public float frequency = 16f;
    [Range(10f, 100f)] public float smooth = 20f;

    [Header("Camera Tilt Parameters")]
    [Range(0f, 0.5f)] public float tiltAmount = 0.075f;
    [Range(0f, 1f)] public float tiltSpeed = 0.4f;
    [Range(0f, 3f)] public float tiltResetSpeed = 1.5f;

    [Header("Camera Shake Parameters")]
    [Range(0f, 1f)] public float shakeDuration = 0.5f;
    public AnimationCurve curve;
    [Range(0f, 1f)] public float shakeIntensity = 0.05f;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraHolder;

    private const float dampenHeadBobAmount = 9f;

    public bool start = false;

    Vector3 startPosition;
    Quaternion startRotation;

    [SerializeField] FPSController FPSController;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        FPSController = GameObject.Find("Player").GetComponentInParent<FPSController>();
        _camera = GameObject.Find("Main Camera").GetComponent<Transform>();
        _cameraHolder = GameObject.Find("Cam Holder").GetComponent<Transform>();
    }


    void Update()
    {
        if(!FPSController.playerHud.isPaused)
        {
            if(FPSController.currentState != FPSController.PlayerState.STATE_SLIDE)
            {
                CheckForInput();

                StopHeadbob();
                StopSwayCamera();
            }
  
        }
        
    }


    private void CheckForInput()
    {
        float inputMagnitide = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        float horizontalAxis = Input.GetAxis("Horizontal");
        StartSwayCamera(horizontalAxis);

        if (inputMagnitide > 0 && FPSController.isGrounded)
        {
            StartHeadbob();
        }
    }

    
    private Vector3 StartHeadbob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * (amount * FPSController.characterController.velocity.magnitude / dampenHeadBobAmount);
        pos.x += Mathf.Cos(Time.time * frequency / 3) * (amount * FPSController.characterController.velocity.magnitude / dampenHeadBobAmount);

        transform.localPosition += pos;

        return pos;
    }

    // https://www.youtube.com/watch?v=BQGTdRhGmE4
    IEnumerator CameraShakeEvent()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / shakeDuration);
            transform.localPosition = Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }


    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }


    private void StopHeadbob()
    {
        if (transform.localPosition == startPosition) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, smooth * Time.deltaTime);
    }

/// <summary>
/// To sway left: make sure amount is negative. To sway right: make sure amount is positive. amount influence the degree in which the camera will sway.
/// </summary>
/// <param name="horizontal"></param>
/// <returns></returns>
    public Quaternion StartSwayCamera(float horizontal)
    {
        Quaternion rot = Quaternion.identity;
        if(horizontal < 0)
            rot.z = Mathf.Lerp(transform.localRotation.z, tiltAmount * horizontal, Time.deltaTime * tiltSpeed);
        else if (horizontal > 0)
            rot.z = Mathf.Lerp(transform.localRotation.z, -Mathf.Abs(tiltAmount * horizontal), Time.deltaTime * tiltSpeed);
        else
            return rot;

            transform.localRotation = rot;

        return rot;
    }


    public Quaternion StartSlidingCameraTilt()
    {
        Quaternion rot = Quaternion.identity;
        rot.z = Mathf.Lerp(transform.localRotation.z, tiltAmount, Time.deltaTime * tiltSpeed);
        transform.localRotation = rot;

        return rot;
    }


    private void StopSwayCamera()
    {
        if (transform.localRotation == startRotation) return;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, tiltResetSpeed * Time.deltaTime);
    }


    // To be used for sliding
    public void ShakeCamera()
    {
       if (FPSController.currentState == FPSController.PlayerState.STATE_SLIDE)
        {
            transform.localPosition = Random.insideUnitSphere * shakeIntensity;
        }
        else
        {
            transform.position = startPosition;
        }
    }

    public void ShakeCamera(float velocity)
    {
        Vector3 pos = Vector3.zero;

        if (FPSController.currentState != FPSController.PlayerState.STATE_INAIR)
        {
            pos.y += Mathf.Sin(Time.time * frequency) * (amount * FPSController.characterController.velocity.magnitude / dampenHeadBobAmount);
            transform.localPosition += pos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, smooth * Time.deltaTime);
        }

    }
}