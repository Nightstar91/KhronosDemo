using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public InputAction lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //float mouseX = lookAction.
    }

    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
    }

    private void OnEnable()
    {
        lookAction.Enable();
    }


    private void OnDisable()
    {
        lookAction.Disable();
    }
}
