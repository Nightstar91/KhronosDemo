using UnityEngine;
using UnityEngine.Rendering;

public class CameraEffect : MonoBehaviour
{
    [Header("Camera Sway Parameters")]
    [Range(0.001f, 0.01f)] public float amount = 0.1f;
    [Range(1f, 30f)] public float frequency = 15.0f;
    [Range(10f, 100f)] public float smooth = 10.0f;

    [Header("Camera Tilt Parameters")]
    [Range(0f, 0.5f)] public float tiltAmount = 0.5f;
    [Range(0f, 1f)] public float tiltSpeed = 0.4f;
    [Range(0f, 3f)] public float tiltResetSpeed = 1.5f;

    [Header("Camera Shake Parameters")]
    [Range(0f, 1f)] public float shakeDuration = 0.5f;
    [Range(10f, 100f)] public float shakeIntensity;

    Vector3 startPosition;
    Quaternion startRotation;

    private FPSController FPSController;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        FPSController = GetComponentInParent<FPSController>();
    }


    void Update()
    {
        CheckForInput();
        StopHeadbob();
        StopSwayCamera();

         if (FPSController.jumpAction.IsPressed() && !FPSController.isGrounded)
        {
            if (FPSController.CheckIfGrounded())
            {
                StartCameraShake();
            }
        }
    }


    private void CheckForInput()
    {
        float inputMagnitide = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        float horizontalAxis = Input.GetAxis("Horizontal");
        StartSwayCamera(horizontalAxis);

        if (inputMagnitide > 0)
        {
            StartHeadbob();
        }
    }



    
    private Vector3 StartHeadbob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);

        transform.localPosition += pos;

        return pos;
    }


    private Vector3 StartCameraShake()
    {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * shakeIntensity * 1.4f, shakeDuration * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * shakeIntensity * 1.4f, shakeDuration * Time.deltaTime);

        transform.localPosition += pos;

        return pos;
    }


    private void StopHeadbob()
    {
        if (transform.localPosition == startPosition) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, 1 * Time.deltaTime);
    }


    private Quaternion StartSwayCamera(float horizontal)
    {
        Quaternion rot = Quaternion.identity;
        if(horizontal < 0)
            rot.z = Mathf.Lerp(transform.localRotation.z, tiltAmount, Time.deltaTime * tiltSpeed);
        else if (horizontal > 0)
            rot.z = Mathf.Lerp(transform.localRotation.z, -Mathf.Abs(tiltAmount), Time.deltaTime * tiltSpeed);
        else
            return rot;

            transform.localRotation = rot;

        return rot;
    }


    private void StopSwayCamera()
    {
        if (transform.localRotation == startRotation) return;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, tiltResetSpeed * Time.deltaTime);
    }
}