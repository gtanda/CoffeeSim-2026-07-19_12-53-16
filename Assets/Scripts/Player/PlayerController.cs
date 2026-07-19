using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerControls _playerControls;


    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;


    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -9.81f;


    private Vector2 moveInput;
    private Vector2 lookInput;

    private float currentSpeed;

    private bool isSprinting;

    private Vector3 velocity;


    public Vector2 LookInput => lookInput;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _playerControls = new PlayerControls();

        currentSpeed = walkSpeed;

        SetupInput();
    }


    private void SetupInput()
    {
        // Movement
        _playerControls.Player.Movement.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        _playerControls.Player.Movement.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };


        // Look
        _playerControls.Player.Rotation.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();
        };

        _playerControls.Player.Rotation.canceled += ctx =>
        {
            lookInput = Vector2.zero;
        };


        // Sprint
        _playerControls.Player.Sprint.performed += ctx =>
        {
            isSprinting = true;
            currentSpeed = sprintSpeed;
        };

        _playerControls.Player.Sprint.canceled += ctx =>
        {
            isSprinting = false;
            currentSpeed = walkSpeed;
        };


        // Jump
        _playerControls.Player.Jump.performed += ctx =>
        {
            Jump();
        };
    }


    private void OnEnable()
    {
        _playerControls.Enable();
    }


    private void OnDisable()
    {
        _playerControls.Disable();
    }


    private void Update()
    {
        HandleMovement();
        HandleGravity();
    }


    private void HandleMovement()
    {
        Vector3 move =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;


        _characterController.Move(
            move * currentSpeed * Time.deltaTime
        );
    }


    private void HandleGravity()
    {
        if (_characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }


        velocity.y += gravity * Time.deltaTime;


        _characterController.Move(
            velocity * Time.deltaTime
        );
    }


    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }
    }
}