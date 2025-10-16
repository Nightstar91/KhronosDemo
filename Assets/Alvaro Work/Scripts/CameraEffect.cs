using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraEffect : MonoBehaviour
{
    [Header("Camera Bob Parameters")]
    [Range(0, 0.1f)] public float amount = 0.008f;
    [Range(1f, 30f)] public float frequency = 12f;
    [Range(10f, 100f)] public float smooth = 20f;

    [Header("Camera Tilt Parameters")]
    [Range(0f, 0.5f)] public float tiltAmount = 0.07f;
    [Range(0f, 1f)] public float tiltSpeed = 0.4f;
    [Range(0f, 3f)] public float tiltResetSpeed = 1.5f;

    [Header("Camera Shake Parameters")]
    [Range(0f, 1f)] public float shakeDuration = 0.5f;
    public AnimationCurve curve;
    [Range(10f, 100f)] public float shakeIntensity;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraHolder;

    public bool start = false;

    Vector3 startPosition;
    Quaternion startRotation;

    private FPSController FPSController;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        FPSController = GetComponentInParent<FPSController>();
        _camera = GameObject.Find("Main Camera").GetComponent<Transform>();
        _cameraHolder = GameObject.Find("Cam Holder").GetComponent<Transform>();
    }


    void Update()
    {
        if(!FPSController.playerHud.isPaused)
        {
            CheckForInput();
            
            StopHeadbob();
            StopSwayCamera();

            if (!FPSController.isGrounded)
            {
                if (FPSController.isGrounded)
                {
                    StartCoroutine(CameraShakeEvent());
                }
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
        pos.y += Mathf.Sin(Time.time * frequency) * amount;
        pos.x += Mathf.Cos(Time.time * frequency / 3) * amount;

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