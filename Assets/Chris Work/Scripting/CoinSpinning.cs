using UnityEngine;

public class CoinSpinner : MonoBehaviour
{
    // Adjust this speed in the inspector
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate around the Y-axis 
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
