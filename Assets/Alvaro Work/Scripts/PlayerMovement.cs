using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Parameter")]
    private InputAction move_action;
    private InputAction look_action;
    private InputAction jump_action;


    private Vector2 movement_amount;
    private Vector2 look_amount;

    [Header("Movement Parameter")]
    [SerializeField] float rotation = 5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float jump_force = 5f;


    [SerializeField] GameObject player_object;
    [SerializeField] Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_object = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        move_action.Enable();
        look_action.Enable();
        jump_action.Enable();
    }


    private void OnDisable()
    {
        move_action.Disable();
        look_action.Disable();
        jump_action.Disable();
    }


    private void Awake()
    {
        move_action = InputSystem.actions.FindAction("Move");
        look_action = InputSystem.actions.FindAction("Look");
        jump_action = InputSystem.actions.FindAction("Jump");
    }


    // Update is called once per frame
    void Update()
    {
        movement_amount = move_action.ReadValue<Vector2>();
        look_amount = look_action.ReadValue<Vector2>();

        if (jump_action.WasPressedThisFrame())
        {
            Jump();
        }
    }


    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }


    private void Jump()
    {
        rb.AddForceAtPosition(new Vector3(0, jump_force, 0), Vector3.up, ForceMode.Impulse);
    }


    private void Walking()
    {
        rb.MovePosition(rb.position + transform.forward * movement_amount.y * speed * Time.deltaTime);
    }


    private void Rotating()
    {
        if(movement_amount.y != 0)
        {
            float rotation_amount = look_amount.x * rotation * Time.deltaTime;
            Quaternion delta_quaternion = Quaternion.Euler(0, rotation_amount, 0);
            rb.MoveRotation(rb.rotation * delta_quaternion);
        }
    }


}
