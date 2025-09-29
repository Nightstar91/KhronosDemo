using UnityEngine;
using UnityEngine.Rendering;

public class HeadBobController : MonoBehaviour
{
    [Range(0.001f, 0.01f)] public float amount = 0.002f;
    [Range(1f, 30f)] public float frequency = 10.0f;
    [Range(10f, 100f)] public float smooth = 10.0f;

    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.localPosition;
    }


    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadbob();
    }


    private void CheckForHeadbobTrigger()
    {
        float inputMagnitide = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
    
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


    private void StopHeadbob()
    {
        if (transform.localPosition == startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }
}