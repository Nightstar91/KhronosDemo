using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    private void Start()
    {
        cameraPosition = GameObject.Find("CameraPos").GetComponent<Transform>();
    }


    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
